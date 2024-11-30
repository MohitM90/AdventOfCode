using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Day11
    {
        internal static int PuzzleA(string input)
        {
            int sum = 0;
            var inputs = Expand(input.Split("\r\n").ToList());
            var matrix = GetMatrix(inputs);
            var galaxies = GetGalaxies(matrix);
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += GetShortestPath(matrix, galaxies[i], galaxies[j]);
                }
            }
            
            return sum;
        }
        internal static async Task<long> PuzzleB(string input)
        {
            long sum = 0;
            var inputs = Expand2(input.Split("\r\n").ToList());
            var matrix = GetMatrix(inputs);
            var galaxies = GetGalaxies(matrix);
            List<Task<long>> tasks = new();
            Enumerable.Range(0, galaxies.Count).Select(i => 
                Enumerable.Range(i + 1, galaxies.Count - i - 1)
                    .Select(j => (i, j))).ToList().ForEach(x => x.ToList().ForEach(y => 
                        tasks.Add(Task.Run(() => GetShortestPath2(matrix, galaxies[y.i], galaxies[y.j])))));
            //for (int i = 0; i < galaxies.Count; i++)
            //{
            //    for (int j = i + 1; j < galaxies.Count; j++)
            //    {
                    
            //        tasks.Add(Task.Run(() => GetShortestPath2(matrix, galaxies[i], galaxies[j])));
            //        //sum += GetShortestPath2(matrix, galaxies[i], galaxies[j]);
            //    }
            //}
            long[] result = await Task.WhenAll(tasks);
            sum = result.Sum();

            return sum;
        }

        private static long GetShortestPath2(char[,] matrix, Galaxy start, Galaxy end)
        {
            if (!IsValidCell(matrix, start.X, start.Y) || !IsValidCell(matrix, end.X, end.Y) || matrix[start.X, start.Y] == '.' || matrix[end.X, end.Y] == '.')
            {
                throw new ArgumentException("Invalid start or end position.");
            }

            var visited = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            var queue = new Queue<(Galaxy, long)>();

            visited[start.X, start.Y] = true;
            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (current, distance) = queue.Dequeue();

                if (current.Equals(end))
                {
                    return distance;
                }

                foreach (var neighbor in GetNeighbors(matrix, current))
                {
                    if (!visited[neighbor.X, neighbor.Y])
                    {
                        visited[neighbor.X, neighbor.Y] = true;
                        var dist = distance + (matrix[neighbor.X, neighbor.Y] == ',' ? 1000000 - 1 : 1);
                        queue.Enqueue((neighbor, dist));
                    }
                }
            }

            // No path found
            return -1;
        }

        private static int GetShortestPath(char[,] matrix, Galaxy start, Galaxy end)
        {
            if (!IsValidCell(matrix, start.X, start.Y) || !IsValidCell(matrix, end.X, end.Y) || matrix[start.X, start.Y] == '.' || matrix[end.X, end.Y] == '.')
            {
                throw new ArgumentException("Invalid start or end position.");
            }

            var visited = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            var queue = new Queue<(Galaxy, int)>();

            visited[start.X, start.Y] = true;
            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (current, distance) = queue.Dequeue();

                if (current.Equals(end))
                {
                    return distance;
                }

                foreach (var neighbor in GetNeighbors(matrix, current))
                {
                    if (!visited[neighbor.X, neighbor.Y])
                    {
                        visited[neighbor.X, neighbor.Y] = true;
                        queue.Enqueue((neighbor, distance + 1));
                    }
                }
            }

            // No path found
            return -1;
        }

        private static List<string> Expand(List<string> input)
        {
            var output = new List<string>(input);
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].All(c => c == '.'))
                {
                    output.Insert(i, new string('.', output[i].Length));
                    i++;
                }
            }
            output = Transpose(output);
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].All(c => c == '.'))
                {
                    output.Insert(i++, new string('.', output[i].Length));
                }
            }
            return output;
        }

        private static List<string> Expand2(List<string> input)
        {
            var output = new List<string>(input);
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].All(c => c is '.' or ','))
                {
                    output.Insert(i, new string(',', output[i].Length));
                    i++;
                }
            }
            output = Transpose(output);
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].All(c => c is '.' or ','))
                {
                    output.Insert(i++, new string(',', output[i].Length));
                }
            }
            return output;
        }
        private static char[,] GetMatrix(List<string> inputs)
        {
            var matrix = new char[inputs.Count, inputs[0].Length];
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs[i].Length; j++)
                {
                    matrix[i, j] = inputs[i][j];
                }
            }
            return matrix;
        }
        private static List<string> Transpose(List<string> input)
        {
            List<string> result = [];
            for (int i = 0; i < input[0].Length; i++)
            {
                string s = "";
                for (int j = 0; j < input.Count; j++)
                {
                    s += input[j][i];
                }
                result.Add(s);
            }
            return result;
        }

        private static List<Galaxy> GetGalaxies(char[,] map)
        {
            var galaxies = new List<Galaxy>();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == '#')
                    {
                        galaxies.Add(new Galaxy(x, y));
                    }
                }
            }
            return galaxies;
        }

        private record Galaxy(int X, int Y)
        {

        }

        private static IEnumerable<Galaxy> GetNeighbors(char[,] matrix, Galaxy position)
        {
            var directions = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

            foreach (var (dx, dy) in directions)
            {
                int newX = position.X + dx;
                int newY = position.Y + dy;

                if (IsValidCell(matrix, newX, newY))
                {
                    yield return new Galaxy(newX, newY);
                }
            }
        }

        private static bool IsValidCell(char[,] matrix, int x, int y)
    {
        return x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
    }
    }
}

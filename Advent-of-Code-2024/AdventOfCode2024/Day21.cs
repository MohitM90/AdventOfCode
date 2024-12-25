using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day21 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n");


        foreach (var sequence in input)
        {
            Console.WriteLine("Sequence: " + sequence);
            var startPosition = 'A';
            var numPadSequences = GetControlSequences(NumPadPaths, sequence, startPosition);

            HashSet<string> dirPadSequences = numPadSequences;
            HashSet<string> resultSequences = [];
            for (int i = 0; i < 2; i++)
            {
                var length = int.MaxValue;
                var iterations = dirPadSequences.Count;

                foreach (var dirPadSequence in dirPadSequences)
                {
                    var tempDirPadSequences = GetControlSequences(DirPadPaths, dirPadSequence, startPosition);
                    if (tempDirPadSequences.Any(x => x.Length < length))
                    {
                        length = tempDirPadSequences.Min(x => x.Length);
                        resultSequences = tempDirPadSequences;
                    }
                    Console.WriteLine("Remaining : " + --iterations);
                }
                dirPadSequences = resultSequences;
            }

            var minSequence = resultSequences.Min(x => x.Length);
            answer += minSequence * long.Parse(sequence.Replace("A", ""));
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n");

        return answer;
    }

    private HashSet<string> GetControlSequences(Dictionary<char, Dictionary<char, Dictionary<char, Key>>> paths, string sequence, char start)
    {
        var currentPosition = start;
        HashSet<string> results = [""];

        foreach (var c in sequence)
        {
            Queue<string> queue = new Queue<string>(results);
            results.Clear();
            while (queue.Count > 0)
            {
                var tempSequence = queue.Dequeue();
                var cSequences = GetControlSequences(paths, tempSequence, c, currentPosition);
                foreach (var cSequence in cSequences)
                {
                    results.Add(cSequence);
                }
            }
            currentPosition = c;
        }

        return results;
    }

    private HashSet<string> GetControlSequences(Dictionary<char, Dictionary<char, Dictionary<char, Key>>> paths, string sequence, char target, char currentPosition)
    {
        HashSet<string> results = [];
        var directions = paths[currentPosition]
            .GroupBy(x => x.Value[target].Distance)
            .MinBy(x => x.Key);

        if (directions == null || !directions.Any())
        {
            return results;
        }

        foreach (var direction in directions)
        {
            var tempSequence = "";
            var position = direction.Value[target];
            while (position.Parent != null)
            {
                tempSequence = position.ParentDirection.Value + tempSequence;
                position = position.Parent;
            }
            tempSequence += 'A';
            results.Add(sequence + tempSequence);
        }

        return results;
    }


    private readonly static char[,] NumPad = new char[4, 3]
{
        { '7', '8', '9' },
        { '4', '5', '6' },
        { '1', '2', '3' },
        { 'x', '0', 'A' }
};

    private readonly static char[,] DirPad = new char[2, 3]
    {
        { 'x', '^', 'A' },
        { '<', 'v', '>' }
    };

    private Dictionary<char, Dictionary<char, Dictionary<char, Key>>> NumPadPaths = GetAllPaths(NumPad);
    private Dictionary<char, Dictionary<char, Dictionary<char, Key>>> DirPadPaths = GetAllPaths(DirPad);

    private static Dictionary<char, Dictionary<char, Dictionary<char, Key>>> GetAllPaths(char[,] pad)
    {
        Dictionary<char, Dictionary<char, Dictionary<char, Key>>> paths = [];
        for (int y = 0; y < pad.GetLength(0); y++)
        {
            for (int x = 0; x < pad.GetLength(1); x++)
            {
                if (pad[y, x] == 'x')
                {
                    continue;
                }
                foreach (var direction in new[] { LEFT, DOWN, RIGHT, UP })
                {
                    var start = new Key(pad[y, x], new Position(x, y), 0, null, direction);
                    var keys = GetShortestPaths(pad, start);
                    if (!paths.TryGetValue(start.Value, out var tempValue))
                    {
                        paths.Add(start.Value, []);
                    }
                    if (!paths[start.Value].TryGetValue(direction.Value, out var tempValue2))
                    {
                        paths[start.Value].Add(direction.Value, keys);
                    }
                    else
                    {
                        paths[start.Value][direction.Value] = keys;
                    }
                }
                
            }
        }
        return paths;
    }

    private static Dictionary<char, Key> GetShortestPaths(char[,] map, Key start)
    {
        Dictionary<char, Key> keys = [];
        Queue<Key> queue = new();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (keys.TryGetValue(current.Value, out var tempValue))
            {
                if (current.Distance < tempValue.Distance)
                {
                    keys.Remove(current.Value);
                    keys.Add(current.Value, current);
                }
            }
            else
            {
                keys.Add(current.Value, current);
            }
            var neighbors = GetNeighbors(map, current);

            foreach (var neighbor in neighbors)
            {
                if (keys.TryGetValue(neighbor.Value, out var tempValue2))
                {
                    if (neighbor.Distance >= tempValue2.Distance)
                    {
                        continue;
                    }
                }
                queue.Enqueue(neighbor);
            }

        }
        return keys;
    }

    private static List<Key> GetNeighbors(char[,] map, Key current)
    {
        List<Key> neighbors = [];
        foreach (var direction in new[] { LEFT, DOWN, RIGHT, UP })
        {
            var newPosition = new Position(current.Position.X + direction.Delta.X, current.Position.Y + direction.Delta.Y);
            if (newPosition.X < 0 || newPosition.X >= map.GetLength(1) || newPosition.Y < 0 || newPosition.Y >= map.GetLength(0))
            {
                continue;
            }
            if (map[newPosition.Y, newPosition.X] == 'x')
            {
                continue;
            }

            var distance = 1;
            if (current.ParentDirection != null && current.ParentDirection.Value != direction.Value)
            {
                distance++;
            }
            var newKey = new Key(map[newPosition.Y, newPosition.X], newPosition, current.Distance + distance, current, direction);
            neighbors.Add(newKey);
        }
        return neighbors;
    }

    private record Position(int X, int Y);
    private record Direction(char Value, Position Delta);
    private record Key(char Value, Position Position, int Distance, Key? Parent, Direction? ParentDirection);
    private record Key2(char Value, string Path);

    private static readonly Direction UP = new('^', new(0, -1));
    private static readonly Direction DOWN = new('v', new(0, 1));
    private static readonly Direction LEFT = new('<', new(-1, 0));
    private static readonly Direction RIGHT = new('>', new(1, 0));
}
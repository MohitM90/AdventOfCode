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
            var startPosition = 'A';
            var numPadSequences = GetControlSequences(NumPadPaths, sequence, startPosition);

            HashSet<string> dirPadSequences = numPadSequences;

            var minLength = dirPadSequences.Min(x => Solve(DirPadPaths, x, 2, []));
            answer += minLength * long.Parse(sequence.Replace("A", ""));

        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n");


        foreach (var sequence in input)
        {
            var startPosition = 'A';
            var numPadSequences = GetControlSequences(NumPadPaths, sequence, startPosition);

            HashSet<string> dirPadSequences = numPadSequences;

            var minLength = dirPadSequences.Min(x => Solve(DirPadPaths, x, 25, []));
            answer += minLength * long.Parse(sequence.Replace("A", ""));

        }

        return answer;
    }

    private long Solve(Dictionary<char, Dictionary<char, HashSet<string>>> paths, string sequence, int depth, Dictionary<(int Depth, string Sequence), long> cache)
    {
        if (depth == 0)
        {
            return sequence.Length;
        }
        if (cache.TryGetValue((depth, sequence), out var tempValue))
        {
            return tempValue;
        }
        var startPosition = 'A';
        var subSequences = sequence.Split('A').SkipLast(1).ToArray();
        var length = 0L;
        foreach (var subSequence in subSequences)
        {
            var cSequences = GetControlSequences(paths, subSequence + 'A', startPosition);
            length += cSequences.Min(x => Solve(paths, x, depth - 1, cache));
        }
        cache.Add((depth, sequence), length);
        return length;
    }

    private HashSet<string> GetControlSequences(Dictionary<char, Dictionary<char, HashSet<string>>> paths, string sequence, char start)
    {
        var currentPosition = start;
        HashSet<string> results = [""];

        foreach (var c in sequence)
        {
            var cSequences = GetControlSequences(paths, c, currentPosition);
            results = results.SelectMany(x => cSequences.Select(y => x + y)).ToHashSet();
            currentPosition = c;
        }

        return results;
    }

    private HashSet<string> GetControlSequences(Dictionary<char, Dictionary<char, HashSet<string>>> paths, char target, char currentPosition)
    {
        return paths[currentPosition][target];
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

    private Dictionary<char, Dictionary<char, HashSet<string>>> NumPadPaths = GetAllPaths(NumPad);
    private Dictionary<char, Dictionary<char, HashSet<string>>> DirPadPaths = GetAllPaths(DirPad);

    private static Dictionary<char, Dictionary<char, HashSet<string>>> GetAllPaths(char[,] pad)
    {
        Dictionary<char, Dictionary<char, HashSet<string>>> paths = [];
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
                    var shortest = GetShortestPaths(pad, start).ToDictionary(x => x.Key, x => ToPath(x.Value));
                    if (!paths.TryGetValue(start.Value, out var tempValue))
                    {
                        paths.Add(start.Value, []);
                    }
                    foreach (KeyValuePair<char, string> path in shortest)
                    {
                        if (!paths[start.Value].TryGetValue(path.Key, out var tempValue2))
                        {
                            paths[start.Value].Add(path.Key, [path.Value]);
                        }
                        else
                        {
                            paths[start.Value][path.Key].Add(path.Value);
                        }
                    }
                }
                
            }
        }
        return paths;
    }

    private static string ToPath(Key key)
    {
        var path = "";
        while (key.Parent != null)
        {
            path = key.ParentDirection.Value + path;
            key = key.Parent;
        }
        path += 'A';
        return path;
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal partial class Day08 : BaseDay<long>
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");
        ILookup<char, Point> antennas = GetAntennaPositions(inputs);
        char[,] antinodeMap = new char[inputs[0].Length, inputs.Length];
        foreach (var antenna in antennas)
        {
            MarkAntinodes(inputs, antenna.Key, [.. antenna], antinodeMap);
        }

        foreach (var c in antinodeMap)
        {
            if (c == '#')
            {
                answer++;
            }
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");
        ILookup<char, Point> antennas = GetAntennaPositions(inputs);
        char[,] antinodeMap = new char[inputs[0].Length, inputs.Length];
        foreach (var antenna in antennas)
        {
            MarkAntinodes(inputs, antenna.Key, [.. antenna], antinodeMap, true);
        }

        foreach (var c in antinodeMap)
        {
            if (c == '#')
            {
                answer++;
            }
        }

        return answer;
    }


    private static ILookup<char, Point> GetAntennaPositions(string[] inputs)
    {
        return inputs.SelectMany((s, y) => AntennaRegex().Matches(s)
            .Select(m => new { Antenna = m.Value[0], Position = new Point(m.Index, y) }))
            .ToLookup(x => x.Antenna, x => x.Position);
    }

    private static void MarkAntinodes(string[] map, char antenna, IList<Point> points, char[,] antinodeMap, bool withResonancy = false)
    {
        for (int i = 0; i < points.Count; i++)
        {
            var p1 = points[i];
            for (int j = i + 1; j < points.Count; j++)
            {
                var p2 = points[j];
                var dx = p2.X - p1.X;
                var dy = p2.Y - p1.Y;

                if (withResonancy)
                {
                    for (int x = p1.X, y = p1.Y;
                     x >= 0 && x < map[0].Length && y >= 0 && y < map.Length;
                     x += dx, y += dy)
                    {
                        antinodeMap[x, y] = '#';
                    }

                    for (int x = p1.X, y = p1.Y;
                         x >= 0 && x < map[0].Length && y >= 0 && y < map.Length;
                         x -= dx, y -= dy)
                    {
                        antinodeMap[x, y] = '#';
                    }
                    continue;
                }

                var node1 = Point.MinY(p1, p2);
                var node2 = Point.MaxY(p1, p2);
                if (node1 == node2)
                {
                    node1 = Point.MinX(p1, p2);
                    node2 = Point.MaxX(p1, p2);
                }

                var antinode1 = new Point(node1.X - dx, node1.Y - dy);
                var antinode2 = new Point(node2.X + dx, node2.Y + dy);

                if (antinode1.X >= 0 && antinode1.X < map[0].Length && antinode1.Y >= 0 && antinode1.Y < map.Length)
                {
                    antinodeMap[antinode1.X, antinode1.Y] = '#';
                }
                if (antinode2.X >= 0 && antinode2.X < map[0].Length && antinode2.Y >= 0 && antinode2.Y < map.Length)
                {
                    antinodeMap[antinode2.X, antinode2.Y] = '#';
                }
            }
        }
    }

    private record Point(int X, int Y)
    {
        public static Point MinY(Point p1, Point p2) => p1.Y < p2.Y ? p1 : p2;
        public static Point MaxY(Point p1, Point p2) => p1.Y > p2.Y ? p1 : p2;
        public static Point MinX(Point p1, Point p2) => p1.X < p2.X ? p1 : p2;
        public static Point MaxX(Point p1, Point p2) => p1.X > p2.X ? p1 : p2;
    }

    [GeneratedRegex("[A-Za-z0-9]")]
    private static partial Regex AntennaRegex();
}

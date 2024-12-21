using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day20 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var map = Input.Split("\r\n")
            .Select((row, y) =>
                row.Select((column, x) =>
                    new Tile(column, new(x, y)))
                .ToArray()
            );

        var initialMap = map.ToArray();
        var steps = GetStepsMap(initialMap);

        var newMap = map.ToArray();
        var startPos = FindStartPosition(newMap);
        var endPos = FindEndPosition(newMap);
        var cheats = FindCheats(newMap, steps, 2, startPos, endPos);

        answer = cheats.Count;

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var map = Input.Split("\r\n")
            .Select((row, y) => 
                row.Select((column, x) => 
                    new Tile(column, new(x, y)))
                .ToArray()
            );

        var initialMap = map.ToArray();
        var steps = GetStepsMap(initialMap);

        var newMap = map.ToArray();
        var startPos = FindStartPosition(newMap);
        var endPos = FindEndPosition(newMap);
        var cheats = FindCheats(newMap, steps, 20, startPos, endPos);

        answer = cheats.Count;

        return answer;
    }

    private long[,] GetStepsMap(Tile[][] map)
    {
        long steps = 0;
        long[,] fieldSteps = new long[map.Length, map[0].Length];
        var pos = FindStartPosition(map);
        while (map[pos.Y][pos.X].Type != 'E')
        {
            var nextPos = GetNext(map, pos);
            if (nextPos == null)
            {
                break;
            }
            map[pos.Y][pos.X].Visited = true;
            fieldSteps[pos.X, pos.Y] = steps;
            pos = nextPos;
            steps++;
        }
        fieldSteps[pos.X, pos.Y] = steps;
        return fieldSteps;
    }

    private HashSet<Cheat> FindCheats(Tile[][] map, long[,] stepsField, int maxCheatDuration, Position startPos, Position endPos, int threshold = 100)
    {
        HashSet<Cheat> cheats = new HashSet<Cheat>(1000000, new CheatEqualityComparer());
        var stepsToEnd = stepsField[endPos.X, endPos.Y] - stepsField[startPos.X, startPos.Y];

        var pos = startPos;
        var reachablePositions = GetReachablePositions(maxCheatDuration, pos);
        while (map[pos.Y][pos.X].Type != 'E')
        {
            map[pos.Y][pos.X].Visited = true;
            var nextPos = GetNext(map, pos);
            var direction = new Position(nextPos.X - pos.X, nextPos.Y - pos.Y);
            foreach (var reachablePos in reachablePositions)
            {
                if (reachablePos.X < 0 || reachablePos.X >= map[0].Length || reachablePos.Y < 0 || reachablePos.Y >= map.Length)
                {
                    reachablePos.X += direction.X;
                    reachablePos.Y += direction.Y;
                    continue;
                }
                if (stepsField[reachablePos.X, reachablePos.Y] < stepsField[pos.X, pos.Y])
                {
                    reachablePos.X += direction.X;
                    reachablePos.Y += direction.Y;
                    continue;
                }
                if (map[reachablePos.Y][reachablePos.X].Type == '#')
                {
                    reachablePos.X += direction.X;
                    reachablePos.Y += direction.Y;
                    continue;
                }
                var stepsFromCheatPosToEndPos = stepsField[endPos.X, endPos.Y] - stepsField[reachablePos.X, reachablePos.Y];
                var stepsFromStartPosToCheatPos = stepsField[pos.X, pos.Y] + Math.Abs(reachablePos.X - pos.X) + Math.Abs(reachablePos.Y - pos.Y);
                if (stepsToEnd - (stepsFromStartPosToCheatPos + stepsFromCheatPosToEndPos) >= threshold)
                {
                    var cheatPos = reachablePos with { };
                    var cheat = new Cheat(stepsFromStartPosToCheatPos, pos, reachablePos);
                    cheats.Add(cheat);
                }
                reachablePos.X += direction.X;
                reachablePos.Y += direction.Y;
            }
            pos = nextPos;
        }

        return cheats.Distinct(new CheatEqualityComparer()).ToHashSet();
    }

    private HashSet<Position> GetReachablePositions(int maxCheatDuration, Position pos) // Sliding window
    {
        HashSet<Position> reachablePositions = [];

        foreach (var yDirection in new[] { UP, DOWN })
        {
            foreach (var xDirection in new[] { LEFT, RIGHT })
            {
                for (int y = 0; y <= maxCheatDuration; y++)
                {
                    for (int x = 0; x <= maxCheatDuration - y; x++)
                    {
                        var nextPos = new Position(pos.X + xDirection.X * x, pos.Y + yDirection.Y * y);
                        reachablePositions.Add(nextPos);
                    }
                }
            }
        }

        return reachablePositions;
    }

    private Position FindStartPosition(Tile[][] map)
    {
        return map
            .First(y => y.Any(x => x.Type == 'S'))
            .First(x => x.Type == 'S').Position;
    }

    private Position FindEndPosition(Tile[][] map)
    {
        return map
            .First(y => y.Any(x => x.Type == 'E'))
            .First(x => x.Type == 'E').Position;
    }

    private Position GetNext(Tile[][] map, Position currentPos)
    {
        Position newPosition;
        foreach (var direction in DIRECTIONS)
        {
            newPosition = new Position(currentPos.X + direction.X, currentPos.Y + direction.Y);
            if (newPosition.X < 0 || newPosition.X >= map[0].Length || newPosition.Y < 0 || newPosition.Y >= map.Length)
            {
                continue;
            }
            if (map[newPosition.Y][newPosition.X].Visited || map[newPosition.Y][newPosition.X].Type == '#')
            {
                continue;
            }

            return newPosition;
        }
        return null;
    }

    private record Cheat(long Steps, Position Start, Position End);

    private record Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    private record Tile(char Type, Position Position)
    {
        public bool Visited { get; set; }
    }

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);
    private static readonly Position[] DIRECTIONS = [UP, DOWN, LEFT, RIGHT];

    private class CheatEqualityComparer : IEqualityComparer<Cheat>
    {
        public bool Equals(Cheat a, Cheat b)
        {
            return a.Start.X == b.Start.X && a.Start.Y == b.Start.Y && a.End.X == b.End.X && a.End.Y == b.End.Y;
        }
        public int GetHashCode([DisallowNull] Cheat obj)
        {
            return obj.Start.GetHashCode() ^ obj.End.GetHashCode();
        }
    }
}
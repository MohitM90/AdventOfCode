using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day06 : BaseDay
{
    public override async Task<long> Puzzle1Async()
    {
        int answer = 0;
        var map = Input.Split("\r\n").Select(s => s.ToCharArray());
        var traversedMap = MoveGuard(map.ToArray());

        foreach (var c in traversedMap)
        {
            if (c == 'X')
            {
                answer++;
            }
        }

        await Task.CompletedTask;
        return answer;
    }

    public override async Task<long> Puzzle2Async()
    {
        int answer = 0;
        var map = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();
        var traversedMap = MoveGuard(map);
        var startingPosos = FindStartPosition(map);

        var tasks = new List<Task<bool>>();

        Enumerable.Range(0, traversedMap.GetLength(0)).ToList().ForEach(i =>
        {
            Enumerable.Range(0, traversedMap.GetLength(1)).ToList().ForEach(j =>
            {
                if (traversedMap[i, j] == 'X')
                {
                    tasks.Add(Task.Run(() => BecomesLoopWithObstacle(map, startingPosos, new Position(i, j))));
                }
            });
        });

        var results = await Task.WhenAll(tasks);
        answer = results.Count(r => r);

        return answer;
    }

    private char[,] MoveGuard(char[][] map)
    {
        char[,] visited = new char[map.Length, map[0].Length];

        var pos = FindStartPosition(map);
        var directionChar = map[pos.Y][pos.X];
        var direction = GetMoveDirection(directionChar);

        var nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        while (nextPos.X >= 0 && nextPos.X < map[0].Length && nextPos.Y >= 0 && nextPos.Y < map.Length)
        {
            if (map[nextPos.Y][nextPos.X] == '#')
            {
                directionChar = TurnRight(directionChar);
                visited[pos.X, pos.Y] = directionChar;
                direction = GetMoveDirection(directionChar);
            }
            else
            {
                visited[nextPos.X, nextPos.Y] = directionChar;
                visited[pos.X, pos.Y] = 'X';
                pos = nextPos;
            }

            nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        }

        visited[pos.X, pos.Y] = 'X';
        return visited;
    }

    private bool BecomesLoopWithObstacle(char[][] map, Position startingPos, Position obstacle)
    {
        var pos = startingPos;
        if (obstacle == pos)
        {
            return false;
        }

        Direction[,] visited = new Direction[map.Length, map[0].Length];

        var directionChar = map[pos.Y][pos.X];
        var directionEnum = GetDirection(directionChar);
        var direction = GetMoveDirection(directionChar);

        var nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        while (nextPos.X >= 0 && nextPos.X < map[0].Length && nextPos.Y >= 0 && nextPos.Y < map.Length)
        {
            if (visited[nextPos.X, nextPos.Y].HasFlag(directionEnum))
            {
                return true;
            }

            if (map[nextPos.Y][nextPos.X] == '#' || nextPos == obstacle)
            {
                directionChar = TurnRight(directionChar);
                directionEnum = GetDirection(directionChar);
                visited[pos.X, pos.Y] = visited[pos.X, pos.Y] | directionEnum;
                direction = GetMoveDirection(directionChar);
            }
            else
            {
                visited[nextPos.X, nextPos.Y] = visited[nextPos.X, nextPos.Y] | directionEnum;
                pos = nextPos;
            }

            nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);

        }

        return false;
    }


    private Position FindStartPosition(char[][] map)
    {
        return map
            .Select((row, y) => (row, pos: new Position(Array.IndexOf(row, '^'), y)))
            .Where(t => t.pos.X >= 0)
            .Select(t => t.pos)
            .First();
    }



    private Position GetMoveDirection(char pos)
    {
        return pos switch
        {
            '^' => UP,
            'v' => DOWN,
            '<' => LEFT,
            '>' => RIGHT,
            _ => new(0, 0)
        };
    }

    private Direction GetDirection(char pos)
    {
        return pos switch
        {
            '^' => Direction.UP,
            'v' => Direction.DOWN,
            '<' => Direction.LEFT,
            '>' => Direction.RIGHT,
            _ => throw new Exception("Invalid direction")
        };
    }

    private char TurnRight(char pos)
    {
        return pos switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => pos
        };
    }

    private record Position(int X, int Y);

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);

    [Flags]
    private enum Direction
    {
        UP = 1,
        DOWN = 2,
        LEFT = 4,
        RIGHT = 8
    }
}

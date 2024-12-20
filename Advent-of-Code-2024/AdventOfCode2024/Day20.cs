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
        var map = Input.Split("\r\n").Select(s => s.ToCharArray());

        var steps = GetStepsMap(map.ToArray());
        var cheats = FindCheats(map.ToArray());
        foreach (var cheat in cheats)
        {
            var cheatPos = cheat.Position;
            var cheatSteps = cheat.Steps;
            var fieldSteps = steps[cheatPos.X, cheatPos.Y];
            var timeSaved = fieldSteps - cheatSteps;
            if (timeSaved >= 100)
            {
                answer++;
            }
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n");


        return answer;
    }

    private long[,] GetStepsMap(char[][] map)
    {
        long steps = 0;
        long[,] fieldSteps = new long[map.Length, map[0].Length];
        var pos = FindStartPosition(map);
        while (map[pos.Y][pos.X] != 'E')
        {
            var nextPos = GetNext(map, pos);
            if (nextPos == null)
            {
                break;
            }
            map[pos.Y][pos.X] = 'X';
            fieldSteps[pos.X, pos.Y] = steps;
            pos = nextPos;
            steps++;
        }
        return fieldSteps;
    }

    private List<Cheat> FindCheats(char[][] map)
    {
        long steps = 0;

        List<Cheat> cheats = [];
        var pos = FindStartPosition(map);

        while (map[pos.Y][pos.X] != 'E')
        {
            foreach (var direction in new[] { UP, DOWN, LEFT, RIGHT })
            {
                if (IsCheat(map, pos, direction, out var cheatPos))
                {
                    cheats.Add(new Cheat(steps + 2, cheatPos));
                }
            }
            var nextPos = GetNext(map, pos);
            if (nextPos == null)
            {
                break;
            }
            map[pos.Y][pos.X] = 'X';
            pos = nextPos;
            steps++;
        }
       
        return cheats;
    }

    private Position FindStartPosition(char[][] map)
    {
        return map
            .Select((row, y) => (row, pos: new Position(Array.IndexOf(row, 'S'), y)))
            .Where(t => t.pos.X >= 0)
            .Select(t => t.pos)
            .First();
    }

    private Position GetNext(char[][] map, Position currentPos)
    {
        Position newPosition;
        foreach (var direction in new[] { UP, DOWN, LEFT, RIGHT })
        {
            newPosition = new Position(currentPos.X + direction.X, currentPos.Y + direction.Y);
            if (newPosition.X < 0 || newPosition.X >= map[0].Length || newPosition.Y < 0 || newPosition.Y >= map.Length)
            {
                continue;
            }
            if (map[newPosition.Y][newPosition.X] != '.' && map[newPosition.Y][newPosition.X] != 'E')
            {
                continue;
            }

            return newPosition;
        }
        return null;
    }

    private bool IsCheat(char[][] map, Position pos, Position direction, out Position cheatPos)
    {
        var nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        cheatPos = new Position(pos.X + direction.X * 2, pos.Y + direction.Y * 2);
        if (nextPos.X < 0 || cheatPos.X < 0 || nextPos.X >= map[0].Length || cheatPos.X >= map[0].Length || 
            nextPos.Y < 0 || cheatPos.Y < 0 || nextPos.Y >= map.Length || cheatPos.Y >= map.Length)
        {
            cheatPos = null;
            return false;
        }
        if (map[nextPos.Y][nextPos.X] == '#' && (map[cheatPos.Y][cheatPos.X] == '.' || map[cheatPos.Y][cheatPos.X] == 'E'))
        {
            return true;
        }
        cheatPos = null;
        return false;
    }

    private record Cheat(long Steps, Position Position);

    private record Position(int X, int Y);

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);

}
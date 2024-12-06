using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day06 : BaseDay
{
    public override int Puzzle1()
    {
        int answer = 0;
        var map = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();
        var traversedMap = MoveGuard(map);

        answer = traversedMap.Sum(row => row.Count(c => c == 'X'));

        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;
        



        return answer;
    }

    private char[][] MoveGuard(char[][] map)
    {
        var pos = FindStartPosition(map);
        var directionChar = map[pos.Y][pos.X];
        var direction = GetMoveDirection(directionChar);

        var nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        while (nextPos.X >= 0 && nextPos.X < map[0].Length && nextPos.Y >= 0 && nextPos.Y < map.Length)
        {
            if (map[nextPos.Y][nextPos.X] == '#')
            {
                directionChar = TurnRight(directionChar);
                map[pos.Y][pos.X] = directionChar;
                direction = GetMoveDirection(directionChar);
            } 
            else
            {
                map[nextPos.Y][nextPos.X] = directionChar;
                map[pos.Y][pos.X] = 'X';
                pos = nextPos;
            }
            
            nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
        }

        map[pos.Y][pos.X] = 'X';
        return map;
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

}

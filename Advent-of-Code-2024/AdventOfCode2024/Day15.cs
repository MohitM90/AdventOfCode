using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day15 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n\r\n");
        var map = inputs[0].Split("\r\n").Select(s => s.ToCharArray());
        var moves = inputs[1].Replace("\r\n", "");

        var newMap = MoveGuard(map.ToArray(), moves);

        answer = newMap.SelectMany((row, y) => row.Select((c, x) => (c, position: new Position(x, y)))
            .Where(t => t.c == 'O'))
            .Sum(t => t.position.X + 100 * t.position.Y);

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n\r\n");
        var map = inputs[0].Split("\r\n")
            .Select(s => s
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.")
                .ToCharArray());

        var moves = inputs[1].Replace("\r\n", "");

        var newMap = MoveGuard(map.ToArray(), moves, false);

        answer = newMap.SelectMany((row, y) => row.Select((c, x) => (c, position: new Position(x, y)))
            .Where(t => t.c == '['))
            .Sum(t => t.position.X + 100 * t.position.Y);

        return answer;
    }

    private char[][] MoveGuard(char[][] map, string moves, bool printMap = false)
    {
        var pos = FindStartPosition(map);

        for (int i = 0; i < moves.Length; i++)
        {
            var directionChar = moves[i];
            var direction = GetMoveDirection(directionChar);
            var nextPos = new Position(pos.X + direction.X, pos.Y + direction.Y);
            if (map[nextPos.Y][nextPos.X] == 'O')
            {
                MoveBox(map, nextPos, direction);
            }
            if (map[nextPos.Y][nextPos.X] == '[' || map[nextPos.Y][nextPos.X] == ']')
            {
                MoveLargeBox(map, nextPos, direction);
            }
            if (map[nextPos.Y][nextPos.X] == '.')
            {
                map[pos.Y][pos.X] = '.';
                map[nextPos.Y][nextPos.X] = '@';
                pos = nextPos;
            }
            if (printMap)
            {
                PrinMap(map, 500);
            }
        }

        return map;
    }

    private void MoveBox(char[][] map, Position boxPosition, Position moveDirection)
    {
        var nextPos = new Position(boxPosition.X + moveDirection.X, boxPosition.Y + moveDirection.Y);
        if (map[nextPos.Y][nextPos.X] == '#')
        {
            return;
        }
        if (map[nextPos.Y][nextPos.X] == 'O')
        {
            MoveBox(map, nextPos, moveDirection);
        }
        if (map[nextPos.Y][nextPos.X] == '.')
        {
            map[boxPosition.Y][boxPosition.X] = '.';
            map[nextPos.Y][nextPos.X] = 'O';
        }
    }

    private bool MoveLargeBox(char[][] map, Position boxPosition, Position moveDirection)
    {
        var box = boxPosition;
        bool moved = false;
        if (moveDirection == UP || moveDirection == DOWN)
        {
            if (CanMoveLargeBox(map, box, moveDirection))
            {
                if (map[box.Y][box.X] == ']')
                {
                    box = new Position(box.X - 1, box.Y);
                }
                var nextPos = new Position(box.X + moveDirection.X, box.Y + moveDirection.Y);
                if (map[nextPos.Y][nextPos.X] == '[' || map[nextPos.Y][nextPos.X] == ']')
                {
                    MoveLargeBox(map, nextPos, moveDirection);
                }
                if (map[nextPos.Y][nextPos.X + 1] == '[')
                {
                    nextPos = new Position(box.X + moveDirection.X + 1, box.Y + moveDirection.Y);
                    MoveLargeBox(map, nextPos, moveDirection);
                    nextPos = new Position(box.X + moveDirection.X, box.Y + moveDirection.Y);
                }
                map[box.Y][box.X] = '.';
                map[box.Y][box.X + 1] = '.';
                map[nextPos.Y][nextPos.X] = '[';
                map[nextPos.Y][nextPos.X + 1] = ']';
                moved = true;
            }
        }
        if (moveDirection == LEFT || moveDirection == RIGHT)
        {
            if (CanMoveLargeBox(map, box, moveDirection))
            {
                var nextPos = new Position(box.X + 2 * moveDirection.X, box.Y + moveDirection.Y);
                if (map[nextPos.Y][nextPos.X] == '[' || map[nextPos.Y][nextPos.X] == ']')
                {
                    MoveLargeBox(map, nextPos, moveDirection);
                }
                map[nextPos.Y][nextPos.X] = map[nextPos.Y][nextPos.X - moveDirection.X];
                map[nextPos.Y][nextPos.X - moveDirection.X] = map[box.Y][box.X];
                map[box.Y][box.X] = '.';
                moved = true;
            }
        }
        return moved;
    }

    private bool CanMoveLargeBox(char[][] map, Position boxPosition, Position moveDirection)
    {
        var box = boxPosition;
        bool canMove = true;
        if (moveDirection == UP || moveDirection == DOWN)
        {
            if (map[box.Y][box.X] == ']')
            {
                box = new Position(box.X - 1, box.Y);
            }
            var nextPos = new Position(box.X + moveDirection.X, box.Y + moveDirection.Y);
            if (map[nextPos.Y][nextPos.X] == '#' || map[nextPos.Y][nextPos.X + 1] == '#')
            {
                return false;
            }
            if (map[nextPos.Y][nextPos.X] == '[' || map[nextPos.Y][nextPos.X] == ']')
            {
                canMove &= CanMoveLargeBox(map, nextPos, moveDirection);
            }
            if (map[nextPos.Y][nextPos.X + 1] == '[')
            {
                nextPos = new Position(box.X + moveDirection.X + 1, box.Y + moveDirection.Y);
                canMove &= CanMoveLargeBox(map, nextPos, moveDirection);
            }
        }
        if (moveDirection == LEFT || moveDirection == RIGHT)
        {
            var nextPos = new Position(box.X + 2 * moveDirection.X, box.Y + moveDirection.Y);
            if (map[nextPos.Y][nextPos.X] == '#')
            {
                return false;
            }
            if (map[nextPos.Y][nextPos.X] == '[' || map[nextPos.Y][nextPos.X] == ']')
            {
                canMove &= CanMoveLargeBox(map, nextPos, moveDirection);
            }
        }
        return canMove;
    }

    private void PrinMap(char[][] map, int delay)
    {
        Console.SetCursorPosition(0, 0);
        string output = "";
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                output += map[y][x];
            }
            output += "\n";
        }
        Console.WriteLine(output);
        Thread.Sleep(delay);
    }

    private Position FindStartPosition(char[][] map)
    {
        return map
            .Select((row, y) => (row, pos: new Position(Array.IndexOf(row, '@'), y)))
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

    private record Position(int X, int Y);

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);
}
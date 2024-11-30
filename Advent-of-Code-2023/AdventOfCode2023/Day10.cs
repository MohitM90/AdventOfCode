using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day10
{
    internal static int PuzzleA(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n");

        var start = FindStartPosition(inputs);
        var nextPos = GetNextPositions(inputs, start);

        var lastPos1 = start;
        var lastPos2 = start;
        var pos1 = nextPos[0];
        var pos2 = nextPos[1];

        while (pos1 != pos2)
        {
            var nextPos1 = GetNextPosition(lastPos1, pos1, inputs[pos1.y][pos1.x]);
            var nextPos2 = GetNextPosition(lastPos2, pos2, inputs[pos2.y][pos2.x]);
            sum++;
            lastPos1 = pos1;
            lastPos2 = pos2;
            pos1 = nextPos1;
            pos2 = nextPos2;
        }

        return sum + 1;
    }

    internal static int PuzzleB(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n");
        var copy = inputs.Select(x => new string('.', x.Length)).ToArray();
        var start = FindStartPosition(inputs);
        var nextPos = GetNextPositions(inputs, start);

        var lastPos1 = start;
        var lastPos2 = start;
        var pos1 = nextPos[0];
        var pos2 = nextPos[1];

        var strBuilder = new StringBuilder(copy[start.y]);
        strBuilder[start.x] = ReplaceS(start, pos1, pos2);
        copy[start.y] = strBuilder.ToString();

        while (pos1 != pos2)
        {
            strBuilder = new StringBuilder(copy[pos1.y]);
            strBuilder[pos1.x] = inputs[pos1.y][pos1.x];
            copy[pos1.y] = strBuilder.ToString();

            strBuilder = new StringBuilder(copy[pos2.y]);
            strBuilder[pos2.x] = inputs[pos2.y][pos2.x];
            copy[pos2.y] = strBuilder.ToString();

            var nextPos1 = GetNextPosition(lastPos1, pos1, inputs[pos1.y][pos1.x]);
            var nextPos2 = GetNextPosition(lastPos2, pos2, inputs[pos2.y][pos2.x]);

            lastPos1 = pos1;
            lastPos2 = pos2;
            pos1 = nextPos1;
            pos2 = nextPos2;
        }

        strBuilder = new StringBuilder(copy[pos1.y]);
        strBuilder[pos1.x] = inputs[pos1.y][pos1.x];
        copy[pos1.y] = strBuilder.ToString();

        for (int y = 1; y < copy.Length; y++)
        {
            if (copy[y].All(c => c == '.'))
            {
                continue;
            }
            var odd = false;
            for (int x = 0; x < copy[y].Length; x++)
            {
                char c = copy[y][x];
                if (c == '.')
                {
                    if (odd)
                    {
                        sum++;
                    }
                    continue;
                }
                if (AreConnecting(copy[y-1][x], copy[y][x]))
                {
                    odd = !odd;
                }

            }


            //var odd = false;
            //foreach(var c in s)
            //{
            //    //if (c )
            //}
            Console.WriteLine(copy[y]);
        }

        return sum;
    }

    private static (int x, int y) GetNextPosition((int x, int y) lastPosition, (int x, int y) currentPosition, char pipe)
    {
        (int x, int y) pos1 = currentPosition;
        (int x, int y) pos2 = currentPosition;
        switch (pipe)
        {
            case '|':
                pos1.y--; pos2.y++;
                break;
            case '-':
                pos1.x--; pos2.x++;
                break;
            case 'L':
                pos1.y--; pos2.x++;
                break;
            case 'J':
                pos1.y--; pos2.x--;
                break;
            case '7':
                pos1.y++; pos2.x--;
                break;
            case 'F':
                pos1.y++; pos2.x++;
                break;
        }
        return pos1 == lastPosition ? pos2 : pos1;
    }

    private static (int x, int y)[] GetNextPositions(string[] map, (int x, int y) pos)
    {
        char pipe = map[pos.y][pos.x];


        if (pipe == 'S')
        {
            List<(int x, int y)> positions = [];

            if (pos.y > 0 && map[pos.y - 1][pos.x] is '|' or '7' or 'F')
            {
                positions.Add((pos.x, pos.y - 1));
            }
            if (pos.y < map.Length -1 && map[pos.y + 1][pos.x] is '|' or 'L' or 'J')
            {
                positions.Add((pos.x, pos.y + 1));
            }
            if (pos.x > 0 && map[pos.y][pos.x - 1] is '-' or 'L' or 'F')
            {
                positions.Add((pos.x - 1, pos.y));
            }
            if (pos.x < map[pos.y].Length - 1 && map[pos.y][pos.x + 1] is '-' or '7' or 'J')
            {
                positions.Add((pos.x + 1, pos.y));
            }
            return positions.ToArray();
        }

        (int x, int y) pos1 = pos;
        (int x, int y) pos2 = pos;
        switch (pipe)
        {
            case '|':
                pos1.y--; pos2.y++;
                break;
            case '-':
                pos1.x--; pos2.x++;
                break;
            case 'L':
                pos1.y--; pos2.x++;
                break;
            case 'J':
                pos1.y--; pos2.x--;
                break;
            case '7':
                pos1.y++; pos2.x--;
                break;
            case 'F':
                pos1.y++; pos2.x++;
                break;
        }
        return [pos1, pos2];
    }


    private static (int x, int y) FindStartPosition(string[] input)
    {
        for (int y = 0; y < input.Length; y++)
        {
            int x = input[y].IndexOf('S');
            if (x > -1)
            {
                return (x, y);
            }
        }
        return (-1, -1);
    }

    private static char ReplaceS((int x, int y) posS, (int x, int y) pos1, (int x, int y) pos2)
    {
        char[] possible = ['|', '-', 'L', 'J', '7', 'F'];
        var positions = (List<(int x, int y)>)[pos1, pos2];
        foreach (var pos in positions)
        {
            if (pos.x < posS.x)
            {
                possible = possible.Where(c => c is '-' or 'J' or '7').ToArray();
            }
            if (pos.x > posS.x)
            {
                possible = possible.Where(c => c is '-' or 'L' or 'F').ToArray();
            }
            if (pos.y > posS.y)
            {
                possible = possible.Where(c => c is '|' or '7' or 'F').ToArray();
            }
            if (pos.y < posS.y)
            {
                possible = possible.Where(c => c is '|' or 'L' or 'J').ToArray();
            }
        }
        

        return possible.First();
    }

    private static bool AreConnecting(char top, char bottom)
    {
        return (top is '|' or '7' or 'F') && (bottom is '|' or 'L' or 'J');
    }
}

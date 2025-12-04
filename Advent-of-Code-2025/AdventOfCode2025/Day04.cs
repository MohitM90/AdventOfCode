using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day04 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string input)
    {
        long answer = 0;
        var map = input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();
        answer = RemovePaperRolls(map, false);


        return answer;
    }

    public override async Task<long> Puzzle2(string input)
    {
        long answer = 0;
        var map = input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();
        var count = 0;
        do
        {
            count = RemovePaperRolls(map, true);
            answer += count;
        } while (count > 0);


        return answer;
    }

    private int RemovePaperRolls(char[][] map, bool replaceChar)
    {
        int removedCount = 0;
        for (int x = 0; x < map.Length; x++)
        {
            for (int y = 0; y < map[x].Length; y++)
            {
                if (map[x][y] == '@')
                {
                    var count = GetAdjacentCount(map, x, y);
                    if (count < 4)
                    {
                        if (replaceChar)
                        {
                            map[x][y] = 'x';
                        }
                        
                        removedCount++;
                    }
                }
            }
        }
        return removedCount;
    }

    private int GetAdjacentCount(char[][] map, int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (x - dx >= 0 && x - dx < map.Length &&
                    y - dy >= 0 && y - dy < map[0].Length &&
                    !(dx == 0 && dy == 0))
                {
                    if (map[x - dx][y - dy] == '@')
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
}

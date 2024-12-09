using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day09 : BaseDay
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Chunk(2).ToArray();

        answer = CalculateChecksum(inputs);

        return answer;
    }

    public override long Puzzle2()
    {
        int answer = 0;
        var inputs = Input.Split("\r\n");



        return answer;
    }

    private long CalculateChecksum(char[][] diskMap)
    {
        List<char[]> map = diskMap.ToList();
        long checksum = 0;
        long pos = 0;

        for (int i = 0; i < map.Count; i++)
        {
            // Left part
            long n = map[i][0].ToInt();
            long id = i;
            checksum += id * ((pos + n) * (pos + n - 1)/2 - pos * (pos - 1) / 2);

            pos += n;

            if (i == map.Count - 1)
            {
                break;
            }

            // Right part
            n = map[i][1].ToInt();
            while (n > 0)
            {
                long m = map.Last()[0].ToInt();
                long movedBlocks = Math.Min(m, n);
                long lastId = map.Count - 1;
                checksum += lastId * ((pos + movedBlocks) * (pos + movedBlocks - 1) / 2 - pos * (pos - 1) / 2);

                pos += movedBlocks;

                n -= movedBlocks;
                m -= movedBlocks;

                if (m == 0)
                {
                    map.RemoveAt(map.Count - 1);
                }
                else
                {
                    map.Last()[0] = m.ToChar();
                }
            }
        }
        return checksum;
    }
    
}

public static class CharExtensions
{
    public static int ToInt(this char c)
    {
        return c - '0';
    }
}

public static class LongExtensions
{
    public static char ToChar(this long c)
    {
        return (char)('0' + c);
    }
}
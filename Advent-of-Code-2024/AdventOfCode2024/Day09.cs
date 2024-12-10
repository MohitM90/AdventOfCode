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
        long answer = 0;
        if (Input.Length % 2 != 0)
        {
            Input += "0";
        }
        var inputs = Input.Chunk(2).ToArray();

        answer = CalculateChecksumWithoutDefrag(inputs);

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
            long n = map[i][0].ToLong();
            long id = i;
            checksum += id * ((pos + n) * (pos + n - 1)/2 - pos * (pos - 1) / 2);

            pos += n;

            if (i == map.Count - 1)
            {
                break;
            }

            // Right part
            n = map[i][1].ToLong();
            while (n > 0)
            {
                long m = map.Last()[0].ToLong();
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

    private long CalculateChecksumWithoutDefrag(char[][] diskMap)
    {
        List<(int Index, char[] Item, long Filled)> map = diskMap
            .Index()
            .Select(c => (c.Index, c.Item, 0L))
            .ToList();

        long checksum = 0;

        long pos = 0;
        for (int i = map.Count - 1; i >= 0; i--)
        {
            long n = map[i].Item[0].ToLong();
            long id = i;

            var x = map.FirstOrDefault(c => c.Item[1].ToLong() >= n && c.Index < map[i].Index);
            if (x == default)
            {
                pos = map.Take(map[i].Index)
                    .Sum(c => c.Item[0].ToLong() + c.Item[1].ToLong() + c.Filled);

                checksum += id * ((pos + n) * (pos + n - 1) / 2 - pos * (pos - 1) / 2);

                continue;
            }

            pos = x.Item[0].ToLong() + x.Filled +
                map.Take(x.Index).Sum(c => c.Item[0].ToLong() + c.Item[1].ToLong() + c.Filled);


            checksum += id * ((pos + n) * (pos + n - 1) / 2 - pos * (pos - 1) / 2);

            x.Item[1] -= (char)n;
            x.Filled += n;
            map[x.Index] = (x.Index, x.Item, x.Filled);
        }

        return checksum;
    }

}

public static class CharExtensions
{
    public static long ToLong(this char c)
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
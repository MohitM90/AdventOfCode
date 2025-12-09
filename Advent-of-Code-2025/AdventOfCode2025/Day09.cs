using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day09 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        var tiles = inputs.Select(x => x.Split(',')).Select(x => new Tile(int.Parse(x[0]), int.Parse(x[1]))).ToList();
        long max = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                var tile1 = tiles[i];
                var tile2 = tiles[j];
                var area = (Math.Abs(tile1.X - tile2.X) + 1) * (Math.Abs(tile1.Y - tile2.Y) + 1);
                if (area >= max)
                {
                    max = area;
                }
            }
        }
        answer = max;

        return answer;
    }

    public override async Task<long> Puzzle2(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");



        return answer;
    }

    public record Tile(long X, long Y);
}

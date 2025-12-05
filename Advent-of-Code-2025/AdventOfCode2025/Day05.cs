using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace AdventOfCode2025;

internal class Day05 : BaseDay<ulong>
{
    public override async Task<ulong> Puzzle1(string input)
    {
        ulong answer = 0;
        var inputs = input.Split("\r\n\r\n");
        var ranges = inputs[0].Split("\r\n").Select(x => 
        {
            var minMax = x.Split("-");
            var min = ulong.Parse(minMax[0]);
            var max = ulong.Parse(minMax[1]);
            return new ulong[] { min, max };
        });
        var ids = inputs[1].Split("\r\n").Select(x => ulong.Parse(x));
        
        foreach ( var id in ids )
        {
            foreach (var range in ranges)
            {
                var min = range[0];
                var max = range[1];
                
                if (id >= min && id <= max)
                {
                    answer++; break;
                }
            }
        }


        return answer;
    }

    public override async Task<ulong> Puzzle2(string input)
    {
        ulong answer = 0;
        var inputs = input.Split("\r\n\r\n");



        return answer;
    }
}

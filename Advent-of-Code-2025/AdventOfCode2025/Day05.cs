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
        var ranges = inputs[0].Split("\r\n").Select(x =>
        {
            var minMax = x.Split("-");
            var min = ulong.Parse(minMax[0]);
            var max = ulong.Parse(minMax[1]);
            return (min, max);
        });
        var ids = inputs[1].Split("\r\n").Select(x => ulong.Parse(x));

        var mergedRanges = new List<(ulong min, ulong max)>();
        foreach (var range in ranges.OrderBy(x => x.min))
        {
            if (mergedRanges.Count == 0)
            {
                mergedRanges.Add(range);
                continue;
            }

            bool isMerged = false;
            for (int i = 0; i < mergedRanges.Count; i++)
            {
                var mergedRange = mergedRanges[i];
                if (range.min < mergedRange.min && range.max < mergedRange.min)
                {
                    mergedRanges.Insert(i, range);
                    isMerged = true;
                    break;
                }
                if (range.min < mergedRange.min && range.max >= mergedRange.min)
                {
                    mergedRanges[i] = (range.min, Math.Max(range.max, mergedRange.max));
                    isMerged = true;
                    break;
                }
                if (range.min >= mergedRange.min && range.max <= mergedRange.max)
                {
                    isMerged = true;
                    break;
                }
                if (range.min >= mergedRange.min && range.min <= mergedRange.max && range.max >= mergedRange.max)
                {
                    mergedRanges[i] = (mergedRange.min, range.max);
                    isMerged = true;
                    break;
                }
                if (range.min > mergedRange.max)
                {
                    continue;
                }
            }
            if (!isMerged)
            {
                mergedRanges.Add(range);
            }
        }

        foreach (var mergedRange in mergedRanges)
        {
            var count = mergedRange.max - mergedRange.min + 1;
            answer += count;
        }

        return answer;
    }
}

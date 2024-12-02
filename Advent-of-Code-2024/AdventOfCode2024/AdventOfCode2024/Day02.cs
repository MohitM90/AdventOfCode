using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day02 : BaseDay
{
    public override int Puzzle1()
    {
        int answer = 0;
        var inputs = Input
            .Split("\r\n")
            .Select(x => x.Split(" ")
                .Select(s => int.Parse(s))
                .ToArray());

        foreach (var report in inputs)
        {
            if (IsSafe(report))
            {
                answer++;
            }
        }

        return answer;
    }

    public override int Puzzle2()
    {
        return 0;
    }

    private bool IsSafe(int[] report) {
        bool? isDecreasing = null;
        int start = report[0];
        for (int i = 1; i < report.Length; i++)
        {
            if (i == 1)
            {
                if (report[i] < start)
                {
                    isDecreasing = true;
                }
                else if (report[i] > start)
                {
                    isDecreasing = false;
                }
                else
                {
                    return false;
                }
            }

            var prev = report[i - 1];
            var current = report[i];
            if (isDecreasing == true && current >= prev)
            {
                return false;
            }
            if (isDecreasing == false && current <= prev)
            {
                return false;
            }

            var diff = Math.Abs(prev - current);
            if (diff < 1 || diff > 3)
            {
                return false;
            }
        }

        return true;
    }
}

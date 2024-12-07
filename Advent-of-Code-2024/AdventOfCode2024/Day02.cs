using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day02 : BaseDay
{
    public override long Puzzle1()
    {
        int answer = 0;
        var inputs = Input
            .Split("\r\n")
            .Select(x => x.Split(" ")
                .Select(s => int.Parse(s))
                .ToList());

        foreach (var report in inputs)
        {
            if (IsSafe(report))
            {
                answer++;
            }
        }

        return answer;
    }

    public override long Puzzle2()
    {
        int answer = 0;
        var inputs = Input
            .Split("\r\n")
            .Select(x => x.Split(" ")
                .Select(s => int.Parse(s))
                .ToList());

        foreach (var report in inputs)
        {
            if (IsSafe(report, 1))
            {
                answer++;
            }
        }

        return answer;
    }

    private bool IsSafe(List<int> report, int tolerance = 0, int removeIndex = -1) {
        if (tolerance < 0)
        {
            return false;
        }
        if (removeIndex >= 0)
        {
            report.RemoveAt(removeIndex);
        }

        bool? isDecreasing = null;

        for (int i = 1; i < report.Count; i++)
        {
            if (isDecreasing == null)
            {
                if (report[i] < report[i - 1])
                {
                    isDecreasing = true;
                }
                else if (report[i] > report[i - 1])
                {
                    isDecreasing = false;
                }
                else
                {
                    if (Enumerable.Range(0, report.Count).Any(n => IsSafe([.. report], tolerance - 1, n)))
                    {
                        return true;
                    }
                    return false;
                }
            }

            var prev = report[i - 1];
            var current = report[i];
            if (isDecreasing == true && current >= prev)
            {
                if (Enumerable.Range(0, report.Count).Any(n => IsSafe([.. report], tolerance - 1, n)))
                {
                    return true;
                }
                return false;
            }
            if (isDecreasing == false && current <= prev)
            {
                if (Enumerable.Range(0, report.Count).Any(n => IsSafe([.. report], tolerance - 1, n)))
                {
                    return true;
                }
                return false;
            }

            var diff = Math.Abs(prev - current);
            if (diff < 1 || diff > 3)
            {
                if (Enumerable.Range(0, report.Count).Any(n => IsSafe([.. report], tolerance - 1, n)))
                {
                    return true;
                }
                return false;
            }
        }

        return true;
    }
}

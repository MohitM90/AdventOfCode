using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day05 : BaseDay
{
    public override int Puzzle1()
    {
        int answer = 0;
        var inputs = Input.Split("\r\n\r\n");

        var rules = inputs[0].Split("\r\n")
            .Select(r => r.Split('|'));
        var rulesAfter = rules
            .ToLookup(r => int.Parse(r[0]), r => int.Parse(r[1]));
        var rulesBefore = rules
            .ToLookup(r => int.Parse(r[1]), r => int.Parse(r[0]));

        var pages = inputs[1].Split("\r\n")
            .Select(p => p.Split(',')
                .Select(i => int.Parse(i))
                .ToList());

        answer = pages.Where(p => IsValid(p, rulesAfter, rulesBefore))
            .Sum(p => p[p.Count/2]);
        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;

        

        return answer;
    }

    private bool IsValid(List<int> page, ILookup<int, int> rulesAfter, ILookup<int, int> rulesBefore)
    {
        foreach (var p in page)
        {
            var r = rulesAfter[p];
            var pIndex = page.IndexOf(p);
            if (r.Any(i => page.Contains(i) && page.IndexOf(i) < pIndex))
            {
                return false;
            }

            r = rulesBefore[p];
            if (r.Any(i => page.Contains(i) && page.IndexOf(i) > pIndex))
            {
                return false;
            }
        }
        return true;
    }
}

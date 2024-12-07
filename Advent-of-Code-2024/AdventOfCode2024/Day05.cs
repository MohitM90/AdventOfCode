using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day05 : BaseDay
{
    public override long Puzzle1()
    {
        int answer = 0;
        var inputs = Input.Split("\r\n\r\n");

        var rules = inputs[0].Split("\r\n")
            .Select(r => r.Split('|'));
        var rulesAfter = rules
            .OrderBy(r => int.Parse(r[1]))
            .ToLookup(r => int.Parse(r[0]), r => int.Parse(r[1]));
        var rulesBefore = rules
            .OrderBy(r => int.Parse(r[0]))
            .ToLookup(r => int.Parse(r[1]), r => int.Parse(r[0]));

        var pages = inputs[1].Split("\r\n")
            .Select(p => p.Split(',')
                .Select(i => int.Parse(i))
                .ToList());

        answer = pages.Where(p => IsValid(p, rulesAfter, rulesBefore))
            .Sum(p => p[p.Count/2]);
        return answer;
    }

    public override long Puzzle2()
    {
        int answer = 0;
        var inputs = Input.Split("\r\n\r\n");

        var rules = inputs[0].Split("\r\n")
            .Select(r => r.Split('|'));
        var rulesAfter = rules
            .OrderBy(r => int.Parse(r[1]))
            .ToLookup(r => int.Parse(r[0]), r => int.Parse(r[1]));
        var rulesBefore = rules
            .OrderBy(r => int.Parse(r[0]))
            .ToLookup(r => int.Parse(r[1]), r => int.Parse(r[0]));

        var pages = inputs[1].Split("\r\n")
            .Select(p => p.Split(',')
                .Select(i => int.Parse(i))
                .ToList());

        answer = pages.Where(p => !IsValid(p, rulesAfter, rulesBefore))
            .Select(p => Correct(p, rulesAfter, rulesBefore))
            .Sum(p => p[p.Count / 2]);


        return answer;
    }

    private bool IsValid(List<int> page, ILookup<int, int> rulesAfter, ILookup<int, int> rulesBefore)
    {
        foreach (var p in page)
        {
            var r = rulesAfter[p].Order();
            var pIndex = page.IndexOf(p);
            if (r.Any(i => page.Contains(i) && page.IndexOf(i) < pIndex))
            {
                return false;
            }

            r = rulesBefore[p].Order();
            if (r.Any(i => page.Contains(i) && page.IndexOf(i) > pIndex))
            {
                return false;
            }
        }
        return true;
    }

    private List<int> Correct(List<int> page, ILookup<int, int> rulesAfter, ILookup<int, int> rulesBefore)
    {
        var correctedPage = new List<int>(page);
        for (int i = 0; i < page.Count; i++)
        {
            var p = correctedPage[i];
            for (int j = i + 1; j < correctedPage.Count; j++)
            {
                var value = correctedPage[j];
                if (rulesBefore[p].Contains(value))
                {
                    correctedPage.RemoveAt(j);
                    correctedPage.Insert(i, value);
                    i--;
                    break;
                }
            }
        }
        for (int i = correctedPage.Count - 1; i >= 0; i--)
        {
            var p = correctedPage[i];
            for (int j = i - 1; j >= 0; j--)
            {
                var value = correctedPage[j];
                if (rulesAfter[p].Contains(value))
                {
                    correctedPage.RemoveAt(j);
                    correctedPage.Insert(i, value);
                    i++;
                    break;
                }
            }
        }
        return correctedPage;
    }
}

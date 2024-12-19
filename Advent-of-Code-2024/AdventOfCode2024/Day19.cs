using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day19 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n\r\n");

        var rules = input[0].Split(", ").ToList();

        List<string> rulesFiltered = new();
        foreach (var rule in rules)
        {
            var p = "^(?:" + string.Join("|", rules.Except([rule])) + ")+$";
            if (!Regex.IsMatch(rule, p))
            {
                rulesFiltered.Add(rule);
            }
        }

        var pattern = "^(?:" + string.Join("|", rulesFiltered) + ")+$";
        answer = input[1].Split("\r\n").Count(x => Regex.IsMatch(x, pattern));

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n\r\n");


        return answer;
    }

}
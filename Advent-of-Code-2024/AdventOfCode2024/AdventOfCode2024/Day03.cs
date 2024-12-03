﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day03 : BaseDay
{
    public override int Puzzle1()
    {
        int answer = 0;

        var inputs = Regex.Matches(Input, "mul\\((\\d{1,3}),(\\d{1,3})\\)");

        answer = inputs.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum();

        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;
        var inputs = Input.Split("\r\n");



        return answer;
    }
}

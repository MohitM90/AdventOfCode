using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2025;

internal class Day06 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");

        var operators = inputs.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var nums = inputs[0..(inputs.Length-1)]
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(y => long.Parse(y)).ToArray())
            .ToArray();
        for (int i = 0; i < operators.Length; i++)
        {
            long result = 0;
            if (operators[i] == "+")
            {
                result = nums.Sum(x => x[i]);
            }
            else if (operators[i] == "*")
            {
                result = nums.Aggregate(1L, (acc, x) => acc * x[i]);
            }
            answer += result;

        }
        return answer;
    }

    public override async Task<long> Puzzle2(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");



        return answer;
    }
}
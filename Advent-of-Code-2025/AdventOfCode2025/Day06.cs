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

        var operators = inputs.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
        var nums = inputs[0..(inputs.Length - 1)]
            .Select(x => x.ToCharArray().Reverse().ToArray())
            .ToArray()
            .Transpose();
        int n = 0;
        List<long> results = [];
        for (int x = 0; x < nums.Length; x++)
        {
            if (nums[x].All(y => y == ' '))
            {
                answer += Calc(operators[n], results);
                n++;
                results.Clear();
                continue;
            }
            var num = long.Parse(new string(nums[x]).Trim());
            results.Add(num);
        }
        answer += Calc(operators[n], results);


        return answer;
    }

    private static long Calc(string op, List<long> nums)
    {
        if (op == "+")
        {
            return nums.Sum();
        }
        else if (op == "*")
        {
            return nums.Aggregate(1L, (acc, x) => acc * x);
        }
        return 0;
    }
}
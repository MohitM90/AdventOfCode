using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day07 : BaseDay
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");
        var calibration = inputs
            .Select(x => x.Split([": ", " "], StringSplitOptions.None))
            .Select(s => new Calibration(
                long.Parse(s[0]),
                s[1..].Select(l => long.Parse(l)).ToArray()));
        
        answer = calibration.Sum(c => Solve(c.Sum, c.Inputs.First(), c.Inputs[1..]));

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");

        


        return answer;
    }

    private long Solve(long sum, long initial, long[] inputs)
    {
        if (Solve(sum, initial, inputs, Operator.Mul) == sum)
        {
            return sum;
        }
        return 0;
    }

    private long Solve(long sum, long initial, long[] inputs, Operator op)
    {
        if (inputs.Length == 0)
        {
            return initial;
        }

        var temp = op switch
        {
            Operator.Mul => initial * inputs[0],
            Operator.Add => initial + inputs[0],
            _ => throw new NotImplementedException()
        };

        if (temp < sum)
        {
            temp = Solve(sum, temp, inputs[1..], Operator.Mul);
        }

        if (temp != sum && op == Operator.Mul)
        {
            temp = Solve(sum, initial, inputs, Operator.Add);
        }

        return temp;
    }

    private record Calibration(long Sum, long[] Inputs);

    private enum Operator
    {
        Mul,
        Add
    }
}

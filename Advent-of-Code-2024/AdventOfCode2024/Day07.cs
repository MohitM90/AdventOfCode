using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day07 : BaseDay<long>
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
        var calibration = inputs
    .Select(x => x.Split([": ", " "], StringSplitOptions.None))
    .Select(s => new Calibration(
        long.Parse(s[0]),
        s[1..].Select(l => long.Parse(l)).ToArray()));

        answer = calibration.Sum(c => Solve(c.Sum, c.Inputs.First(), c.Inputs[1..], true));

        return answer;
    }

    private long Solve(long sum, long initial, long[] inputs, bool concatEnabled = false)
    {
        if (Solve(sum, initial, inputs, Operator.Mul, concatEnabled) == (sum, true))
        {
            return sum;
        }
        return 0;
    }

    private (long, bool) Solve(long sum, long initial, long[] inputs, Operator op, bool concatEnabled = false)
    {
        if (inputs.Length == 0)
        {
            return (initial, true);
        }

        bool reachedEnd = false;

        var temp = op switch
        {
            Operator.Mul => initial * inputs[0],
            Operator.Add => initial + inputs[0],
            Operator.Concat => Concat(initial, inputs[0]),
            _ => throw new NotImplementedException()
        };


        if (temp < sum)
        {
            (temp, reachedEnd) = Solve(sum, temp, inputs[1..], Operator.Mul, concatEnabled);
        }

        if (temp != sum && op == Operator.Mul)
        {
            (temp, reachedEnd) = Solve(sum, initial, inputs, Operator.Add, concatEnabled);
        }

        if (concatEnabled && temp != sum && op == Operator.Add)
        {
            (temp, reachedEnd) = Solve(sum, initial, inputs, Operator.Concat, concatEnabled);
        }

        if (reachedEnd)
        {
            return (temp, true);
        }

        return Solve(sum, temp, inputs[1..], Operator.Mul, concatEnabled);
    }

    private long Concat(long p, long q)
    {
        return p * (long)Math.Pow(10, (int)Math.Log10(q) + 1) + q;
    }

    private record Calibration(long Sum, long[] Inputs);

    private enum Operator
    {
        Mul,
        Add,
        Concat
    }
}

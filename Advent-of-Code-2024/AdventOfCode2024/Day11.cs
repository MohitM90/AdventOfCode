using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day11 : BaseDay
{

    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split(" ").Select(x => (long.Parse(x), 1l));


        for (int i = 0; i < 25; i++)
        {
            inputs = inputs
                .GroupBy(x => x)
                .SelectMany(i => ApplyRules((i.Key.Item1, i.Sum(x => x.Item2))));

        }

        answer = inputs.Sum(x => x.Item2);

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split(" ").Select(x => (long.Parse(x), 1L));

        for (int i = 0; i < 75; i++)
        {
            inputs = inputs
                .GroupBy(x => x)
                .SelectMany(i => ApplyRules((i.Key.Item1, i.Sum(x => x.Item2))));
        }

        answer = inputs.Sum(x => x.Item2);

        return answer;
    }

    private (long value, long count)[] ApplyRules((long value, long count) input)
    {
        if (input.value == 0)
        {
            return [(1L, input.count)];
        }

        if (HasEvenDigits(input.value))
        {
            var value = SplitHalf(input.value);
            if (value.Item1 == value.Item2)
            {
                return [(value.Item1, 2 * input.count)];
            }
            return [(value.Item1, input.count), (value.Item2, input.count)];
        }

        return [(input.value * 2024, input.count)];
    }

    private bool HasEvenDigits(long input)
    {
        var digits = (int)(Math.Log10(input) + 1);
        return digits % 2 == 0;
    }

    private (long, long) SplitHalf(long input)
    {
        var digits = (int)(Math.Log10(input) + 1);

        long factor = (long)Math.Pow(10, digits / 2);
        var firstHalf = input / factor;
        var secondHalf = input - firstHalf * factor;

        return (firstHalf, secondHalf);
    }

}
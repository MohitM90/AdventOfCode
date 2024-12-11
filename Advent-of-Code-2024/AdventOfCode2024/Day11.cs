using System;
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
        var inputs = Input.Split(" ").Select(x => ulong.Parse(x));

        for (int i = 0; i < 25; i++)
        {
            inputs = inputs.SelectMany(i => ApplyRules(i));
            Console.WriteLine(string.Join(" ", inputs));
            Console.WriteLine();
        }
        return inputs.Count();
    }

    public override long Puzzle2()
    {
        return 0;
    }

    private ulong[] ApplyRules(ulong input)
    {
        if (input == 0)
        {
            return [1];
        }

        if (HasEvenDigits(input)) {
            return SplitHalf(input);
        }

        return [input * 2024];
    }

    private bool HasEvenDigits(ulong input)
    {
        var digits = (int)(Math.Log10(input) + 1);
        return digits % 2 == 0;
    }

    private ulong[] SplitHalf(ulong input)
    {
        var digits = (int)(Math.Log10(input) + 1);

        ulong factor = (ulong)Math.Pow(10, digits / 2);
        var firstHalf = input / factor;
        var secondHalf = input - firstHalf * factor;

        return [firstHalf, secondHalf];
    }

}
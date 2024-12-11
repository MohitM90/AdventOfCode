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
        var inputs = Input.Split(separator: " ");

        for (int i = 0; i < 25; i++)
        {
            inputs = inputs.SelectMany(i => ApplyRules(i)).ToArray();
            //Console.WriteLine(string.Join(" ", inputs));
            //Console.WriteLine();
        }
        return inputs.LongLength;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();

        

        return answer;
    }

    private string[] ApplyRules(string input)
    {
        if (input == "0")
        {
            return ["1"];
        }

        if (HasEvenDigits(input)) {
            return SplitHalf(input);
        }

        return MultiplyBy2024(input);
    }

    private bool HasEvenDigits(string input)
    {
        return input.Length %2 == 0;
    }

    private string[] SplitHalf(string input)
    {
        var firstHalf = ulong.Parse(input[0..(input.Length / 2)]).ToString();
        var secondHalf = ulong.Parse(input[(input.Length / 2)..]).ToString();
        return [firstHalf, secondHalf];
    }

    private string[] MultiplyBy2024(string input)
    {
        return [(ulong.Parse(input) * 2024).ToString()];
    }

    private record Point(int X, int Y);
}
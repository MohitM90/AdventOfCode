using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day01 : BaseDay<long>
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");

        var value = 50;
        foreach (var input in inputs)
        {
            int num = int.Parse(input[1..]);
            if (input[0] == 'L')
            {
                value = Modulo(value - num, 100);
            }
            else
            {
                value = Modulo(value + num, 100);
            }
            if (value == 0)
            {
                answer++;
            }
        }


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");



        return answer;
    }

    public override async Task<long> Puzzle1Async()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");
        return answer;
    }
    public override async Task<long> Puzzle2Async()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");
        return answer;
    }

    int Modulo(int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }
}

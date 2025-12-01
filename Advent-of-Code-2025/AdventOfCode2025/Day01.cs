using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        var value = 50;
        foreach (var input in inputs)
        {
            int num = int.Parse(input[1..]);
            var add = 0;
            if (num == 0)
            {
                continue;
            }
            if (input[0] == 'L')
            {
                
                if ((value - num) <= 0)
                {
                    add = (int)Math.Floor(Math.Abs(value - num) / 100.0) + 1;
                    if (value == 0)
                    {
                        add--;
                    }
                }
                
                value = Modulo(value - num, 100);
            }
            else
            {
                if ((value + num) >= 100)
                {
                    add = (int)Math.Floor(Math.Abs(value + num) / 100.0);
                }
                value = Modulo(value + num, 100);
            }
            answer += add;
            //if (value == 0)
            //{
            //    answer++;
            //}
        }

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

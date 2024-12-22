using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day22 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n").Select(x => long.Parse(x));

        foreach (var seed in input)
        {
            var secret = seed;
            for (int i = 0; i < 2000; i++)
            {
                secret = GetRandomNumber(secret);
            }
            answer += secret;
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n");

        return answer;
    }

    
    private long GetRandomNumber(long seed)
    {
        seed = ((seed * 64L) ^ seed) % 16777216;
        seed = ((seed / 32L) ^ seed) % 16777216;
        seed = ((seed * 2048L) ^ seed) % 16777216;
        return seed;
    }
}
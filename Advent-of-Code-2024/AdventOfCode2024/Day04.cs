using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day04 : BaseDay
{
    public override int Puzzle1()
    {
        int answer = 0;
        var input = Input.Split("\r\n");

        answer += CountStringHorizontal(input, "XMAS|SAMX");
        answer += CountStringDiagonal(input, "XMAS");
        answer += CountStringDiagonal(input, "SAMX");

        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;



        return answer;
    }

    private int CountStringHorizontal(string[] input, string pattern)
    {
        return input.Sum(s =>
        {
            return Regex.Matches(s, pattern).Count;
        });
    }

    private int CountStringDiagonal(string[] input, string search)
    {
        int count = 0;
        for (int i = 0; i < input.Length - search.Length; i++)
        {
            for (int j = 0; j < input[i].Length - search.Length; j++)
            {
                string s = "";
                for (int k = j; k < j + search.Length; k++)
                {
                    s += input[i + k][k];
                }
                if (s == search)
                {
                    count++;
                }
            }
            
        }
        return count;
    }
}

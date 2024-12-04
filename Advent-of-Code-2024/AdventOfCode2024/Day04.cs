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

        string search = "XMAS";
        answer += CountStringHorizontal(input, search);
        answer += CountStringHorizontal(Transpose(input), search);
        answer += CountStringDiagonal(input, search);
        answer += CountStringDiagonal(Reverse(input), search);
        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;
        var input = Input.Split("\r\n");

        answer = CountStringCrossDiagonal(input, "MAS");

        return answer;
    }

    private int CountStringHorizontal(string[] input, string search)
    {
        return input.Sum(s => 
            Regex.Matches(s, search).Count
            + Regex.Matches(s, new string(search.Reverse().ToArray())).Count);
    }

    private int CountStringDiagonal(string[] input, string search)
    {
        int count = 0;
        for (int i = 0; i < input.Length - search.Length + 1; i++)
        {
            for (int j = 0; j < input[i].Length - search.Length + 1; j++)
            {
                string s = "";
                for (int k = j; k < j + search.Length; k++)
                {
                    s += input[i + k - j][k];
                }
                if (s == search || s == new string(search.Reverse().ToArray()))
                {
                    count++;
                }
            }
            
        }
        return count;
    }
    private int CountStringCrossDiagonal(string[] input, string search)
    {
        int count = 0;
        for (int i = 0; i < input.Length - search.Length + 1; i++)
        {
            for (int j = 0; j < input[i].Length - search.Length + 1; j++)
            {
                bool left = ContainsDiagonal(input, j, i, search, Direction.Forward);
                bool right = ContainsDiagonal(input, j + (search.Length - 1), i, search, Direction.Backward);
                string searchReverse = new string(search.Reverse().ToArray());
                bool leftReverse = ContainsDiagonal(input, j, i, searchReverse, Direction.Forward);
                bool rightReverse = ContainsDiagonal(input, j + (search.Length - 1), i, searchReverse, Direction.Backward);
                if ((left || leftReverse) && (right || rightReverse))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool ContainsDiagonal(string[] input, int startX, int startY, string search, Direction direction)
    {
        for (int i = 0; i < search.Length; i++)
        {
            if (input[startY + i][startX + i * (int)direction] != search[i])
            {
                return false;
            }
        }
        return true;
    }

    private string[] Transpose(string[] input)
    {
        string[] output = new string[input[0].Length];
        for (int i = 0; i < input[0].Length; i++)
        {
            output[i] = "";
            for (int j = 0; j < input.Length; j++)
            {
                output[i] += input[j][i];
            }
        }
        return output;
    }

    private string[] Reverse(string[] input)
    {
        string[] output = new string[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = new string(input[i].Reverse().ToArray());
        }
        return output;
    }

    private enum Direction
    {
        Forward = 1,
        Backward = -1
    }
}

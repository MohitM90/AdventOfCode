using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day07 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");
        inputs[0] = inputs[0].Replace('S', '|');
        for (int i = 0; i < inputs.Length - 1; i++)
        {
            var indexes = inputs[i].AllIndexesOf('|');
            var nextline = inputs[i + 1];
            foreach (var index in indexes)
            {
                if (nextline[index] == '.')
                {
                    nextline = nextline.ReplaceAt(index, '|');
                }
                else if (nextline[index] == '^')
                {
                    nextline = nextline.ReplaceAt(index - 1, '|');
                    nextline = nextline.ReplaceAt(index + 1, '|');
                    answer++;
                }

            }
            inputs[i + 1] = nextline;
        }

        return answer;
    }

    public override async Task<long> Puzzle2(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");



        return answer;
    }
}
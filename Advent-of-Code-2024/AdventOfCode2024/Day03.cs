using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day03 : BaseDay
{
    public override long Puzzle1()
    {
        int answer = 0;

        var inputs = Regex.Matches(Input, "mul\\((\\d{1,3}),(\\d{1,3})\\)");

        answer = inputs.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum();

        return answer;
    }

    public override long Puzzle2()
    {
        int answer = 0;

        var inputs = Regex.Matches("do()" + Input, "do\\(\\).*?(?:mul\\(\\d{1,3},\\d{1,3}\\)).*?(?:don't\\(\\)|$)", RegexOptions.Singleline);

        foreach (Match m in inputs)
        {
            var groups = Regex.Matches(m.Value, "mul\\((\\d{1,3}),(\\d{1,3})\\)");
            answer += groups.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum();
        }

        return answer;
    }
}

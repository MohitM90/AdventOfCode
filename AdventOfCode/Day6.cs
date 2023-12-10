using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day6
{
    internal static long PuzzleDay6a(string input)
    {
        long sum = 1;
        string[] inputs = input.Split("\r\n");
        var times = Regex.Matches(inputs[0], "(\\d+)")
            .Select(x => long.Parse(x.Value)).ToArray();
        var distance = Regex.Matches(inputs[1], "(\\d+)")
            .Select(x => long.Parse(x.Value)).ToArray();
        for (long t = 0; t < times.Length; t++)
        {
            long time = times[t];
            long recordDistance = distance[t];
            List<long> wins = [];
            for (long i = 1; i < time; i++)
            {
                long tempDistance = i * (time - i);
                if (tempDistance > recordDistance)
                {
                    wins.Add(tempDistance);
                }
            }
            sum *= wins.Count;
        }

        return sum;
    }
}

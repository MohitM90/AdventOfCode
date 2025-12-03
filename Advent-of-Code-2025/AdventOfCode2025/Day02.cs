using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day02 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string input)
    {
        long answer = 0;
        var inputs = input.Split(",");
        foreach (var i in inputs)
        {
            var range = i.Split("-");
            var minLen = range[0].Length;
            var maxLen = range[1].Length;

            if (minLen % 2 == 1 && maxLen % 2 == 1)
            {
                continue;
            }

            var len = (minLen % 2 == 1 ? maxLen : minLen) / 2;

            long min = long.Parse(range[0]);

            var max = long.Parse(range[1]);
            var minUpper = minLen % 2 == 1 && len > 1 ? (long)Math.Max(long.Parse(range[0][..(len - 1)]), Math.Pow(10, len-1)) : long.Parse(range[0][..len]);

            long b = (long)Math.Pow(10, len);

            long n = minUpper;
            while (n < b && n * b + n <= max)
            {
                long id = n * b + n;
                if (id >= min)
                {
                    answer += id;
                }
                
                n++;
            }
        }

        return answer;
    }

    public override async Task<long> Puzzle2(string input)
    {
        long answer = 0;
        var inputs = input.Split(",");
        foreach (var i in inputs)
        {
            var range = i.Split("-");
            var minLen = range[0].Length;
            var maxLen = range[1].Length;

            long min = long.Parse(range[0]);
            long max = long.Parse(range[1]);

            int n = 1;
            var answers = new HashSet<long>();
            while (n <= maxLen/2)
            {
                for (int x = (int)Math.Pow(10, n - 1); x <= (int)Math.Pow(10, n); x++)
                {
                    var substr = x.ToString()[..n].Repeat(minLen/n);
                    long val = long.Parse(substr);

                    if (val > max)
                    {
                        break;
                    }

                    if (val >= min && val <= max)
                    {
                        answers.Add(val);
                    }

                    if (minLen != maxLen)
                    {
                        substr = x.ToString()[..n].Repeat(maxLen / n);
                        val = long.Parse(substr);
                        if (val >= min && val <= max)
                        {
                            answers.Add(val);
                        }
                    }
                }
                n++;
            }
            foreach (var a in answers)
            {
                if (a < 10)
                {
                    continue;
                }
                //Console.WriteLine(a);
                answer += a;
            }
        }

        return answer;
    }
}

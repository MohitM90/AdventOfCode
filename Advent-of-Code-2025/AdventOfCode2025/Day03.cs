using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day03 : BaseDay<ulong>
{
    public override async Task<ulong> Puzzle1(string input)
    {
        ulong answer = 0;
        var inputs = input.Split("\r\n");
        foreach(var i in inputs)
        {
            uint max = 0;
            bool found = false;
            for (char c = '9';  c >= '0'; c--)
            {

                var indexes = i.Select((item, idx) => c == item ? idx : -1).Where(idx => idx != -1);
                foreach (var idx in indexes)
                {
                    var bigValues = i.Where((x, n) => n > idx).Select(x => uint.Parse("" + c + x));
                    if (bigValues.Any() && bigValues.Max() > max)
                    {
                        max = bigValues.Max();
                        found = true;
                    }
                }
                if (found) break;
            }
            answer += max;
        }


        return answer;
    }

    public override async Task<ulong> Puzzle2(string data)
    {
        ulong answer = 0;
        var inputs = data.Split("\r\n");

        foreach (var input in inputs)
        {
            //Console.WriteLine();
            //Console.WriteLine(input);

            string largestStr = input[(input.Length - 12)..(input.Length)];
            //Console.WriteLine(largestStr);
            for (int i = input.Length - 13; i >= 0; i--)
            {          
                if (input[i] >= largestStr[0])
                {
                    //Console.WriteLine(largestStr);
                    var smallIndex = -1;
                    for(char c = '0'; c <= '9'; c++)
                    {
                        var index = largestStr.IndexOf(c);
                        if (index != -1)
                        {
                            smallIndex = index;
                            break;
                        }
                    }
                    for (int n = 0; n < largestStr.Length - 1; n++)
                    {
                        if (largestStr[n] < largestStr[n + 1])
                        {
                            smallIndex = n;
                            break;
                        }
                    }

                    if (smallIndex != -1)
                    {
                        largestStr = input[i] + largestStr.Remove(smallIndex, 1);
                        //Console.WriteLine(largestStr);
                    }
                }
            }
            //Console.WriteLine(largestStr);
            answer += ulong.Parse(largestStr);
        }

        return answer;
    }
}

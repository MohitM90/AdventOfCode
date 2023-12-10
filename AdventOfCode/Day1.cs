using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

internal class Day1
{

    static int PuzzleDay1a(string input)
    {
        int sum = 0;
        foreach (var s in input.Split("\r\n"))
        {
            var num1 = s.First(x => x >= '0' && x <= '9').ToString() ?? string.Empty;
            var num2 = s.Last(x => x >= '0' && x <= '9').ToString() ?? string.Empty;
            if (int.TryParse(num1 + num2, out int tempSum))
            {
                sum += tempSum;
            }
        }
        return sum;
    }

    static int PuzzleDay1b(string input)
    {
        int sum = 0;
        string[] numbers = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        foreach (var s in input.Split("\r\n"))
        {
            var minText = numbers.Select(x => new { number = x, idx = s.IndexOf(x) })
                .Where(x => x.idx > -1)
                .OrderBy(x => x.idx)
                .FirstOrDefault();
            char numIndex = s.FirstOrDefault(x => x >= '0' && x <= '9');
            int minIndex = numIndex == default ? int.MaxValue : s.IndexOf(numIndex.ToString());

            var num1 = minText == null ? numIndex.ToString() : minText.idx < minIndex ? ToNumeric(minText.number) : numIndex.ToString();

            var maxText = numbers.Select(x => new { number = x, idx = s.LastIndexOf(x) })
                .Where(x => x.idx > -1)
                .OrderBy(x => x.idx)
                .LastOrDefault();
            numIndex = s.LastOrDefault(x => x >= '0' && x <= '9');
            int maxIndex = numIndex == default ? -1 : s.LastIndexOf(numIndex.ToString());

            var num2 = maxText == null ? numIndex.ToString() : maxText.idx > maxIndex ? ToNumeric(maxText.number) : numIndex.ToString();

            if (int.TryParse(num1 + num2, out int tempSum))
            {
                sum += tempSum;
                Console.WriteLine($"{s} => {num1} + {num2} = {tempSum}");
            }
            else
            {
                Console.WriteLine($"Cannot parse!!! {s}");
            }
        }
        return sum;
    }


    private static string ToNumeric(string input)
    {
        return input.Replace("one", "1")
            .Replace("two", "2")
            .Replace("three", "3")
            .Replace("four", "4")
            .Replace("five", "5")
            .Replace("six", "6")
            .Replace("seven", "7")
            .Replace("eight", "8")
            .Replace("nine", "9");
    }

}

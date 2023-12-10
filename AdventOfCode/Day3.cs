using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day3
{
    internal static int PuzzleDay3a(string input)
    {
        int sum = 0;
        var s = input.Split("\r\n");
        for (int i = 0; i < s.Length; i++)
        {
            var yStart = i - 1;
            var yEnd = i + 1;
            var matches = Regex.Matches(s[i], "(\\d+)");

            foreach (var match in matches.Cast<Match>())
            {
                var xStart = int.Max(match.Index - 1, 0);
                var xEnd = int.Min(match.Index + match.Length + 1, s.Length);

                if ((yStart >= 0 && ContainsSymbol(s[yStart][xStart..xEnd])) ||
                    (ContainsSymbol(s[i][xStart..xEnd])) ||
                    (yEnd < s.Length && ContainsSymbol(s[yEnd][xStart..xEnd])))
                {
                    sum += int.Parse(match.Value);
                    continue;
                }
            }
        }
        return sum;
    }

    private static bool ContainsSymbol(string input)
    {
        return input.Any(c => c != '.' && !(c >= '0' && c <= '9'));
    }

    internal static int PuzzleDay3b(string input)
    {
        int sum = 0;
        var s = input.Split("\r\n");
        for (int i = 0; i < s.Length; i++)
        {
            var yStart = i - 1;
            var yEnd = i + 1;
            var starMatches = Regex.Matches(s[i], "(\\*)");

            foreach (var starMatch in starMatches.Cast<Match>())
            {
                var starIndex = starMatch.Index;
                List<int> numbers = [];

                List<int> rows = [yStart, i, yEnd];
                foreach (var y in rows)
                {
                    if (y < 0 || y >= s.Length) continue;

                    var numberMatches = Regex.Matches(s[y], "(\\d+)");
                    foreach (var numberMatch in numberMatches.Cast<Match>())
                    {
                        var numberIndex = numberMatch.Index;
                        var numberLength = numberMatch.Length;
                        if (numberIndex >= starIndex-numberLength && numberIndex <= starIndex + 1)
                        {
                            numbers.Add(int.Parse(numberMatch.Value));
                        }
                    }
                }
                if (numbers.Count != 2)
                {
                    continue;
                }

                sum += numbers[0] * numbers[1];
            }
        }
        return sum;
    }

    private static bool IsNumeric(char c)
    {
        return c >= '0' && c <= '9';
    }

}

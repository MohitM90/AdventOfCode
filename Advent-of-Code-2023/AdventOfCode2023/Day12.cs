using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day12
{
    internal static async Task<int> PuzzleA(string input)
    {
        var inputs = input.Split("\r\n");
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<int>> tasks = [];

        foreach (var s in inputs)
        {
            var task = Task.Run(() =>
            {
                var data = s.Split(" ");
                var unknownRegex = new Regex("\\?");
                var matches = unknownRegex.Matches(data[0]);


                int tests = (int)Math.Pow(2, matches.Count);
                int decimals = matches.Count;
                var pattern = BuildRegexPattern(data[1].Split(","));
                var testRegex = new Regex(pattern);

                var tempSum = 0;
                for (var i = 0; i < tests; i++)
                {
                    string temp = data[0];
                    for (int n = 0; n < decimals; n++)
                    {
                        if (((i >> (decimals - 1 - n)) & 1) == 1)
                        {
                            temp = unknownRegex.Replace(temp, "#", 1);
                        }
                        else
                        {
                            temp = unknownRegex.Replace(temp, ".", 1);
                        }
                    }
                    
                    if (testRegex.IsMatch(temp))
                    {
                        Console.WriteLine(temp);
                        tempSum++;
                    }
                }
                return tempSum;
            });
            tasks.Add(task);
        }
        var result = await Task.WhenAll(tasks);

        Console.WriteLine(sw.ToString());

        return result.Sum();
    }

    internal static async Task<ulong> PuzzleB(string input)
    {
        var inputs = input.Split("\r\n").ToList();
        //foreach (var s in inputs.ToList())
        //{
        //    var data = s.Split(" ");
        //    if (data[0].StartsWith('?') && data[0].EndsWith('?'))
        //    {
        //        inputs.Remove(s);
        //        inputs.Add("." + data[0][1..(data[0].Length-1)] + ". " + data[1] + " " + data[0]);
        //        inputs.Add("." + data[0][1..(data[0].Length-1)] + "# " + data[1] + " " + data[0]);
        //        inputs.Add("#" + data[0][1..(data[0].Length-1)] + ". " + data[1] + " " + data[0]);
        //        inputs.Add("#" + data[0][1..(data[0].Length-1)] + "# " + data[1] + " " + data[0]);
        //    }
        //    else if (data[0].StartsWith('?'))
        //    {
        //        inputs.Remove(s);
        //        inputs.Add("." + data[0][1..(data[0].Length)] + " " + data[1] + " " + data[0]);
        //        inputs.Add("#" + data[0][1..(data[0].Length)] + " " + data[1] + " " + data[0]);
        //    }
        //    else if (data[0].EndsWith('?'))
        //    {
        //        inputs.Remove(s);
        //        inputs.Add(data[0][0..(data[0].Length - 1)] + ". " + data[1] + " " + data[0]);
        //        inputs.Add(data[0][0..(data[0].Length - 1)] + "# " + data[1] + " " + data[0]);
        //    } 
        //    else
        //    {
        //        inputs.Remove(s);
        //        inputs.Add(data[0] + " " + data[1] + " " + data[0]);
        //    }
        //}
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<ulong>> tasks = [];

        foreach (var s in inputs)
        {
            var task = Task.Run(() =>
            {
                //var data = s.Split(" ");
                //var patternOriginal = data[0];
                //var groupSize = data[1].Split(",");
                //var numOriginal = GetNumArrangements(patternOriginal, groupSize);

                //string patternLeft = (patternOriginal.EndsWith('#') ? "." : "?") + data[2];
                //string patternRight = data[2] + "?";

                //var numLeft = GetNumArrangements(patternLeft, groupSize);
                //var numRight = GetNumArrangements(patternRight, groupSize);

                //return numOriginal * (ulong)Math.Pow(Math.Max(numLeft, numRight), 4);
                var data = s.Split(" ");
                var patternOriginal = string.Join('?', Enumerable.Repeat(data[0], 5));
                var groupSize = string.Join(',', Enumerable.Repeat(data[1], 5)).Split(",").Select(s => int.Parse(s)).ToList();
                var numOriginal = GetNumArrangements(patternOriginal, groupSize);
                Console.WriteLine(s + " done");
                return numOriginal;
            });
            tasks.Add(task);
        }
        var result = await Task.WhenAll(tasks);

        Console.WriteLine(sw.ToString());

        return result.Aggregate((current, next) => current + next);
    }

    private static ulong GetNumArrangements(string input, string[] groupSize)
    {
        var unknownRegex = new Regex("\\?");
        var matches = unknownRegex.Matches(input);


        int tests = (int)Math.Pow(2, matches.Count);
        int decimals = matches.Count;
        var pattern = BuildRegexPattern(groupSize);
        var testRegex = new Regex(pattern);

        var tempSum = 0ul;
        for (var i = 0; i < tests; i++)
        {
            string temp = input;
            for (int n = 0; n < decimals; n++)
            {
                if (((i >> (decimals - 1 - n)) & 1) == 1)
                {
                    temp = unknownRegex.Replace(temp, "#", 1);
                }
                else
                {
                    temp = unknownRegex.Replace(temp, ".", 1);
                }
            }
            if (testRegex.IsMatch(temp))
            {

                tempSum++;
            }
        }
        return tempSum;
    }

    private static ulong GetNumArrangements(string input, IList<int> pattern)
    {
        var unknownRegex = new Regex("\\?");
        var matches = unknownRegex.Matches(input);
        Dictionary<int, int> indexMap = [];
        for (int i = 0; i < matches.Count; i++)
        {
            indexMap.Add(i, matches[i].Index);
        }
        bool[] records = input.Select(c => c == '#').ToArray();
        var tests = BigInteger.Pow(2, matches.Count); //(ulong)Math.Pow(2, matches.Count);
        int decimals = matches.Count;

        var tempSum = 0ul;
        for (var i = BigInteger.Zero; i < tests; i++)
        {
            for (var n = 0; n < decimals; n++)
            {
                records[indexMap[n]] = ((i >> (decimals - 1 - n)) & BigInteger.One) == BigInteger.One;
            }
            if (Matches(records, pattern))
            {
                tempSum++;
            }
        }

        return tempSum;
    }

    private static bool HasValidArrangement(string input, string[] groupSize)
    {
        var unknownRegex = new Regex("\\?");
        var matches = unknownRegex.Matches(input);


        int tests = (int)Math.Pow(2, matches.Count);
        int decimals = matches.Count;
        var pattern = BuildRegexPattern(groupSize);
        var testRegex = new Regex(pattern);

        for (var i = 0; i < tests; i++)
        {
            string temp = input;
            for (int n = 0; n < decimals; n++)
            {
                if (((i >> (decimals - 1 - n)) & 1) == 1)
                {
                    temp = unknownRegex.Replace(temp, "#", 1);
                }
                else
                {
                    temp = unknownRegex.Replace(temp, ".", 1);
                }
            }
            if (testRegex.IsMatch(temp))
            {

                return true;
            }
        }
        return false;
    }

    private static string BuildRegexPattern(string[] data)
    {
        string pattern = "(?<!#.*)\\#{";

        pattern += string.Join("}[\\.]+\\#{", data);

        pattern += "}(?!.*#)";

        return pattern;
    }

    public static bool Matches(bool[] bitArray, IList<int> pattern)
    {
        int patternIndex = 0;
        int count = 0;
        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i])
            {
                count++;
            }
            if (!bitArray[i] && count > 0)
            {
                if (patternIndex < pattern.Count && count == pattern[patternIndex])
                {
                    patternIndex++;
                    count = 0;
                } 
                else
                {
                    return false;
                }
            }
        }
        if (count > 0)
        {
            if (patternIndex < pattern.Count && count == pattern[patternIndex])
            {
                patternIndex++;
            }
            else
            {
                return false;
            }
        }
        if (patternIndex == pattern.Count)
        {
            return true;
        }
        return false;
    }
}

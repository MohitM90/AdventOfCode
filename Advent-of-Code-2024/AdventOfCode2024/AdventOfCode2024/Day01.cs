using System.Diagnostics;

namespace AdventOfCode2024;
internal class Day01
{
    internal static int Puzzle1(string input)
    {
        int answer = 0;
        Stopwatch sw = Stopwatch.StartNew();

        var inputs = input.Split("\r\n").Select(s => s.Split("   "));
        var list1 = inputs.Select(s => int.Parse(s[0]));
        var list2 = inputs.Select(s => int.Parse(s[1]));
        list1 = list1.Order();
        list2 = list2.Order();
        answer = list1.Zip(list2, (a, b) => Math.Abs(a - b)).Sum();

        Console.WriteLine("Time: " + sw.ToString() + "\r\n");
        return answer;
    }

    internal static int Puzzle2(string input)
    {
        int answer = 0;
        Stopwatch sw = Stopwatch.StartNew();

        var inputs = input.Split("\r\n").Select(s => s.Split("   "));
        var list1 = inputs.Select(s => int.Parse(s[0]));
        var list2 = inputs.Select(s => int.Parse(s[1]));
        list1 = list1.Order();
        list2 = list2.Order();
        answer = list1.Select(a => a * list2.Where(b => a == b).Count()).Sum();

        Console.WriteLine("Time: " + sw.ToString() + "\r\n");
        return answer;
    }
}

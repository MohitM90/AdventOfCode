using System.Diagnostics;

namespace AdventOfCode2024;

internal class Day01 : BaseDay
{

    public override int Puzzle1()
    {
        int answer = 0;

        var inputs = Input.Split("\r\n").Select(s => s.Split("   "));
        var list1 = inputs.Select(s => int.Parse(s[0]));
        var list2 = inputs.Select(s => int.Parse(s[1]));
        list1 = list1.Order();
        list2 = list2.Order();

        answer = list1.Zip(list2, (a, b) => Math.Abs(a - b)).Sum();

        return answer;
    }

    public override int Puzzle2()
    {
        int answer = 0;

        var inputs = Input.Split("\r\n").Select(s => s.Split("   "));
        var list1 = inputs.Select(s => int.Parse(s[0]));
        var list2 = inputs.Select(s => int.Parse(s[1]));
        list1 = list1.Order();
        list2 = list2.Order();

        answer = list1.Select(a => a * list2.Where(b => a == b).Count()).Sum();

        return answer;
    }
}

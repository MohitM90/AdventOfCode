using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day9
{
    internal static int PuzzleA(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n");
        foreach (var s in inputs)
        {
            sum += ExtrapolateNext(s.Split(" ").Select(x => int.Parse(x)).ToList());
        }

        return sum;
    }

    private static int ExtrapolateNext(List<int> history)
    {
        if (history.All(x => x == 0))
        {
            return 0;
        }
        List<int> list = new();
        for (int i = 1; i < history.Count; i++)
        {
            list.Add(history[i] - history[i-1]);
        }
        history.Add(history.Last() + ExtrapolateNext(list));
        return history.Last();
    }

    internal static int PuzzleB(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n");
        foreach (var s in inputs)
        {
            sum += ExtrapolatePrevious(s.Split(" ").Select(x => int.Parse(x)).ToList());
        }

        return sum;
    }

    private static int ExtrapolatePrevious(List<int> history)
    {
        if (history.All(x => x == 0))
        {
            return 0;
        }
        List<int> list = new List<int>();
        for (int i = 1; i < history.Count; i++)
        {
            list.Add(history[i] - history[i - 1]);
        }
        history.Insert(0, history.First() - ExtrapolatePrevious(list));
        return history.First();
    }
}

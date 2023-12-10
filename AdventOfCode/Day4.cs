using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day4
{
    internal static int PuzzleDay4a(string input)
    {
        int sum = 0;
        string[] inputs = input.Split("\r\n");
        foreach (var s in inputs)
        {
            string[] data = s.Split(':', '|');

            var winning = Regex.Matches(data[1], "(\\d+)")
                .Select(x => int.Parse(x.Value))
                .ToList();
            var drawn = Regex.Matches(data[2], "(\\d+)")
                .Select(x => int.Parse(x.Value))
                .ToList();
            var num = drawn.Where(x => winning.Contains(x)).Count();
            if (num > 0)
            {
                sum += (int)Math.Pow(2, num - 1);
            }
        }

        return sum;
    }

    internal static int PuzzleDay4b(string input)
    {
        int sum = 0;
        string[] inputs = input.Split("\r\n");
        Dictionary<int, int> cards = new();

        foreach (var s in inputs)
        {
            string[] data = s.Split(':', '|');
            var id = int.Parse(Regex.Match(data[0], "(\\d+)").Value);
            Add(cards, id, 1);

            var winning = Regex.Matches(data[1], "(\\d+)")
                .Select(x => int.Parse(x.Value))
                .ToList();
            var drawn = Regex.Matches(data[2], "(\\d+)")
                .Select(x => int.Parse(x.Value))
                .ToList();
            var num = drawn.Where(x => winning.Contains(x)).Count();
            for (int i = id + 1; i <= Math.Min(id + num, inputs.Length); i++)
            {
                Add(cards, i, cards[id]);
            }
        }

        sum = cards.Sum(x => x.Value);

        return sum;
    }

    private static void Add(Dictionary<int, int> cards, int key, int num)
    {
        if (!cards.TryAdd(key, num))
        {
            cards[key] += num;
        }
    }
}

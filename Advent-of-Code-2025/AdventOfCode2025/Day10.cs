using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day10 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        foreach (var input in inputs)
        {
            var machine = input.Split(' ');

            long target = (long)machine[0].Replace("[", "").Replace("]", "")
                .Select((c, i) => c == '.' ? 0 : Math.Pow(2, i)).Sum();
            long[] buttonSets = machine[1..(machine.Length - 1)].Select(b => b.Replace("(", "").Replace(")", "").Split(','))
                .Select(x => x.Select(b => (long)Math.Pow(2, long.Parse(b))).Sum()).ToArray();

            long initial = 0;

            Queue<long> queue = new Queue<long>();
            Dictionary<long, int> visited = new Dictionary<long, int>();
            visited.Add(initial, 0);
            queue.Enqueue(initial);
            while (queue.Count > 0)
            {
                int level = visited[queue.Peek()];
                var current = queue.Dequeue();
                foreach (var buttonSet in buttonSets)
                {
                    long next = current ^ buttonSet;
                    if (!visited.ContainsKey(next))
                    {
                        visited.Add(next, level + 1);
                        queue.Enqueue(next);
                        if (next == target)
                        {
                            answer += level + 1;
                            queue.Clear();
                            break;
                        }
                    }
                }
            }
        }



        return answer;
    }

    public override async Task<long> Puzzle2(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");



        return answer;
    }
}

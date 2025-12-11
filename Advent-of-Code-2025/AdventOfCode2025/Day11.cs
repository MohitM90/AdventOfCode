using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day11 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        Dictionary<string, HashSet<string>> connections = [];
        foreach ( var input in inputs )
        {
            var split = input.Split(": ");
            connections.Add(split[0], [.. split[1].Split(' ')]);
        }

        var initial = "you";
        var target = "out";
        var queue = new Queue<string>();
        queue.Enqueue(initial);
        while (queue.Count > 0)
        {
            var input = queue.Dequeue();
            if (input == target)
            {
                answer++;
                continue;
            }
            var paths = connections[input];
            foreach (var path in paths)
            {
                queue.Enqueue(path);
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

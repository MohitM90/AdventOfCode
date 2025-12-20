using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2025;

internal class Day11 : BaseDay<long>
{
    internal Day11() : base(useExample2: true)
    {
    }

    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var input in inputs)
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
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var input in inputs)
        {
            var split = input.Split(": ");
            connections.Add(split[0], [.. split[1].Split(' ')]);
        }

        Dictionary<string, (long all, long withFft, long withDac, long both)> paths = [];
        var result = GetPaths2("svr", "out", connections, paths);

        answer = result.both;
        return answer;
    }

    private (long all, long withFft, long withDac, long both) GetPaths2(string current, string target, Dictionary<string, HashSet<string>> connections, Dictionary<string, (long all, long withFft, long withDac, long both)> known)
    {
        var fft = current == "fft";
        var dac = current == "dac";
        if (current == target)
        {
            return (
                1,
                fft ? 1 : 0, 
                dac ? 1 : 0,
                0
            );
        }

        if (known.TryGetValue(current, out (long all, long withFft, long withDac, long both) value))
        {
            return (
                value.all, 
                fft ? known[current].all : known[current].withFft, 
                dac ? known[current].all : known[current].withDac,
                fft ? known[current].withDac : dac ? known[current].withFft : known[current].both
            );
        }

        (long all, long withFft, long withDac, long both) count = (0, 0, 0, 0);
        foreach (var neighbor in connections[current])
        {
            var (nAll, nWithFft, nWithDac, nBoth) = GetPaths2(neighbor, target, connections, known);
            count.all += nAll;
            count.withFft += nWithFft;
            count.withDac += nWithDac;
            count.both += nBoth;
        }
        known[current] = (
                count.all,
                fft ? count.all : count.withFft,
                dac ? count.all : count.withDac,
                fft ? count.withDac : dac ? count.withFft : count.both
            );
        return known[current];
    }
}

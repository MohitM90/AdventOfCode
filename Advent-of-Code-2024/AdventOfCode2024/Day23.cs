using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day23 : BaseDay<string>
{

    public override string Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n")
            .Select(x => x.Split("-"))
            .SelectMany(x => new (string, string)[] { (x[0], x[1]), (x[1], x[0]) })
            .Distinct()
            .ToLookup(x => x.Item1, x => x.Item2);

        var network = FindInterconnected(input);

        answer = network.Where(x => x.c1.StartsWith('t') || x.c2.StartsWith('t') || x.c3.StartsWith('t')).Count();
        
        return $"{answer}";
    }

    public override string Puzzle2()
    {
        string answer = "";
        var input = Input.Split("\r\n")
            .Select(x => x.Split("-"))
            .SelectMany(x => new (string, string)[] { (x[0], x[1]), (x[1], x[0]) })
            .Distinct()
            .ToLookup(x => x.Item1, x => x.Item2);

        var network = FindInterconnected(input);

        Dictionary<string, int> occurences = [];
        foreach (var key in input)
        {
            occurences[key.Key] = network.Count(x => x.c1 == key.Key || x.c2 == key.Key || x.c3 == key.Key);
        }

        Dictionary<(string c1, string c2, string c3), int> occurences2 = [];
        foreach (var key in network)
        {
            occurences2[key] = occurences[key.c1] + occurences[key.c2] + occurences[key.c3];
        }
        int max = occurences2.Max(x => x.Value);
        HashSet<string> computers = [];
        foreach (var key in occurences2.Where(x => x.Value == max))
        {
            computers.Add(key.Key.c1);
            computers.Add(key.Key.c2);
            computers.Add(key.Key.c3);
        }

        answer = string.Join(",", computers.Order());

        return answer;
    }

    private List<(string c1, string c2, string c3)> FindInterconnected(ILookup<string, string> lookup)
    {
        List<(string c1, string c2, string c3)> network = [];
        foreach (var group in lookup)
        {
            var c1 = group.Key;
            var pairs = group.SelectMany(x => group, (c2, c3) => (c2, c3)).Where(x => x.c2 != x.c3).ToList();
            foreach (var pair in pairs)
            {
                if (lookup[pair.c2].Contains(pair.c3))
                {
                    if (!network.Contains((c1, pair.c2, pair.c3)) && !network.Contains((c1, pair.c3, pair.c2)) &&
                        !network.Contains((pair.c2, c1, pair.c3)) && !network.Contains((pair.c3, c1, pair.c2)) &&
                        !network.Contains((pair.c2, pair.c3, c1)) && !network.Contains((pair.c3, pair.c2, c1)))
                        network.Add((c1, pair.c2, pair.c3));
                }
            }
        }
        return network;
    }

}
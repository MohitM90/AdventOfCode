using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

internal class Day25 : BaseDay<long>
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n\r\n").Select(x => x.Split("\r\n"));
        List<int[]> locks = [];
        List<int[]> keys = [];

        int pins = 5;
        int maxHeight = 5;

        string lockString = new('#', pins);
        foreach (var input in inputs)
        {
            var heights = Enumerable.Range(0, pins).Select(x => input.Skip(1).Take(maxHeight).Sum(y => y[x] == '#' ? 1 : 0)).ToArray();
            if (input[0] == lockString)
            {

                locks.Add(heights);
            }
            else
            {
                keys.Add(heights);
            }
        }

        answer = GetMatchingKeyLockPairs(locks, keys, maxHeight).Count();


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n");



        return answer;
    }


    private static IEnumerable<(int[] Lock, int[] Key)> GetMatchingKeyLockPairs(List<int[]> locks, List<int[]> keys, int maxHeight)
    {
        return locks.SelectMany(l => keys.Select(k => (Lock: l, Key: k)))
                    .Where(c => c.Lock.Zip(c.Key, (l, k) => l + k).All(x => x <= maxHeight));
    }
}

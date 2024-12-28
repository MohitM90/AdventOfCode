using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day22 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n").Select(x => long.Parse(x));

        foreach (var seed in input)
        {
            var secret = seed;
            for (int i = 0; i < 2000; i++)
            {
                secret = GetRandomNumber(secret);
            }
            answer += secret;
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n").Select(x => long.Parse(x));
        List<List<long>> prices = [];
        List<Dictionary<(long A, long B, long C, long D), long>> changeSequences = [];
        foreach (var seed in input)
        {
            List<long> price = [];
            Dictionary<(long A, long B, long C, long D), long> changeSequence = [];
            var secret = seed;
            for (int i = 0; i < 2000; i++)
            {
                price.Add(secret % 10);
                if (i >= 4)
                {
                    var change = (
                        price[i - 3] - price[i - 4],
                        price[i - 2] - price[i - 3],
                        price[i - 1] - price[i - 2],
                        price[i] - price[i - 1]);
                    if (!changeSequence.ContainsKey(change))
                    {
                        changeSequence.Add(change, price[i]);
                    }
                }
                secret = GetRandomNumber(secret);
            }
            prices.Add(price);
            changeSequences.Add(changeSequence);
        }

        foreach (var changeSequence in changeSequences)
        {
            var changes = changeSequence.OrderByDescending(x => x.Value);
            foreach (var change in changes)
            {
                long sum = 0;
                foreach (var changeSequence2 in changeSequences.Where(x => x.Count > 0))
                {
                    if (changeSequence2.TryGetValue(change.Key, out var temp))
                    {
                        sum += temp;
                        changeSequence2.Remove(change.Key);
                    }
                }
                if (sum > answer)
                {
                    answer = sum;
                }
            }
        }
        return answer;

    }

    
    private long GetRandomNumber(long seed)
    {
        seed = ((seed * 64L) ^ seed) % 16777216;
        seed = ((seed / 32L) ^ seed) % 16777216;
        seed = ((seed * 2048L) ^ seed) % 16777216;
        return seed;
    }
}
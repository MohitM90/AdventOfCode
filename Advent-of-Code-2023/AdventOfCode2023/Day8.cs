using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day8
{
    internal static int PuzzleA(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n");
        Instruction[] instructions = inputs[0].Select(c => (Instruction)Enum.Parse(typeof(Instruction), c.ToString())).ToArray();
        Dictionary<string, string[]> map = [];
        for (int i = 2; i < inputs.Length; i++)
        {
            var match = Regex.Match(inputs[i], "(...) = \\((...), (...)\\)");
            map.Add(match.Groups[1].Value, [match.Groups[2].Value, match.Groups[3].Value]);
        }
        var key = "AAA";
        int pos = 0;
        while (key != "ZZZ")
        {
            key = map[key][(int)instructions[pos]];
            sum++;
            pos++;
            pos %= instructions.Length;
        }

        return sum;
    }

    internal static long PuzzleB(string input)
    {

        var inputs = input.Split("\r\n");
        Instruction[] instructions = inputs[0].Select(c => (Instruction)Enum.Parse(typeof(Instruction), c.ToString())).ToArray();
        Dictionary<string, string[]> map = [];
        for (int i = 2; i < inputs.Length; i++)
        {
            var match = Regex.Match(inputs[i], "(...) = \\((...), (...)\\)");
            map.Add(match.Groups[1].Value, [match.Groups[2].Value, match.Groups[3].Value]);
        }

        var keys = map.Keys.Where(x => x.EndsWith('A')).ToArray();
        var trips = new List<Trip>();
        foreach (var key in keys)
        {
            var trip = new Trip();
            string k = key;
            int pos = 0;
            long sum = 0;
            while (!k.EndsWith('Z'))
            {
                k = map[k][(int)instructions[pos]];
                sum++;
                pos++;
                pos %= instructions.Length;
            }
            string endKey = k;
            trip.First = sum;
            sum = 0;
            do
            {
                k = map[k][(int)instructions[pos]];
                sum++;
                pos++;
                pos %= instructions.Length;
            } while (k != endKey);
            trip.Second = sum;
            trips.Add(trip);
        }

        return CalculateLCM(trips.Select(x => x.First).ToArray());
    }

    // Hilfsfunktion, um den größten gemeinsamen Teiler (ggT) zweier Zahlen zu finden
    internal static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // Funktion zur Berechnung des kgV einer Liste von Zahlen
    internal static long CalculateLCM(long[] numbers)
    {
        if (numbers.Length < 2)
        {
            throw new ArgumentException("Es werden mindestens zwei Zahlen benötigt.");
        }

        long lcm = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            lcm = (lcm * numbers[i]) / GCD(lcm, numbers[i]);
        }
        return lcm;
    }


    internal enum Instruction
    {
        L = 0,
        R = 1
    }

    private class Trip
    {
        public long First { get; set; }
        public long Second { get; set; }
    }
}

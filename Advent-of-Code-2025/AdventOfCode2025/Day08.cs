using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day08 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        int connections = inputs.Length <= 20 ? 10 : 1000;
        List<JuncBox> boxes = [.. inputs.Select(line =>
        {
            var parts = line.Split(',');
            return new JuncBox(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        })];
        List<(JuncBox, JuncBox, double)> distances = [];

        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                var box1 = boxes[i];
                var box2 = boxes[j];
                distances.Add((box1, box2, box1.DistanceTo(box2)));
            }
        }
        distances.Sort((a, b) => a.Item3.CompareTo(b.Item3));

        Dictionary<JuncBox, List<JuncBox>> circuits = [];
        for (int i = 0; i < Math.Min(connections, distances.Count); i++)
        {
            var newCircuit = distances[i];
            var circuit1 = circuits.GetValueOrDefault(newCircuit.Item1);
            var circuit2 = circuits.GetValueOrDefault(newCircuit.Item2);
            if (circuit1 == null && circuit2 == null)
            {
                var newCircuitList = new List<JuncBox> { newCircuit.Item1, newCircuit.Item2 };
                circuits[newCircuit.Item1] = newCircuitList;
                circuits[newCircuit.Item2] = newCircuitList;
            }
            else if (circuit1 != null && circuit2 == null)
            {
                circuit1.Add(newCircuit.Item2);
                circuits[newCircuit.Item2] = circuit1;
            }
            else if (circuit1 == null && circuit2 != null)
            {
                circuit2.Add(newCircuit.Item1);
                circuits[newCircuit.Item1] = circuit2;
            }
            else if (circuit1 != circuit2)
            {
                circuit1.AddRange(circuit2);
                foreach (var box in circuit2)
                {
                    circuits[box] = circuit1;
                }
            }
        }
        var circuitSets = new HashSet<List<JuncBox>>(circuits.Values).OrderByDescending(x => x.Count);
        answer = circuitSets.Take(3).Select(x => x.Count).Aggregate(1, (a, b) => a * b);
        return answer;
    }

    public override async Task<long> Puzzle2(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");



        return answer;
    }

    private record JuncBox(int X, int Y, int Z)
    {
        public double DistanceTo(JuncBox other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }
    }
}

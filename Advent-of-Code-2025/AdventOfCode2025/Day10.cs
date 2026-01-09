using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        foreach (var input in inputs)
        {
            var machine = input.Split(' ');

            int[] target = machine[machine.Length - 1]
                .Replace("{", "")
                .Replace("}", "")
                .Split(',')
                .Select(x => int.Parse(x))
                .ToArray();

            List<int[]> buttonSets = machine[1..(machine.Length - 1)]
                .Select(b => b.Replace("(", "").Replace(")", "").Split(','))
                .Select(x => x.Select(b => int.Parse(b)).ToArray()).ToList();

            Solver solver = Solver.CreateSolver("SCIP");
            if (solver is null)
            {
                return -1;
            }

            var variables = buttonSets
                .Select((x, i) => solver.MakeIntVar(0.0, double.PositiveInfinity, $"x{i}"))
                .ToList();

            for (int i = 0; i < target.Length; i++)
            {
                var constraint = solver.MakeConstraint(target[i], target[i]);
                for (int n = 0; n < buttonSets.Count; n++)
                {
                    if (buttonSets[n].Contains(i))
                    {
                        constraint.SetCoefficient(variables[n], 1.0);
                    }
                    else
                    {
                        constraint.SetCoefficient(variables[n], 0.0);
                    }
                }
                
            }

            var objective = solver.Objective();
            objective.SetMinimization();
            foreach (var variable in variables)
            {
                objective.SetCoefficient(variable, 1.0);
            }


            solver.Solve();

            answer += (long)solver.Objective().Value();
            
        }



        return answer;
    }

    private class LongArrayComparer : IEqualityComparer<short[]>
    {
        public bool Equals(short[]? x, short[]? y)
        {
            if (x == null || y == null) return false;
            if (x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public int GetHashCode([DisallowNull] short[] obj)
        {
            var result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i].GetHashCode();
                }
            }
            return result;
        }
    }
}
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
internal class Day24 : BaseDay<string>
{

    public override string Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n\r\n");
        var variables = input[0].Split("\r\n")
            .Select(x => x.Split(": "))
            .ToDictionary(x => x[0], x => x[1] == "1");
        var operations = input[1].Split("\r\n")
            .Select(x => x.Split(" "))
            .Select(x => new Operation(x[0], x[2], GetGate(x[1]), x[4]))
            .ToArray();

        var queue = new Queue<Operation>(operations);
        while (queue.Count > 0)
        {
            var operation = queue.Dequeue();
            if (!ApplyOperation(variables, operation))
            {
                queue.Enqueue(operation);
            }
        }

        var results = variables.Where(x => x.Key.StartsWith('z')).OrderByDescending(x => x.Key).Select(x => x.Value);

        foreach (var variable in results)
        {
            answer <<= 1;
            if (variable)
            {
                answer += 1;
            }
        }
        
        return $"{answer}";
    }

    public override string Puzzle2()
    {
        var input = Input.Split("\r\n\r\n");
        var variables = input[0].Split("\r\n")
            .Select(x => x.Split(": "))
            .ToDictionary(x => x[0], x => x[1] == "1");
        var operations = input[1].Split("\r\n")
            .Order()
            .Select(x => x.Split(" "))
            .Select(x => new Operation(x[0], x[2], GetGate(x[1]), x[4]))
            .OrderByDescending(x => x.Operand1.StartsWith('x') || x.Operand1.StartsWith('y') || x.Operand2.StartsWith('x') || x.Operand2.StartsWith('y'))
            .ThenBy(x => x.Operand1[1..])
            .ThenBy(x => x.Operand2[1..])
            .ThenByDescending(x => x.Gate)
            .ToArray();

        //var queue = new Queue<Operation>(operations);

        List<string> errors = new();
        var ci = operations.First(o => ((o.Operand1 == "x00" && o.Operand2 == "y00") || (o.Operand1 == "y00" && o.Operand2 == "x00")) && o.Gate == Gate.AND).Result;
        for (int i = 1; i < 45; i++)
        {
            var x = "x" + i.ToString("D2");
            var y = "y" + i.ToString("D2");

            var st = operations.First(o => ((o.Operand1 == x && o.Operand2 == y) || (o.Operand1 == y && o.Operand2 == x)) && o.Gate == Gate.XOR).Result;
            var ct = operations.First(o => ((o.Operand1 == x && o.Operand2 == y) || (o.Operand1 == y && o.Operand2 == x)) && o.Gate == Gate.AND).Result;

            var si = operations.FirstOrDefault(o => ((o.Operand1 == ci && o.Operand2 == st) || (o.Operand1 == st && o.Operand2 == ci)) && o.Gate == Gate.XOR)?.Result;
            if (si == null)
            {
                var si2 = si = operations.FirstOrDefault(o => ((o.Operand1 == ci && o.Operand2 == ct) || (o.Operand1 == ct && o.Operand2 == ci)) && o.Gate == Gate.XOR)?.Result;
                if (si2 == null)
                {
                    break;
                }
                errors.Add(st);
                errors.Add(ct);
                si = si2;
                (st, ct) = (ct, st);
            }
            else if (!si.StartsWith('z') && ct.StartsWith('z'))
            {
                errors.Add(si);
                errors.Add(ct);
                ct = si;
            }
            var cti = operations.First(o => ((o.Operand1 == ci && o.Operand2 == st) || (o.Operand1 == st && o.Operand2 == ci)) && o.Gate == Gate.AND).Result;
            if (!si.StartsWith('z') && cti.StartsWith('z'))
            {
                errors.Add(si);
                errors.Add(cti);
                cti = si;
            }
            ci = operations.First(o => ((o.Operand1 == cti && o.Operand2 == ct) || (o.Operand1 == ct && o.Operand2 == cti)) && o.Gate == Gate.OR).Result;
            if (!si.StartsWith('z') && ci.StartsWith('z'))
            {
                errors.Add(si);
                errors.Add(ci);
                ci = si;
            }
        }

        return string.Join(",", errors.Order());
    }

    private bool ApplyOperation(Dictionary<string, bool> variables, Operation operation)
    {
        bool? operand1 = GetOperandValue(variables, operation.Operand1);
        if (operand1 == null)
        {
            return false;
        }
        bool? operand2 = GetOperandValue(variables, operation.Operand2);
        if (operand2 == null)
        {
            return false;
        }

        bool result = operation.Gate switch
        {
            Gate.AND => operand1.Value && operand2.Value,
            Gate.OR => operand1.Value || operand2.Value,
            Gate.XOR => operand1.Value ^ operand2.Value,
            _ => throw new Exception("Invalid gate")
        };
        variables[operation.Result] = result;
        return true;
    }

    private bool? GetOperandValue(Dictionary<string, bool> variables, string operand)
    {
        if (variables.TryGetValue(operand, out bool value))
        {
            return value;
        }
        return null;
    }

    private Gate GetGate(string gate)
    {
        return gate switch
        {
            "AND" => Gate.AND,
            "OR" => Gate.OR,
            "XOR" => Gate.XOR,
            _ => throw new Exception("Invalid gate")
        };
    }

    private enum Gate
    {
        AND,
        OR,
        XOR,
    }
    private record Operation(string Operand1, string Operand2, Gate Gate, string Result);

}
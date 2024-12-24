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
internal class Day24 : BaseDay<long>
{

    public override long Puzzle1()
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
        
        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n");


        return answer;
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
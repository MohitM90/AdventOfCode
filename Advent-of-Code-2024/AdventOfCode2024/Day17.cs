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
internal class Day17 : BaseDay<string>
{

    public override string Puzzle1()
    {
        string answer = "";
        var input = Regex.Match(Input,
            "Register A: (?<a>\\d+)\r\n" +
            "Register B: (?<b>\\d+)\r\n" +
            "Register C: (?<c>\\d+)\r\n\r\n" +
            "Program: (?<ins>.+)");
        var app = new Application(
            long.Parse(input.Groups["a"].Value),
            long.Parse(input.Groups["b"].Value),
            long.Parse(input.Groups["c"].Value),
            input.Groups["ins"].Value.Split(",").Select(byte.Parse).ToArray());

        answer = string.Join(",", app.Run());

        return answer;
    }

    public override string Puzzle2()
    {
        string answer = "";
        var input = Regex.Match(Input,
            "Register A: (?<a>\\d+)\r\n" +
            "Register B: (?<b>\\d+)\r\n" +
            "Register C: (?<c>\\d+)\r\n\r\n" +
            "Program: (?<ins>.+)");
        var app = new Application(
            long.Parse(input.Groups["a"].Value),
            long.Parse(input.Groups["b"].Value),
            long.Parse(input.Groups["c"].Value),
            input.Groups["ins"].Value.Split(",").Select(byte.Parse).ToArray());

        var program = app.GetProgram().Select(x => (long)x).ToArray();

        var queue = new Queue<long>();
        queue.Enqueue(0);

        long[] output = [];
        while (true)
        {
            if (output.Length == program.Length && output.SequenceEqual(program))
            {
                answer = $"{queue.Dequeue()}";
                break;
            }

            var a = queue.Dequeue();
            a <<= 3;
            for (int i = 0; i < 8; i++)
            {
                app.RegisterA = a + i;
                app.RegisterB = 0;
                app.RegisterC = 0;
                output = app.Run();
                if (program.TakeLast(output.Length).SequenceEqual(output))
                {
                    queue.Enqueue(a + i);
                }
            }
        }

        return answer;
    }

    private record Application
    {
        public long RegisterA { get; set; }
        public long RegisterB { get; set; }
        public long RegisterC { get; set; }
        public int InstructionPointer { get; set; } = 0;

        private Instruction[] Instructions { get; set; }
        private List<long> Output { get; set; } = new();

        public Application(long registerA, long registerB, long registerC, byte[] instructions)
        {
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;

            Instructions = instructions.Chunk(2).Select(i => new Instruction((Opcode)i[0], i[1])).ToArray();
        }

        public long[] Run()
        {
            Output.Clear();
            InstructionPointer = 0;
            while (InstructionPointer < Instructions.Length)
            {
                ExecuteInstruction(Instructions[InstructionPointer]);
            }
            return [.. Output];
        }

        public byte[] GetProgram()
        {
            return Instructions.SelectMany(i => new byte[] { (byte)i.Opcode, i.Operand }).ToArray();
        }

        private long GetComboOperandValue(byte operand)
        {
            return operand switch
            {
                >= 0 and <= 3 => operand,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new InvalidOperationException()
            };
        }

        private void ExecuteInstruction(Instruction instruction)
        {
            switch (instruction.Opcode)
            {
                case Opcode.Adv:
                    Adv(instruction.Operand);
                    break;
                case Opcode.Bxl:
                    Bxl(instruction.Operand);
                    break;
                case Opcode.Bst:
                    Bst(instruction.Operand);
                    break;
                case Opcode.Jnz:
                    Jnz(instruction.Operand);
                    break;
                case Opcode.Bxc:
                    Bxc(instruction.Operand);
                    break;
                case Opcode.Out:
                    Output.Add(Out(instruction.Operand));
                    break;
                case Opcode.Bdv:
                    Bdv(instruction.Operand);
                    break;
                case Opcode.Cdv:
                    Cdv(instruction.Operand);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void Adv(byte comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, GetComboOperandValue(comboOperand));
            RegisterA = (long)(numerator / denominator);
            InstructionPointer++;
        }

        private void Bxl(byte literalOperand)
        {
            RegisterB = RegisterB ^ literalOperand;
            InstructionPointer++;
        }

        private void Bst(byte comboOperand)
        {
            RegisterB = Modulo(GetComboOperandValue(comboOperand), 8);
            InstructionPointer++;
        }

        private void Jnz(byte literalOperand)
        {
            if (RegisterA != 0)
            {
                InstructionPointer = literalOperand;
                return;
            }
            InstructionPointer++;
        }

        private void Bxc(byte operand)
        {
            RegisterB = RegisterB ^ RegisterC;
            InstructionPointer++;
        }

        private long Out(byte comboOperand)
        {
            var value = Modulo(GetComboOperandValue(comboOperand), 8);
            InstructionPointer++;
            return value;
        }

        private void Bdv(byte comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, GetComboOperandValue(comboOperand));
            RegisterB = (long)(numerator / denominator);
            InstructionPointer++;
        }

        private void Cdv(byte comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, GetComboOperandValue(comboOperand));
            RegisterC = (long)(numerator / denominator);
            InstructionPointer++;
        }

        private long Modulo(long a, long b)
        {
            return (Math.Abs(a * b) + a) % b;
        }

        private record Instruction(Opcode Opcode, byte Operand)
        {


        };

        private enum Opcode
        {
            Adv = 0,
            Bxl = 1,
            Bst = 2,
            Jnz = 3,
            Bxc = 4,
            Out = 5,
            Bdv = 6,
            Cdv = 7
        }
    }
}
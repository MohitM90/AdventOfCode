using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;

internal class Day13 : BaseDay
{

    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n\r\n")
            .Select(x => Regex.Match(x, @"Button A: X\+(?<Ax>\d+), Y\+(?<Ay>\d+)\r\n" +
                                        @"Button B: X\+(?<Bx>\d+), Y\+(?<By>\d+)\r\n" +
                                        @"Prize: X=(?<Px>\d+), Y=(?<Py>\d+)"))
            .Select(x => new ClawMachine
            (
                new Button(
                    int.Parse(x.Groups["Ax"].Value),
                    int.Parse(x.Groups["Ay"].Value),
                    3),
                new Button(
                    int.Parse(x.Groups["Bx"].Value),
                    int.Parse(x.Groups["By"].Value),
                    1),
                new Point(
                    int.Parse(x.Groups["Px"].Value),
                    int.Parse(x.Groups["Py"].Value))
            ).Win())
            .Where(w => w.X != -1 && w.Y != -1)
            .ToArray();

        answer = inputs.Sum(w => 3 * w.X + w.Y);


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n\r\n")
            .Select(x => Regex.Match(x, @"Button A: X\+(?<Ax>\d+), Y\+(?<Ay>\d+)\r\n" +
                                        @"Button B: X\+(?<Bx>\d+), Y\+(?<By>\d+)\r\n" +
                                        @"Prize: X=(?<Px>\d+), Y=(?<Py>\d+)"))
            .Select(x => new ClawMachine
            (
                new Button(
                    long.Parse(x.Groups["Ax"].Value),
                    long.Parse(x.Groups["Ay"].Value),
                    3),
                new Button(
                    long.Parse(x.Groups["Bx"].Value),
                    long.Parse(x.Groups["By"].Value),
                    1),
                new Point(
                    long.Parse(x.Groups["Px"].Value) + 10000000000000,
                    long.Parse(x.Groups["Py"].Value) + 10000000000000)
            ).Win(limit: long.MaxValue))
            .Where(w => w.X != -1 && w.Y != -1)
            .ToArray();

        answer = inputs.Sum(w => 3 * w.X + w.Y);


        return answer;
    }

    private record ClawMachine
    {
        public Button ButtonA { get; init; }
        public Button ButtonB { get; init; }
        public Point Prize { get; init; }

        public ClawMachine(Button buttonA, Button buttonB, Point prize)
        {
            ButtonA = buttonA;
            ButtonB = buttonB;
            Prize = prize;
        }

        public Point Win(long limit = 100)
        {
            decimal a = (Math.Abs(ButtonB.X * Prize.Y - ButtonB.Y * Prize.X) * 1.0m) / (Math.Abs(ButtonB.X * ButtonA.Y - ButtonB.Y * ButtonA.X) * 1.0m);
            decimal b = ((Prize.X - a * ButtonA.X) * 1.0m) / (ButtonB.X * 1.0m);
            if (a == (long)a && b == (long)b && a <= limit && b <= limit)
            {
                return new Point((long)a, (long)b);
            }
            return new Point(-1, -1);
        }
    }

    public record Button : Point
    {
        public long Tokens { get; set; }

        public Button(long x, long y, long tokens) : base(x, y)
        {
            Tokens = tokens;
        }
    }

    public record class Point
    {
        public long X { get; set; }
        public long Y { get; set; }
        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }
    }
}
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
        var inputs = Input.Split("\r\n");


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

        public Point Win()
        {
            double a = (ButtonB.X * Prize.Y - ButtonB.Y * Prize.X) * 1.0 / (ButtonB.X * ButtonA.Y - ButtonB.Y * ButtonA.X);
            double b = (Prize.X - a * ButtonA.X) * 1.0 / ButtonB.X;
            if (a == (int)a && b == (int)b && a <= 100 && b <= 100)
            {
                return new Point((int)a, (int)b);
            }
            return new Point(-1, -1);
        }
    }

    public record Button : Point
    {
        public int Tokens { get; set; }

        public Button(int x, int y, int tokens) : base(x, y)
        {
            Tokens = tokens;
        }
    }

    public record class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
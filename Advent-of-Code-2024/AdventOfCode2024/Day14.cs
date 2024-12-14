using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day14 : BaseDay
{

    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n")
            .Select(x => Regex.Match(x, @"p=(?<px>\d+),(?<py>\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)"))
            .Select(x => new Robot(
                new Point(
                    int.Parse(x.Groups["px"].Value), 
                    int.Parse(x.Groups["py"].Value)), 
                new Point(
                    int.Parse(x.Groups["vx"].Value), 
                    int.Parse(x.Groups["vy"].Value))))
            .ToArray();

        Point mapDimension = new(101, 103);
        int seconds = 100;

        MoveRobots(inputs, mapDimension, seconds);

        var quadrant1Robots = inputs.Where(x => 
                x.Position.X >= 0 && 
                x.Position.Y >= 0 &&
                x.Position.X < mapDimension.X / 2 && 
                x.Position.Y < mapDimension.Y / 2)
            .LongCount();

        var quadrant2Robots = inputs.Where(x =>
                x.Position.X >= (mapDimension.X / 2) + 1 &&
                x.Position.Y >= 0 &&
                x.Position.X < mapDimension.X &&
                x.Position.Y < mapDimension.Y / 2)
            .LongCount();

        var quadrant3Robots = inputs.Where(x =>
                x.Position.X >= 0 &&
                x.Position.Y >= (mapDimension.Y / 2) + 1 &&
                x.Position.X < mapDimension.X / 2 &&
                x.Position.Y < mapDimension.Y)
            .LongCount();

        var quadrant4Robots = inputs.Where(x =>
                x.Position.X >= (mapDimension.X / 2) + 1 &&
                x.Position.Y >= (mapDimension.Y / 2) + 1 &&
                x.Position.X < mapDimension.X &&
                x.Position.Y < mapDimension.Y)
            .LongCount();

        answer = quadrant1Robots * quadrant2Robots * quadrant3Robots * quadrant4Robots;


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n")
            .Select(x => Regex.Match(x, @"p=(?<px>\d+),(?<py>\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)"))
            .Select(x => new Robot(
                new Point(
                    int.Parse(x.Groups["px"].Value),
                    int.Parse(x.Groups["py"].Value)),
                new Point(
                    int.Parse(x.Groups["vx"].Value),
                    int.Parse(x.Groups["vy"].Value))))
            .ToArray();

        Point mapDimension = new(101, 103);

        long count = 0;
        while (true)
        {
            MoveRobots(inputs, mapDimension, 1);
            count++;

            var quadrant1Robots = inputs.Where(x =>
                    x.Position.X >= 0 &&
                    x.Position.Y >= 0 &&
                    x.Position.X < mapDimension.X / 2 &&
                    x.Position.Y < mapDimension.Y / 2)
                .LongCount();

            var quadrant2Robots = inputs.Where(x =>
                    x.Position.X >= (mapDimension.X / 2) + 1 &&
                    x.Position.Y >= 0 &&
                    x.Position.X < mapDimension.X &&
                    x.Position.Y < mapDimension.Y / 2)
                .LongCount();

            var quadrant3Robots = inputs.Where(x =>
                    x.Position.X >= 0 &&
                    x.Position.Y >= (mapDimension.Y / 2) + 1 &&
                    x.Position.X < mapDimension.X / 2 &&
                    x.Position.Y < mapDimension.Y)
                .LongCount();

            var quadrant4Robots = inputs.Where(x =>
                    x.Position.X >= (mapDimension.X / 2) + 1 &&
                    x.Position.Y >= (mapDimension.Y / 2) + 1 &&
                    x.Position.X < mapDimension.X &&
                    x.Position.Y < mapDimension.Y)
                .LongCount();

            if (quadrant1Robots > 250 || quadrant2Robots > 250 || quadrant3Robots > 250 || quadrant4Robots > 250)
            {
                PrintRobots(inputs, mapDimension);
                break;
            }
        }

        answer = count;

        return answer;
    }

    private void MoveRobots(Robot[] robots, Point mapDimension, long seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            foreach (var robot in robots)
            {
                MoveRobot(robot, mapDimension);
            }
        }
    }

    private void MoveRobot(Robot robot, Point mapDimension)
    {
        robot.Position.X = Modulo(robot.Position.X + robot.Velocity.X, mapDimension.X);
        robot.Position.Y = Modulo(robot.Position.Y + robot.Velocity.Y, mapDimension.Y);
    }

    private void PrintRobots(Robot[] robots, Point mapDimension)
    {
        string output = "";
        for (int y = 0; y < mapDimension.Y; y++)
        {
            for (int x = 0; x < mapDimension.X; x++)
            {
                var sum = robots.Where(r => r.Position.X == x && r.Position.Y == y).Count();
                if (sum > 0)
                {
                    output += "🤖";
                }
                else
                {
                    output += "⚫";
                }
            }
            output += "\n";
        }
        Console.WriteLine(output);
    }

    int Modulo(int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }

    private record Robot 
    {
        public Point Position { get; set; }
        public Point Velocity { get; set; }
        public Robot(Point position, Point velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }

    private record class Point
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
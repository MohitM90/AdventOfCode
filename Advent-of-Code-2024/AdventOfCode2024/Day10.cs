using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day10 : BaseDay<long>
{
    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();

        answer = CalculateScore(inputs);

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();

        answer = CalculateScore(inputs, false);

        return answer;
    }

    private long CalculateScore(char[][] inputs, bool distinctRoutes = true)
    {
        var trailHeads = inputs.SelectMany((row, y) => row.Select((c, x) => (c, new Point(x, y))))
            .Where(t => t.c == '0');

        long score = 0;

        foreach (var trailHead in trailHeads)
        {
            var points = new Stack<(char c, Point p)>();
            List<Point> trailEnds = [];
            points.Push(trailHead);
            while (points.Count > 0)
            {
                var point = points.Pop();

                if (point.c == '9')
                {
                    if (!distinctRoutes || !trailEnds.Contains(point.p))
                    {
                        trailEnds.Add(point.p);
                    }
                    continue;
                }

                //Left
                if (point.p.X > 0 && inputs[point.p.Y][point.p.X - 1] == point.c + 1)
                {
                    points.Push((inputs[point.p.Y][point.p.X - 1], new Point(point.p.X - 1, point.p.Y)));
                }

                //Right
                if (point.p.X < inputs[0].Length - 1 && inputs[point.p.Y][point.p.X + 1] == point.c + 1)
                {
                    points.Push((inputs[point.p.Y][point.p.X + 1], new Point(point.p.X + 1, point.p.Y)));
                }

                //Up
                if (point.p.Y > 0 && inputs[point.p.Y - 1][point.p.X] == point.c + 1)
                {
                    points.Push((inputs[point.p.Y - 1][point.p.X], new Point(point.p.X, point.p.Y - 1)));
                }

                //Down
                if (point.p.Y < inputs.Length - 1 && inputs[point.p.Y + 1][point.p.X] == point.c + 1)
                {
                    points.Push((inputs[point.p.Y + 1][point.p.X], new Point(point.p.X, point.p.Y + 1)));
                }
            }

            score += trailEnds.Count;
        }
        return score;
    }

    private record Point(int X, int Y);
}
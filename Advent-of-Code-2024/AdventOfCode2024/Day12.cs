using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day12 : BaseDay
{

    public override long Puzzle1()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n")
            .Select(s => s.ToCharArray()
                .Select(c => new Flower(c))
                .ToArray())
            .ToArray();

        answer = CalculateTotalFencingPrice(inputs);


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var inputs = Input.Split("\r\n")
            .Select(s => s.ToCharArray()
                .Select(c => new Flower(c))
                .ToArray())
            .ToArray();

        answer = CalculateTotalFencingPrice(inputs, true);

        return answer;
    }

    private long CalculateTotalFencingPrice(Flower[][] inputs, bool bulkDiscount = false)
    {
        long price = 0;
        for (int y = 0; y < inputs.Length; y++)
        {
            for (int x = 0; x < inputs[y].Length; x++)
            {
                if (inputs[y][x].Visited)
                {
                    continue;
                }
                price += CalculateFencingPriceForRegion(inputs, x, y, bulkDiscount);
            }
        }
        return price;
    }

    private long CalculateFencingPriceForRegion(Flower[][] inputs, int x, int y, bool bulkDiscount = false)
    {
        char type = inputs[y][x].Type;
        long perimeter = 0;
        long area = 0;
        long sides = 0;
        var points = new Queue<(int x, int y)>();
        points.Enqueue((x, y));
        while (points.Count > 0)
        {
            var point = points.Dequeue();

            if (inputs[point.y][point.x].Visited)
            {
                continue;
            }
            inputs[point.y][point.x].Visited = true;

            foreach (var direction in directions)
            {
                var newX = point.x + direction.dx;
                var newY = point.y + direction.dy;
                
                if (newX < 0 || newX >= inputs[0].Length || newY < 0 || newY >= inputs.Length
                    || inputs[newY][newX].Type != type)
                {
                    perimeter++;

                    if (IsNewEdge(inputs, type, direction.direction, point.x, point.y))
                    {
                        sides++;
                    }
                    inputs[point.y][point.x].Edges |= direction.direction;

                    continue;
                }
                if (inputs[newY][newX].Visited)
                {
                    continue;
                }
                points.Enqueue((newX, newY));
            }

            area++;
        }

        if (bulkDiscount)
        {
            return sides * area;
        }
        return perimeter * area;
    }

    private bool IsNewEdge(Flower[][] inputs, char type, Direction direction, int x, int y)
    {
        return direction switch
        {
            Direction.Up or Direction.Down => (x - 1 < 0 || inputs[y][x - 1].Type != type || !inputs[y][x - 1].Edges.HasFlag(direction)) && (x + 1 >= inputs[0].Length || inputs[y][x + 1].Type != type || !inputs[y][x + 1].Edges.HasFlag(direction)),
            Direction.Left or Direction.Right => (y - 1 < 0 || inputs[y - 1][x].Type != type || !inputs[y - 1][x].Edges.HasFlag(direction)) && (y + 1 >= inputs.Length || inputs[y + 1][x].Type != type || !inputs[y + 1][x].Edges.HasFlag(direction)),
            _ => false
        };
    }

    private readonly (Direction direction, int dx, int dy)[] directions = [
        (Direction.Up, 0, -1),
        (Direction.Down, 0, 1),
        (Direction.Left, -1, 0),
        (Direction.Right, 1, 0)
    ];

    private record Flower(char type, bool visited = false)
    {
        public char Type { get; init; } = type;
        public bool Visited { get; set; } = visited;
        public Direction Edges { get; set; } = Direction.None;
    }

    [Flags]
    private enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }
}
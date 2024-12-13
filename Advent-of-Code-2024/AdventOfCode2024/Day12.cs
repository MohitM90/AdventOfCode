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
        var inputs = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();

        return answer;
    }

    private long CalculateTotalFencingPrice(Flower[][] inputs)
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
                price += CalculateFencingPriceForRegion(inputs, x, y);
            }
        }
        return price;
    }

    private long CalculateFencingPriceForRegion(Flower[][] inputs, int x, int y)
    {
        char type = inputs[y][x].Type;
        long perimeter = 0;
        long area = 0;
        var points = new Stack<(int x, int y)>();
        points.Push((x, y));
        while (points.Count > 0)
        {
            var point = points.Pop();

            if (point.x < 0 || point.x >= inputs[0].Length || point.y < 0 || point.y >= inputs.Length 
                || inputs[point.y][point.x].Type != type)
            {
                perimeter++;
                continue;
            }

            if (inputs[point.y][point.x].Visited)
            {
                continue;
            }

            inputs[point.y][point.x].Visited = true;


            area++;

            points.Push((point.x + 1, point.y));
            points.Push((point.x - 1, point.y));
            points.Push((point.x, point.y + 1));
            points.Push((point.x, point.y - 1));
        }

        return perimeter * area;
    }

    private record Flower(char type, bool visited = false)
    {
        public char Type { get; init; } = type;
        public bool Visited { get; set; } = visited;
    }
}
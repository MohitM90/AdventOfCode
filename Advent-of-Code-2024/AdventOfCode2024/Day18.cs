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
internal class Day18 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var map = Input.Split("\r\n");


        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var map = Input.Split("\r\n");


        return answer;
    }




    private record Position(int X, int Y);
    private record Tile(Position Position, long Distance, char Direction, Tile? Parent)
    {
        public long GoalDistance { get; set; }

    }

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);
}
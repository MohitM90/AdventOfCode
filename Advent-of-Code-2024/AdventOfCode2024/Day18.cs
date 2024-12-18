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
        var input = Input.Split("\r\n").Select(x => x.Split(",").Select(y => int.Parse(y)).ToArray());
        int[,] map = new int[71,71];
        foreach (var line in input.Take(1024))
        {
            map[line[0], line[1]] = int.MaxValue;
        }

        

        var start = new Tile(new Position(0, 0), 0, null);
        var end = new Position(70, 70);
        var endTile = FindEndTilePathScore(map, start, end);
        answer = -1;
        while (endTile != null)
        {
            answer++;
            endTile = endTile.Parent;
        }

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var map = Input.Split("\r\n");


        return answer;
    }


    private Tile FindEndTilePathScore(int[,] map, Tile start, Position end)
    {
        var startNode = start;
        var endPosition = end;
        startNode.GoalDistance = GetHeuristic(startNode.Position, endPosition);

        List<Tile> closedTiles = [];
        List<Tile> openTiles = [startNode];

        var result = new Tile(new Position(-1, -1), long.MaxValue, null);
        while (openTiles.Count > 0)
        {
            var currentTile = openTiles.MinBy(x => x.GoalDistance)!;
            openTiles.Remove(currentTile);
            if (currentTile.Position == endPosition)
            {
                if (currentTile.Distance < result.Distance)
                {
                    result = currentTile;
                }
            }
            closedTiles.Add(currentTile);
            var children = GetChildren(map, currentTile).Where(x => !closedTiles.Contains(x));
            foreach (var child in children)
            {
                var closedChild = closedTiles.FirstOrDefault(c => child.Position == c.Position);
                if (closedChild != null && child.Distance < closedChild.Distance)
                {
                    closedTiles.Remove(closedChild);
                    closedChild = null;
                }
                var openChild = openTiles.FirstOrDefault(c => child.Position == c.Position);
                if (openChild != null && child.Distance < openChild.Distance)
                {
                    openTiles.Remove(openChild);
                    openChild = null;
                }

                if (closedChild == null && openChild == null)
                {
                    child.GoalDistance = child.Distance + GetHeuristic(child.Position, endPosition);
                    openTiles.Add(child);
                }
            }
        }

        return result;
    }

    private long GetHeuristic(Position current, Position end)
    {
        return (Math.Abs(current.X - end.X) + Math.Abs(current.Y - end.Y));
    }

    private List<Tile> GetChildren(int[,] map, Tile currentTile)
    {
        List<Tile> children = [];
        foreach (var direction in new[] { UP, DOWN, LEFT, RIGHT })
        {
            var newPosition = new Position(currentTile.Position.X + direction.X, currentTile.Position.Y + direction.Y);
            if (newPosition.X < 0 || newPosition.X > map.GetUpperBound(0) || newPosition.Y < 0 || newPosition.Y > map.GetUpperBound(1))
            {
                continue;
            }
            if (map[newPosition.X, newPosition.Y] == int.MaxValue)
            {
                continue;
            }

            var newTile = new Tile(newPosition, currentTile.Distance + 1, currentTile);
            children.Add(newTile);
        }
        return children;
    }

    private record Position(int X, int Y);
    private record Tile(Position Position, long Distance, Tile? Parent)
    {
        public long GoalDistance { get; set; }

    }

    private static readonly Position UP = new(0, -1);
    private static readonly Position DOWN = new(0, 1);
    private static readonly Position LEFT = new(-1, 0);
    private static readonly Position RIGHT = new(1, 0);
}
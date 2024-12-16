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
internal class Day16 : BaseDay
{

    public override long Puzzle1()
    {
        long answer = 0;
        var map = Input.Split("\r\n").Select(s => s.ToCharArray()).ToArray();

        Tile t = FindEndTilePathScore(map);

        answer = t.Distance;

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var map = Input.Split("\r\n").Select(s => s.ToCharArray());

        return answer;
    }

    private Tile FindEndTilePathScore(char[][] map)
    {
        var startNode = FindStartTile(map);
        var endPosition = FindEndPosition(map);
        startNode.GoalDistance = GetHeuristic(startNode.Position, endPosition);

        List<Tile> closedTiles = [];
        List<Tile> openTiles = new();
        openTiles.Add(startNode);

        var result = new Tile(new Position(-1, -1), long.MaxValue, ' ', null);
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
                var closedChild = closedTiles.FirstOrDefault(c => child.Position == c.Position && child.Direction == c.Direction);
                if (closedChild != null && child.Distance < closedChild.Distance)
                {
                    closedTiles.Remove(closedChild);
                    closedChild = null;
                }
                var openChild = openTiles.FirstOrDefault(c => child.Position == c.Position && child.Direction == c.Direction);
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

    private List<Tile> GetChildren(char[][] map, Tile currentTile)
    {
        List<Tile> children = [];
        foreach (var direction in new[] { UP, DOWN, LEFT, RIGHT })
        {
            var newPosition = new Position(currentTile.Position.X + direction.X, currentTile.Position.Y + direction.Y);
            if (newPosition.X < 0 || newPosition.X >= map[0].Length || newPosition.Y < 0 || newPosition.Y >= map.Length)
            {
                continue;
            }
            if (map[newPosition.Y][newPosition.X] == '#')
            {
                continue;
            }

            var moveDirection = GetMoveDirection(direction);
            var distance = GetDistance(currentTile, moveDirection);
            var newTile = new Tile(newPosition, currentTile.Distance + distance, moveDirection, currentTile);
            children.Add(newTile);
        }
        return children;
    }

    private long GetDistance(Tile currentTile, char moveDirection)
    {
        if (currentTile.Direction == moveDirection)
        {
            return 1;
        }

        return currentTile.Direction switch
        {
            '^' or 'v' => moveDirection is '<' or '>' ? 1001 : 2001,
            '<' or '>' => moveDirection is '^' or 'v' ? 1001 : 2001,
            _ => (long)0,
        };
    }

    private long GetHeuristic(Position current, Position end)
    {
        //return 1;
        return (Math.Abs(current.X - end.X) + Math.Abs(current.Y - end.Y));
    }

    private void PrinMap(char[][] map, int delay)
    {
        Console.SetCursorPosition(0, 0);
        string output = "";
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                output += map[y][x];
            }
            output += "\n";
        }
        Console.WriteLine(output);
        Thread.Sleep(delay);
    }

    private Tile FindStartTile(char[][] map)
    {
        return map
            .Select((row, y) => (row, Node: new Tile(new(Array.IndexOf(row, 'S'), y), 0, '>', null)))
            .Where(t => t.Node.Position.X >= 0)
            .Select(t => t.Node)
            .First();
    }

    private Position FindEndPosition(char[][] map)
    {
        return map
            .Select((row, y) => (row, Position: new Position(Array.IndexOf(row, 'E'), y)))
            .Where(t => t.Position.X >= 0)
            .Select(t => t.Position)
            .First();
    }


    private char GetMoveDirection(Position pos)
    {
        if (pos == UP)
        {
            return '^';
        }
        if (pos == DOWN)
        {
            return 'v';
        }
        if (pos == LEFT)
        {
            return '<';
        }
        if (pos == RIGHT)
        {
            return '>';
        }
        return ' ';
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
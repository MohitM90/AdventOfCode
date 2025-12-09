using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2025;

internal class Day09 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        var tiles = inputs.Select(x => x.Split(',')).Select(x => new Tile(int.Parse(x[0]), int.Parse(x[1]))).ToList();
        long max = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                var tile1 = tiles[i];
                var tile2 = tiles[j];
                var area = (Math.Abs(tile1.X - tile2.X) + 1) * (Math.Abs(tile1.Y - tile2.Y) + 1);
                if (area >= max)
                {
                    max = area;
                }
            }
        }
        answer = max;

        return answer;
    }

    public override async Task<long> Puzzle2(string data)
    {
        long answer = 0;
        var inputs = data.Split("\r\n");
        var redTiles = inputs.Select(x => x.Split(',')).Select(x => new Tile(int.Parse(x[0]), int.Parse(x[1]))).ToList();
        var minX = redTiles.Min(x => x.X);
        var maxX = redTiles.Max(x => x.X) - minX;
        var minY = redTiles.Min(x => x.Y);
        var maxY = redTiles.Max(x => x.Y) - minY;
        char[,] grid = new char[maxX + 1, maxY + 1];
        for (long y = 0; y <= maxY; y++)
        {
            for (long x = 0; x <= maxX; x++)
            {
                grid[x, y] = '.';
            }
        }
        var sortedByX = redTiles.OrderBy(x => x.X).ToList();
        for (int i = 0; i < sortedByX.Count; i++)
        {
            for (int j = i; j < sortedByX.Count; j++)
            {
                var tile1 = sortedByX[i];
                var tile2 = sortedByX[j];
                grid[tile1.X - minX, tile1.Y - minY] = '#';
                grid[tile2.X - minX, tile2.Y - minY] = '#';
                if (tile1.Y != tile2.Y)
                {
                    continue;
                }
                for (long x = tile1.X + 1; x < tile2.X; x++)
                {
                    grid[x - minX, tile1.Y - minY] = 'X';
                }
            }
        }
        var sortedByY = redTiles.OrderBy(x => x.Y).ToList();
        for (int i = 0; i < sortedByY.Count; i++)
        {
            for (int j = i; j < sortedByY.Count; j++)
            {
                var tile1 = sortedByY[i];
                var tile2 = sortedByY[j];
                grid[tile1.X - minX, tile1.Y - minY] = '#';
                grid[tile2.X - minX, tile2.Y - minY] = '#';
                if (tile1.X != tile2.X)
                {
                    continue;
                }
                for (long y = tile1.Y + 1; y < tile2.Y; y++)
                {
                    grid[tile1.X - minX, y - minY] = '#';
                }
            }
        }

        for (long y = 0; y <= maxY; y++)
        {
            for (long x = 0; x <= maxX; x++)
            {
                Console.Write(grid[x, y]);
            }
            Console.WriteLine();
        }

        for (int i = 0; i < redTiles.Count; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                char previous = '.';
                var inside = false;
                var corner1 = new Tile(redTiles[i].X - minX, redTiles[i].Y - minY);
                var corner2 = new Tile(redTiles[j].X - minX, redTiles[j].Y - minY);
                var corner3 = new Tile(corner1.X, corner2.Y);
                var corner4 = new Tile(corner2.X, corner1.Y);
                if (IsInsideArea(grid, corner1) && IsInsideArea(grid, corner2) && IsInsideArea(grid, corner3) && IsInsideArea(grid, corner4))
                {
                    var area = (Math.Abs(redTiles[i].X - redTiles[j].X) + 1) * (Math.Abs(redTiles[i].Y - redTiles[j].Y) + 1);
                    if (area >= answer)
                    {
                        answer = area;
                    }
                }
            }
        }




        return answer;
    }

    private bool IsInsideArea(char[,] grid, Tile tile)
    {
        char previous = '.';
        var inside = false;
        for (int x = 0; x <= tile.X; x++)
        {
            var current = grid[x, tile.Y];
            if (current == '#' && previous == '.')
            {
                inside = !inside;
            }
            previous = current;
        }
        return inside;
    }

    public record Tile(long X, long Y);
}

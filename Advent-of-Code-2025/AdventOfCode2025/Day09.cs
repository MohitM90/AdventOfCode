using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks.Dataflow;

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
        var orderedGridY = redTiles.ToLookup(t => t.Y).ToDictionary(k => k.Key, v => v.ToHashSet());
        var orderedGridX = redTiles.ToLookup(t => t.X).ToDictionary(k => k.Key, v => v.ToHashSet());

        for (int i = 0; i < redTiles.Count; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                var corner1 = redTiles[i];
                var corner2 = redTiles[j];
                var corner3 = new Tile(corner1.X, corner2.Y);
                var corner4 = new Tile(corner2.X, corner1.Y);

                if (IsInside(redTiles, corner1) && IsInside(redTiles, corner2) &&
                    IsInside(redTiles, corner3) && IsInside(redTiles, corner4))
                {
                    if (!IntersectsPolygon(redTiles, corner1, corner2, corner3, corner4))
                    {
                        var area = (Math.Abs(corner1.X - corner2.X) + 1) * (Math.Abs(corner1.Y - corner2.Y) + 1);
                        if (area >= answer)
                        {
                            answer = area;
                        }
                    }
                }
            }
        }




        return answer;
    }

    private bool IsInside(List<Tile> tiles, Tile point)
    {
        bool isInside = false;
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            var tileA = tiles[i];
            var tileB = tiles[(i + 1) % tiles.Count];

            var minX = Math.Min(tileA.X, tileB.X);
            var maxX = Math.Max(tileA.X, tileB.X);
            var minY = Math.Min(tileA.Y, tileB.Y);
            var maxY = Math.Max(tileA.Y, tileB.Y);

            if (tileA.Y == tileB.Y && tileA.Y == point.Y 
                && point.X >= minX && point.X <= maxX)
            {
                return true;
            }

            if (tileA.X == tileB.X && tileA.X == point.X
                && point.Y >= minY && point.Y <= maxY)
            {
                return true;
            }

            if ((tileA.Y < point.Y && tileB.Y >= point.Y) || (tileB.Y < point.Y && tileA.Y >= point.Y))
            {
                if (tileA.Y == tileB.Y)
                {
                    continue;
                }

                var intersectX = tileA.X + (point.Y - tileA.Y) * (tileB.X - tileA.X) / (tileB.Y - tileA.Y);

                if (intersectX > point.X)
                {
                    isInside = !isInside;
                }
            }
        }
        return isInside;
    }

    private bool IntersectsPolygon(List<Tile> tiles, Tile corner1, Tile corner2, Tile corner3, Tile corner4)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            var tileA = tiles[i];
            var tileB = tiles[(i + 1) % tiles.Count];
            
            var yMin = Math.Min(corner1.Y, corner2.Y);
            var yMax = Math.Max(corner1.Y, corner2.Y);
            var xMin = Math.Min(corner1.X, corner2.X);
            var xMax = Math.Max(corner1.X, corner2.X);

            if (tileA.Y == tileB.Y)
            {
                var maxX = Math.Max(tileA.X, tileB.X);
                var minX = Math.Min(tileA.X, tileB.X);

                if (tileA.Y > yMin && tileA.Y < yMax && maxX > xMin && minX < xMax)
                {
                    return true; 
                }
            }
            else if (tileA.X == tileB.X)
            {
                var maxY = Math.Max(tileA.Y, tileB.Y);
                var minY = Math.Min(tileA.Y, tileB.Y);
                if (tileA.X > xMin && tileA.X < xMax && maxY > yMin && minY < yMax)
                {
                    return true; 
                }
            }

        }
        return false;
    }


    public record Tile(long X, long Y);
}

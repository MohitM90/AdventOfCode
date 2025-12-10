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
            for (int j = 0; j < redTiles.Count; j++)
            {
                var corner1 = redTiles[i];
                var corner2 = redTiles[j];
                var corner3 = new Tile(corner1.X, corner2.Y);
                var corner4 = new Tile(corner2.X, corner1.Y);

                if (IsInside(orderedGridY[corner1.Y], orderedGridX[corner1.X], corner1, corner3, corner4) &&
                    IsInside(orderedGridY[corner2.Y], orderedGridX[corner2.X], corner2, corner4, corner3) &&
                    IsInside(orderedGridY[corner3.Y], orderedGridX[corner3.X], corner3, corner1, corner4) &&
                    IsInside(orderedGridY[corner4.Y], orderedGridX[corner4.X], corner4, corner2, corner3))
                {
                    var area = (Math.Abs(corner1.X - corner2.X) + 1) * (Math.Abs(corner1.Y - corner2.Y) + 1);
                    if (area >= answer)
                    {
                        answer = area;
                    }
                }

            }
        }




        return answer;
    }

    private bool IsInside(HashSet<Tile> gridY, HashSet<Tile> gridX, Tile tile, Tile tile2, Tile tile3)
    {
        //if (grid.Count == 4) 
        //{
        //    return (grid.Count(t => t.X > tile.X) >= 2 && grid.Count(t => t.X > tile2.X) >= 2) ||
        //        (grid.Count(t => t.X < tile.X) >= 2 && grid.Count(t => t.X < tile2.X) >= 2);
        //}
        return gridY.Count(t => t.X > tile.X) < 2 && gridX.Count(t => t.Y > tile.Y) < 2;
    }


    public record Tile(long X, long Y);
}

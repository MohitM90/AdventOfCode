using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AdventOfCode2025;

internal class Day07 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");

        inputs[0] = inputs[0].Replace('S', '|');
        for (int i = 0; i < inputs.Length - 1; i++)
        {
            var indexes = inputs[i].AllIndexesOf('|');
            var nextline = inputs[i + 1];
            foreach (var index in indexes)
            {
                if (nextline[index] == '.')
                {
                    nextline = nextline.ReplaceAt(index, '|');
                }
                else if (nextline[index] == '^')
                {
                    nextline = nextline.ReplaceAt(index - 1, '|');
                    nextline = nextline.ReplaceAt(index + 1, '|');
                    answer++;
                }

            }
            inputs[i + 1] = nextline;
        }

        return answer;
    }

    public override async Task<long> Puzzle2(string input)
    {
        long answer = 0;
        var inputs = input.Split("\r\n");

        Dictionary<Beam, long> beams = [];
        answer = GetTimelines(new Beam(inputs[0].IndexOf('S'), 0), beams, inputs);

        return answer;
    }

    private long GetTimelines(Beam beam, Dictionary<Beam, long> beams, string[] inputs)
    {
        if (beam.Y == inputs.Length - 1)
        {
            beams.Add(beam, 1);
            return 1;
        }
        if (beams.TryGetValue(beam, out long value))
        {
            return value; 
        }
        var nextline = inputs[beam.Y + 1];
        if (nextline[beam.X] == '.')
        {
            var timelines = GetTimelines(new(beam.X, beam.Y + 1), beams, inputs);
            beams.Add(beam, timelines);
            return timelines;
        }
        else if (nextline[beam.X] == '^')
        {
            var timelines = GetTimelines(new(beam.X - 1, beam.Y + 1), beams, inputs) +
                   GetTimelines(new(beam.X + 1, beam.Y + 1), beams, inputs);
            beams.Add(beam, timelines);
            return timelines;
        }
        return 0;
    }

    public record Beam(int X, int Y);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day2
{

    static int PuzzleDay2a(string input)
    {
        int sum = 0;
        const int MaxRed = 12;
        const int MaxGreen = 13;
        const int MaxBlue = 14;

        foreach (var s in input.Split("\r\n"))
        {

            bool fail = false;
            var data = s.Split(":");
            string idString = data[0].Trim();
            int id = int.Parse(idString[idString.IndexOf(idString.FirstOrDefault(x => x >= '0' && x <= '9'))..]);

            var cubes = data[1].Split(";");
            foreach (var c in cubes)
            {
                var match = Regex.Match(c.Trim(), "(\\d+) red");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > MaxRed)
                    {
                        fail = true;
                        break;
                    }
                }

                match = Regex.Match(c.Trim(), "(\\d+) blue");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > MaxBlue)
                    {
                        fail = true;
                        break;
                    }
                }

                match = Regex.Match(c.Trim(), "(\\d+) green");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > MaxGreen)
                    {
                        fail = true;
                        break;
                    }
                }
            }
            if (!fail)
            {
                sum += id;
            }
        }

        return sum;
    }

    static int PuzzleDay2b(string input)
    {
        List<Day2Input> bags = new List<Day2Input>();

        foreach (var s in input.Split("\r\n"))
        {
            var data = s.Split(":");
            string idString = data[0].Trim();
            int id = int.Parse(idString[idString.IndexOf(idString.FirstOrDefault(x => x >= '0' && x <= '9'))..]);
            var bag = new Day2Input
            {
                Id = id
            };

            var cubes = data[1].Split(";");
            foreach (var c in cubes)
            {
                var match = Regex.Match(c.Trim(), "(\\d+) red");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > bag.Red)
                    {
                        bag.Red = val;
                    }
                }

                match = Regex.Match(c.Trim(), "(\\d+) blue");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > bag.Blue)
                    {
                        bag.Blue = val;
                    }
                }

                match = Regex.Match(c.Trim(), "(\\d+) green");
                if (match.Success)
                {
                    int val = int.Parse(match.Groups[1].Value);
                    if (val > bag.Green)
                    {
                        bag.Green = val;
                    }
                }
            }
            bags.Add(bag);
        }

        return bags.Sum(x => x.Power);
    }

    private class Day2Input
    {
        public int Id { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Power => Red * Green * Blue;
    }

}

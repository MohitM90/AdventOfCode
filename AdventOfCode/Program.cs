using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine(Day10.PuzzleB(input));
    }


}

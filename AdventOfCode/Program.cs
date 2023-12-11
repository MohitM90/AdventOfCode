using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Program
{
    static async Task Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine(await Day11.PuzzleB(input));
    }


}

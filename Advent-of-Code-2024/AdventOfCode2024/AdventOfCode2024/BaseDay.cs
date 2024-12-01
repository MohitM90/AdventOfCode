using System.Diagnostics;

namespace AdventOfCode2024;

internal abstract class BaseDay
{
    public string Input { get; set; } = File.ReadAllText("input.txt");

    public abstract int Puzzle1();
    public abstract int Puzzle2();

    public void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine($"Answer 1: {Puzzle1()} (Time: {sw})");
        sw.Restart();
        Console.WriteLine($"Answer 2: {Puzzle2()} (Time: {sw})");
        Console.WriteLine("--------------------------");
    }
}

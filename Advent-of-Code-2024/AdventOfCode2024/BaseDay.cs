using System.Diagnostics;

namespace AdventOfCode2024;

internal abstract class BaseDay
{
    public string Input { get; set; } = File.ReadAllText("input.txt");

    public virtual int Puzzle1() { throw new NotImplementedException(); }
    public virtual int Puzzle2() { throw new NotImplementedException(); }

    public virtual async Task<int> Puzzle1Async() { throw new NotImplementedException(); }
    public virtual async Task<int> Puzzle2Async() { throw new NotImplementedException(); }

    public void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine($"Answer 1: {Puzzle1()} (Time: {sw})");
        sw.Restart();
        Console.WriteLine($"Answer 2: {Puzzle2()} (Time: {sw})");
        Console.WriteLine("--------------------------");
    }

    public async Task RunAsync()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine($"Answer 1: {await Puzzle1Async()} (Time: {sw})");
        sw.Restart();
        Console.WriteLine($"Answer 2: {await Puzzle2Async()} (Time: {sw})");
        Console.WriteLine("--------------------------");
    }
}

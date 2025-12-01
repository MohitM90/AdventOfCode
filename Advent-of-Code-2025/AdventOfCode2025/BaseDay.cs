using System.Diagnostics;

namespace AdventOfCode2025;

internal abstract class BaseDay<T>
{
    private string Input { get; set; } = File.ReadAllText("input.txt");
    private string ExampleInput { get; set; } = File.ReadAllText("example.txt");

    public abstract Task<T> Puzzle1(string input);
    public abstract Task<T> Puzzle2(string input);

    public async Task RunAsync()
    {
        if (string.IsNullOrEmpty(ExampleInput) || string.IsNullOrEmpty(Input))
        {
            Console.WriteLine("Input or ExampleInput is null or empty. Please ensure 'input.txt' and 'example.txt' files are present.");
            return;
        }
        Console.WriteLine("--------------------------");
        Console.WriteLine("Example Results:");
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine($"Answer 1: {await Puzzle1(ExampleInput)} (Time: {sw})");
        sw.Restart();
        Console.WriteLine($"Answer 2: {await Puzzle2(ExampleInput)} (Time: {sw})");
        Console.WriteLine("--------------------------");
        Console.WriteLine("Puzzle Results:");
        sw.Restart();
        Console.WriteLine($"Answer 1: {await Puzzle1(Input)} (Time: {sw})");
        sw.Restart();
        Console.WriteLine($"Answer 2: {await Puzzle2(Input)} (Time: {sw})");
        Console.WriteLine("--------------------------");
    }
}

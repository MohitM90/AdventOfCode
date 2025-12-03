using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2025;

internal static class Extensions
{
    public static int Modulo(this int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }

    public static long Modulo(this long a, long b)
    {
        return (Math.Abs(a * b) + a) % b;
    }

    public static string Repeat(this string input, int count)
    {
        if (string.IsNullOrEmpty(input) || count <= 1)
            return input;

        var builder = new StringBuilder(input.Length * count);

        for (var i = 0; i < count; i++) builder.Append(input);

        return builder.ToString();
    }
}

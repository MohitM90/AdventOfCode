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

    public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
    {
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
        }
    }

    public static IEnumerable<int> AllIndexesOf(this string str, char searchchar)
    {
        return str.AllIndexesOf(searchchar.ToString());
    }

    public static string ReplaceAt(this string str, int index, char newChar)
    {
        var sb = new StringBuilder(str);
        sb[index] = newChar;
        return sb.ToString();
    }

    public static T[,] To2DArray<T>(this T[][] values)
    {
        int rows = values.Length;
        int cols = values[0].Length;
        T[,] result = new T[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = values[i][j];
            }
        }
        return result;
    }

    public static T[][] Transpose<T>(this T[][] array)
    {
        int rows = array.Length;
        int cols = array[0].Length;
        T[][] transposed = new T[cols][];
        for (int i = 0; i < cols; i++)
        {
            transposed[i] = new T[rows];
            for (int j = 0; j < rows; j++)
            {
                transposed[i][j] = array[j][i];
            }
        }
        return transposed;
    }

    public static T[,] Transpose<T>(this T[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        T[,] result = new T[cols, rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[j, i] = array[i, j];
            }
        }
        return result;
    }
}

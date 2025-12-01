namespace AdventOfCode2025;

internal class Day01 : BaseDay<long>
{
    public override async Task<long> Puzzle1(string inputString)
    {
        long answer = 0;
        var inputs = inputString.Split("\r\n");

        var value = 50;
        foreach (var input in inputs)
        {
            int num = int.Parse(input[1..]);
            if (input[0] == 'L')
            {
                value = (value - num).Modulo(100);
            }
            else
            {
                value = (value + num).Modulo(100);
            }
            if (value == 0)
            {
                answer++;
            }
        }

        return answer;
    }

    public override async Task<long> Puzzle2(string inputString)
    {
        long answer = 0;
        var inputs = inputString.Split("\r\n");

        var value = 50;
        foreach (var input in inputs)
        {
            int num = int.Parse(input[1..]);
            var add = 0;
            if (num == 0)
            {
                continue;
            }
            if (input[0] == 'L')
            {
                
                if ((value - num) <= 0)
                {
                    add = (int)Math.Floor(Math.Abs(value - num) / 100.0);
                    if (value != 0)
                    {
                        add++;
                    }
                }
                
                value = (value - num).Modulo(100);
            }
            else
            {
                if ((value + num) >= 100)
                {
                    add = (int)Math.Floor(Math.Abs(value + num) / 100.0);
                }
                value = (value + num).Modulo(100);
            }
            answer += add;
        }

        return answer;
    }
}

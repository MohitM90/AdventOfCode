using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;
internal class Day7
{
    internal static int PuzzleB(string input)
    {
        int sum = 0;
        var inputs = input.Split("\r\n")
            .Select(x => x.Split(" "))
            .Select(x => new Game(x[0], int.Parse(x[1])))
            .ToList();
        inputs.Sort();
        for (int i = 0; i < inputs.Count; i++)
        {
            sum += inputs[i].Bid * (i + 1);
        }

        return sum;
    }

    private class Game(string hand, int bid) : IComparable<Game>
    {
        public Card[] Hand { get; set; } = hand
            .Select(x => (Card)Enum.Parse(typeof(Card), x.ToString()))
            .ToArray();

        public int Bid { get; set; } = bid;

        public HandType HandType => GetHandType();

        private HandType GetHandType()
        {
            var x = Hand
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .ToList();

            if (x.Count == 1)
            {
                return HandType.FiveOfAKind;
            }
            if (x.Count == 2 && x[0].Count() == 4)
            {
                if (x.Any(x => x.Key == Card.J))
                {
                    return HandType.FiveOfAKind;
                }
                return HandType.FourOfAKind;
            }
            if (x.Count == 2 && x[0].Count() == 3 && x[1].Count() == 2)
            {
                if (x.Any(x => x.Key == Card.J))
                {
                    return HandType.FiveOfAKind;
                }
                return HandType.FullHouse;
            }
            if (x.Count == 3 && x[0].Count() == 3)
            {
                if (x.Any(x => x.Key == Card.J))
                {
                    return HandType.FourOfAKind;
                }

                return HandType.ThreeOfAKind;
            }
            if (x.Count == 3 && x[0].Count() == 2 && x[1].Count() == 2)
            {
                if (x[0].Key == Card.J || x[1].Key == Card.J)
                {
                    return HandType.FourOfAKind;
                }
                if (x[2].Key == Card.J)
                {
                    return HandType.FullHouse;
                }
                return HandType.TwoPair;
            }
            if (x.Count == 4)
            {
                if (x.Any(x => x.Key == Card.J))
                {
                    return HandType.ThreeOfAKind;
                }
                return HandType.OnePair;
            }
            if (x.Any(x => x.Key == Card.J))
            {
                return HandType.OnePair;
            }
            return HandType.HighCard;
        }

        public int CompareTo(Game? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (other.HandType == HandType)
            {
                for (int i = 0; i < Hand.Length; i++)
                {
                    if (Hand[i] != other.Hand[i])
                    {
                        return Hand[i].CompareTo(other.Hand[i]);
                    }
                }
                return 0;
            }
            return HandType.CompareTo(other.HandType);
        }
    }

    internal enum Card
    {
        A = 14,
        K = 13,
        Q = 12,
        J = 1,
        T = 10
    }

    internal enum HandType
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day19 : BaseDay<long>
{

    public override long Puzzle1()
    {
        long answer = 0;
        var input = Input.Split("\r\n\r\n");

        var rules = input[0].Split(", ").ToList();
        List<string> rulesFiltered = FilterRules(rules);

        answer = input[1].Split("\r\n").Count(x => IsMatch(x, rulesFiltered));

        return answer;
    }

    public override long Puzzle2()
    {
        long answer = 0;
        var input = Input.Split("\r\n\r\n");

        var rules = input[0].Split(", ").OrderBy(s => s.Length).ToList();
        List<string> matchRules = FilterRules(rules);

        var lines = input[1].Split("\r\n");

        var pattern = $"^(?=({string.Join("|", rules)}))";

        foreach (string s in lines)
        {
            if (!IsMatch(s, matchRules))
            {
                continue;
            }
            var rulesFiltered = rules.Where(s.Contains).Distinct().ToList();
            
            Dictionary<string, Tree> trees = [];
            var tree = new Tree { Value = string.Empty };

            FindMatches(s, rulesFiltered, trees, tree);
            answer += tree.GetLeavesCount();
        }

        return answer;
    }

    private List<string> FilterRules(List<string> rules)
    {
        List<string> rulesFiltered = new();
        foreach (var rule in rules)
        {
            var p = "^(?:" + string.Join("|", rules.Except([rule])) + ")+$";
            if (!Regex.IsMatch(rule, p))
            {
                rulesFiltered.Add(rule);
            }
        }
        return rulesFiltered;
    }

    private bool IsMatch(string s, List<string> rules)
    {
        var pattern = "^(?:" + string.Join("|", rules) + ")+$";

        return Regex.IsMatch(s, pattern);
    }

    private bool FindMatches(string s, List<string> rules, Dictionary<string, Tree> trees, Tree parent)
    {
        if (s == string.Empty)
        {
            trees.TryAdd(parent.Value, parent);
            return true;
        }
        foreach (var rule in rules.Where(x => x.Length <= s.Length))
        {
            if (s.StartsWith(rule))
            {
                if (trees.TryGetValue(parent.Value + rule, out Tree? value))
                {
                    value.GetLeavesCount();
                    parent.Children.Add(value);
                    continue;
                }
                var tree = new Tree { Value = parent.Value + rule };
                parent.Children.Add(tree);
                bool success = FindMatches(s[rule.Length..], rules, trees, tree);
                if (!success)
                {
                    parent.Children.Remove(tree);
                    continue;
                }
                trees.TryAdd(parent.Value, parent);
            }
        }
        return parent.Children.Count > 0;
    }

    private record Tree
    {
        private long _leavesCount = 0;

        public string Value { get; set; } = string.Empty;
        public List<Tree> Children { get; set; } = new();

        public long GetLeavesCount()
        {
            if (Children.Count == 0)
            {
                return 1;
            }
            if (_leavesCount != 0)
            {
                return _leavesCount;
            }
            _leavesCount = Children.Sum(x => x.GetLeavesCount());
            return _leavesCount;
        }

    }
}
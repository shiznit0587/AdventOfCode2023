using System.Collections;

class Day12
{
    readonly Dictionary<string, HashSet<EquatableList<int>>> rowToGroups = [];
    readonly Dictionary<EquatableList<int>, HashSet<string>> groupToRows = [];
        
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        return;

        List<(string row, List<int> counts)> tuples = [];
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            tuples.Add((parts[0], parts[1].Split(',').Select(int.Parse).ToList()));
        }

        // What's the longest row?
        int longestRow = tuples.Select(t => t.row.Length).Max();

        Console.WriteLine($"Longest Part 2 Row = {longestRow * 3 + 2}");

        foreach (var (row, _) in tuples)
        {
            if (!rowToGroups.ContainsKey(row))
            {

            }
        }





        // I think I need to track groups as a string and not as a List<int>, since it needs to be hashable and usable as a Dictionary key.

        // Options when adding a . are the same as the previous string.
        // Options when adding a # are:
            // When the previous string ended in '#', the same as the previous string, +1 on the last group
            // When the previous string ended in '.', the same as the previous string , plus a new 1 group at the end

        // When checking what row options exist for a group... how? I can figure out what groups for a row, but how do I do the other way around?
        // And if I find all row options that exist for a group, how do I check for matches?

        // I think I need to find all groups that match for all strings, and track in the other direction as well.
        // When I'm looking for rows that match a group,
        //   Get all of them,
        //   check each one against the input.
    }

    private void BuildRowGroups(string row)
    {
        if (rowToGroups.ContainsKey(row))
            return;

        if (rowToGroups.ContainsKey(row[0..-1]))
        {
            // Options when adding a . are the same as the previous string.
            // Options when adding a # are:
                // When the previous string ended in '#', the same as the previous string, +1 on the last group
                // When the previous string ended in '.', the same as the previous string , plus a new 1 group at the end

            //
        }
        else
        {
            //
        }
    }

    private class EquatableList<T> : IReadOnlyList<T>, IEquatable<EquatableList<T>>
    {
        private readonly int _hashCode;
        private readonly List<T> _items;
        
        public EquatableList(List<T> items)
        {
            _items = items;
            _hashCode = _items.Aggregate(0, (hc, a) => hc ^ a!.GetHashCode());
        }

        public T this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public override int GetHashCode() => _hashCode;

        public bool Equals(EquatableList<T>? rhs)
        {
            if (ReferenceEquals(this, rhs))
                return true;
            if (rhs?._hashCode != _hashCode)
                return false;
            return _items.SequenceEqual(rhs._items);
        }

        public override bool Equals(object? obj) => Equals(obj as EquatableList<T>);
    }

    bool RowMatches(string row, string match)
    {
        if (row.Length != match.Length)
            return false;

        for (int i = 0; i < row.Length; ++i)
            if (row[i] != match[i] && row[i] != '?')
                return false;

        return true;
    }

    public async Task Run2()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 12 - Part 1");

        long matchCountSum = 0;
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            List<char> row = [..parts[0]];
            var groups = parts[1].Split(',').Select(int.Parse).ToList();

            matchCountSum += CountMatches(row, groups);
        }

        Console.WriteLine($"Valid Arrangements = {matchCountSum}");

        Console.WriteLine("Running Day 12 - Part 2");

        matchCountSum = 0;
        Parallel.For(0, input.Length, i =>
        {
            string line = input[i];
            var parts = line.Split(' ');
            List<char> row = [..parts[0], '?', ..parts[0], '?', ..parts[0], '?', ..parts[0], '?', ..parts[0]];
            var groups = parts[1].Split(',').Select(int.Parse).ToList();
            groups = [..groups, ..groups, ..groups, ..groups, ..groups];

            long matches = CountMatches(row, groups);
            Interlocked.Add(ref matchCountSum, matches);
        });

        Console.WriteLine($"Valid Arrangements = {matchCountSum}");
    }

    private static long CountMatches(List<char> row, List<int> groups, int i = 0, int currentGroupSize = 0)
    {
        if (i == row.Count)
            return (groups.Count, currentGroupSize) switch
            {
                (0, 0) => 1,
                (1, _) when currentGroupSize == groups[0] => 1,
                _ => 0
            };

        return (row[i], groups.Count, currentGroupSize) switch
        {
            ('.', _, 0) => CountMatches(row, groups, i + 1, currentGroupSize),
            ('.', > 0, _) when groups[0] == currentGroupSize => CountMatches(row, groups[1..], i + 1, 0),
            ('.', _, _) => 0,
            ('#', 0, _) => 0,
            ('#', _, _) when currentGroupSize < groups[0] => CountMatches(row, groups, i + 1, currentGroupSize + 1),
            ('#', _, _) => 0,
            (_, _, _) => 
                CountMatches([..row[0..i] , '#', ..row[(i + 1)..]], groups, i, currentGroupSize) + 
                CountMatches([..row[0..i], '.', ..row[(i + 1)..]], groups, i, currentGroupSize)
        };
    }
}

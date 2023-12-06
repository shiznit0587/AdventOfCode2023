using System.Collections;
using System.Diagnostics.CodeAnalysis;

class Day5
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 5 - Part 1");

        var seeds = input[0][7..].Split(" ").Select(long.Parse).ToList();
        List<MappingList> mappings = [new()];
        MappingList map = mappings[0];

        foreach (string line in input.Skip(2))
        {
            if (string.IsNullOrEmpty(line))
            {
                map = new();
                mappings.Add(map);
            }
            else if (!line.Contains(':'))
            {
                map.Mappings.Add(Mapping.Parse(line));
            }
        }

        var results = seeds.ToList();
        foreach (var m in mappings)
        {
            for (int i = 0; i < seeds.Count; ++i)
            {
                results[i] = m.MapSource(results[i]);
            }
        }

        Console.WriteLine($"Lowest location number = {results.Min()}");

        Console.WriteLine("Running Day 5 - Part 2");
    }

    class Mapping
    {
        public long SourceStart;
        public long DestStart;
        public long Length;

        public bool IsSourceMapped(long source) => SourceStart <= source && source < SourceStart + Length;

        public long MapSource(long source) => source - SourceStart + DestStart;

        public static Mapping Parse(string s)
        {
            var values = s.Split(" ").Select(long.Parse).ToList();
            return new Mapping { SourceStart = values[1], DestStart = values[0], Length = values[2] };
        }
    }

    class MappingList
    {
        static readonly Mapping DEFAULT_MAPPING = new();

        public List<Mapping> Mappings = [];

        public long MapSource(long source)
        {
            var mapping = Mappings.Find(m => m.IsSourceMapped(source)) ?? DEFAULT_MAPPING;
            return mapping.MapSource(source);
        }
    }
}

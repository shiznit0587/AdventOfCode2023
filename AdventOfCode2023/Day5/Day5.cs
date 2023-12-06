using System.Collections;
using System.Diagnostics.CodeAnalysis;

class Day5
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 5 - Part 1");

        var seeds = input[0][7..].Split(" ").Select(long.Parse).ToList();
        Mapping mapping = new();
        List<Mapping> mappings = [mapping];        

        foreach (string line in input.Skip(2))
        {
            if (string.IsNullOrEmpty(line))
            {
                mapping = new();
                mappings.Add(mapping);
            }
            else if (!line.Contains(':'))
            {
                mapping.Entries.Add(MappingEntry.Parse(line));
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

        List<LongRange> ranges = [];
        for (int i = 0; i < seeds.Count; i += 2)
        {
            ranges.Add(new LongRange { Min = seeds[i], Max = seeds[i] + seeds[i + 1] - 1 });
        }

        foreach (var map in mappings)
        {
            ranges = ranges.SelectMany(map.MapSourceRange).ToList();
        }

        Console.WriteLine($"Lowest location number = {ranges.Select(r => r.Min).Min()}");
    }

    class MappingEntry
    {
        private readonly LongRange _sourceRange;
        private readonly long _destOffset;

        public bool IsSourceMapped(long source) => _sourceRange.Contains(source);

        public long MapSource(long source) => source + _destOffset;

        public (LongRange?, IEnumerable<LongRange>) MapSourceRange(LongRange range)
        {
            var intersection = _sourceRange.Intersection(range);
            if (intersection != null)
            {
                intersection = intersection.Offset(_destOffset);
            }

            return (intersection, _sourceRange.Differences(range));
        }

        public static MappingEntry Parse(string s)
        {
            var values = s.Split(" ").Select(long.Parse).ToList();
            return new MappingEntry(values[1], values[0], values[2]);
        }

        MappingEntry(long sourceStart, long destStart, long length)
        {
            _sourceRange = new LongRange { Min = sourceStart, Max = sourceStart + length - 1 };
            _destOffset = destStart - sourceStart;
        }
    }

    class Mapping
    {
        public List<MappingEntry> Entries = [];

        public long MapSource(long source)
        {
            var mapping = Entries.Find(m => m.IsSourceMapped(source));
            return mapping?.MapSource(source) ?? source;
        }

        public List<LongRange> MapSourceRange(LongRange source)
        {
            List<LongRange> sourceRanges = [source];
            List<LongRange> mappedRanges = [];

            foreach (var entry in Entries)
            {
                List<LongRange> unmappedRanges = [];
                foreach (var range in sourceRanges)
                {
                    (var mapped, var unmapped) = entry.MapSourceRange(range);
                    if (mapped != null)
                    {
                        mappedRanges.Add(mapped);
                    }
                    unmappedRanges.AddRange(unmapped);
                }
                sourceRanges = unmappedRanges;
            }

            mappedRanges.AddRange(sourceRanges);
            return mappedRanges;
        }
    }

    class LongRange
    {
        public long Min { get; init; }
        public long Max { get; init; }

        public bool Contains(long value) => Min <= value && value <= Max;

        public bool Intersects(LongRange range) => !(range.Max < Min || range.Min > Max);

        public LongRange Offset(long offset) => new() { Min = Min + offset, Max = Max + offset };

        public LongRange? Intersection(LongRange range)
        {
            return Intersects(range) ? new() { Min = Math.Max(Min, range.Min), Max = Math.Min(Max, range.Max) } : null;
        }

        public IEnumerable<LongRange> Differences(LongRange range)
        {
            // Return full range if there's no intersection
            if (!Intersects(range))
            {
                yield return range;
                yield break;
            }

            // Return range to the left of the intersection (if there is one)
            if (range.Min < Min)
            {
                yield return new LongRange { Min = range.Min, Max = Min - 1 };
            }

            // Return range to the right of the intersection (if there is one)
            if (range.Max > Max)
            {
                yield return new LongRange { Min = Max + 1, Max = range.Max };
            }
        }
    }
}

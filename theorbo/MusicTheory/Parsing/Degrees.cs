using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sprache;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static class Degrees
    {
        public static Dictionary<string, ValueTuple<int, KnownChordKind>> ScaleDegrees =
            new Dictionary<string, ValueTuple<int, KnownChordKind>>
            {
                ["VII"] = (7, KnownChordKind.Maj),
                ["vii"] = (7, KnownChordKind.Min),

                ["VI"] = (6, KnownChordKind.Maj),
                ["vi"] = (6, KnownChordKind.Min),

                ["V"] = (5, KnownChordKind.Maj),
                ["v"] = (5, KnownChordKind.Min),

                ["IV"] = (4, KnownChordKind.Maj),
                ["iv"] = (4, KnownChordKind.Min),

                ["III"] = (3, KnownChordKind.Maj),
                ["iii"] = (3, KnownChordKind.Min),

                ["II"] = (2, KnownChordKind.Maj),
                ["ii"] = (2, KnownChordKind.Min),

                ["I"] = (1, KnownChordKind.Maj),
                ["i"] = (1, KnownChordKind.Min)
            };

        private static readonly IEnumerable<Parser<ValueTuple<int, KnownChordKind>>> RomanNumeralParsers = ScaleDegrees
            .Select(s => Parse.String(s.Key).Select(v => s.Value));

        private static readonly IEnumerable<Parser<ValueTuple<int, KnownChordKind>>> ArabicNumeralParsers = Enumerable
            .Range(1, 8)
            .Select(deg =>
                Parse.String(deg.ToString(CultureInfo.InvariantCulture))
                    .Select(v => ValueTuple.Create(deg, KnownChordKind.Maj)));

        //Matches scale degree without chord extensions
        public static Parser<ValueTuple<Accidental, int, KnownChordKind>> BaseDegreePraser = RomanNumeralParsers
            .Concat(ArabicNumeralParsers)
            .Select(parser => AccidentalWithChordKindAndDegree(DegreeWithChordKind(parser)))
            .Aggregate((p1, p2) => p1.Or(p2));

        //Optionally take into account chord kinds (as, `m` for minor in 1m)
        private static Parser<ValueTuple<int, KnownChordKind>> DegreeWithChordKind(
            Parser<ValueTuple<int, KnownChordKind>> source)
        {
            return from scaleDegree in source
                from kind in Alterations.ChordKindParser.Optional()
                select ValueTuple.Create(scaleDegree.Item1, kind.IsEmpty ? scaleDegree.Item2 : kind.Get());
        }

        //Optionally take into account accidentals (as, `#` for sharp in #I)
        private static Parser<ValueTuple<Accidental, int, KnownChordKind>> AccidentalWithChordKindAndDegree(
            Parser<ValueTuple<int, KnownChordKind>> source)
        {
            return from accidental in Alterations.AccidentalParser.Optional()
                from scaleDegree in source
                select ValueTuple.Create(accidental.IsEmpty ? Accidental.None : accidental.Get(), scaleDegree.Item1,
                    scaleDegree.Item2);
        }
    }
}
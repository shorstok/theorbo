using System.Collections.Generic;
using System.Linq;
using Sprache;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static class Alterations
    {
        public static Dictionary<string, Accidental> Accidentals = new Dictionary<string, Accidental>
        {
            ["b"] = Accidental.Flat,
            ["bb"] = Accidental.DoubleFlat,
            ["##"] = Accidental.DoubleSharp,
            ["#"] = Accidental.Sharp
        };

        public static Dictionary<string, KnownChordKind> KnownChordKinds = new Dictionary<string, KnownChordKind>
        {
            ["m"] = KnownChordKind.Min,
            ["mi"] = KnownChordKind.Min,
            ["min"] = KnownChordKind.Min,
            ["minor"] = KnownChordKind.Min,
            ["M"] = KnownChordKind.Maj,
            ["-"] = KnownChordKind.Min,
            ["aug"] = KnownChordKind.Aug,
            ["dim"] = KnownChordKind.Dim,
            ["lyd"] = KnownChordKind.Lyd,
            ["sus2"] = KnownChordKind.Sus2,
            ["sus4"] = KnownChordKind.Sus4,
            ["sus"] = KnownChordKind.Sus4,
            ["Q"] = KnownChordKind.Q
        };

        internal static readonly Parser<KnownChordKind> ChordKindParser = KnownChordKinds
            .OrderByDescending(s => s.Key.Length)
            .Select(s => Parse.String(s.Key).Except(ChordExtensions.MajParser).Select(v => s.Value)).Aggregate((a, b) => a.Or(b));

        internal static readonly Parser<Accidental> AccidentalParser = Accidentals
            .OrderByDescending(s => s.Key.Length)
            .Select(s => Parse.String(s.Key).Select(v => s.Value)).Aggregate((a, b) => a.Or(b));
    }
}
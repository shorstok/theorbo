using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static class Chords
    {        
        public static Parser<Tuple<NoteValue, Accidental, KnownChordKind>> ChordBasePraser =
            from noteval in Notes.NoteNames.Select(s => Parse.String(s.Key).Select(v => s.Value)).Aggregate((p1, p2) => p1.Or(p2))
            from accidental in Alterations.AccidentalParser.Optional()
            from kind in Alterations.ChordKindParser.Optional()
            select Tuple.Create(noteval, accidental.GetOrElse(Accidental.None), kind.GetOrElse(KnownChordKind.None));

        private static readonly Parser<Tuple<NoteValue,Accidental>> ChordInversionParser =
            from chordName in Notes.NoteNames.Select(s => Parse.IgnoreCase("/"+s.Key).Select(v => s.Value)).Aggregate((p1, p2) => p1.Or(p2))
            from accidental in Alterations.AccidentalParser.Optional()
            select Tuple.Create(chordName, accidental.GetOrElse(Accidental.None));

        //Resulting parser - matches chord with all extensions
        public static Parser<Chord> ChordPraser =
            from chord in ChordBasePraser
            from ext in ChordExtensions.ExtensionParser.Optional()
            from inversion in  ChordInversionParser.Optional() 
            select ChordOrigin.FromParseResults(chord.Item1,
                chord.Item2,
                chord.Item3,
                ext.IsEmpty
                    ? ChordExtensions.ExtensionBase.Default
                    : ext.Get().Item1,
                ext.IsEmpty
                    ? new List<ChordExtensions.Extension>()
                    : ext.Get().Item2,
                inversion.GetOrElse(null)).BuildChord();
    }
}
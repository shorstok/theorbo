using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static class Notes
    {
        public static Dictionary<string, NoteValue> NoteNames =
            new Dictionary<string, NoteValue>
            {
                ["C"]=NoteValue.C,
                ["D"]=NoteValue.D,
                ["E"]=NoteValue.E,
                ["F"]=NoteValue.F,
                ["G"]=NoteValue.G,
                ["A"]=NoteValue.A,
                ["B"]=NoteValue.B,
                ["H"]=NoteValue.B,
            };

        public static Parser<Tuple<NoteValue,Accidental>> NoteParser =
            from chordName in NoteNames.Select(s => Parse.IgnoreCase(s.Key).Select(v => s.Value)).Aggregate((p1, p2) => p1.Or(p2))
            from accidental in Alterations.AccidentalParser.Optional()
            select Tuple.Create(chordName, accidental.GetOrElse(Accidental.None));
    }
}
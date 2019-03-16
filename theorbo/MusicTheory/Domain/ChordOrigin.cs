using System;
using System.Collections.Generic;
using theorbo.MusicTheory.Parsing;

namespace theorbo.MusicTheory.Domain
{
    public class ChordOrigin
    {
        public static ChordOrigin FromParseResults(NoteValue root,
            Accidental accidental,
            KnownChordKind knownChordKind,
            ChordExtensions.ExtensionBase extensionBase,
            IEnumerable<ChordExtensions.Extension> extensions,
            Tuple<NoteValue, Accidental> inversionOrBass) => new ChordOrigin
        {
            ChordKind = knownChordKind,
            Extensions = extensions,
            ExtensionBase = extensionBase,
            Root = new Note(root, accidental),
            BassNoteOrInversion =
                inversionOrBass != null ? new Note(inversionOrBass.Item1, inversionOrBass.Item2) : null
        };

        public Note Root { get; private set; }
        public KnownChordKind? ChordKind { get; private set; } = null;
        public ChordExtensions.ExtensionBase ExtensionBase { get; private set; }
        public IEnumerable<ChordExtensions.Extension> Extensions { get; private set; }
        public Note BassNoteOrInversion { get; private set; }

        public Chord BuildChord() => new Chord(this);
    }
}
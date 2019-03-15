using System;
using System.Collections.Generic;
using theorbo.MusicTheory.Parsing;

namespace theorbo.MusicTheory.Domain
{
    public class Chord
    {
        public Note RootNote { get; private set; }
        public KnownChordKind? ChordKind { get; private set; } = null;
        public ChordExtensions.ExtensionBase ExtensionBase { get; private set;}
        public IEnumerable<ChordExtensions.Extension> Extensions { get;private set; }
        public Note BassNoteOrInversion { get;private set; }

        public IList<Interval> Factors { get; private set; }

        public static Chord FromParseResults(NoteValue root,
            Accidental accidental,
            KnownChordKind knownChordKind,
            ChordExtensions.ExtensionBase extensionBase,
            IEnumerable<ChordExtensions.Extension> extensions,
            Tuple<NoteValue, Accidental> inversionOrBass)
        {
            var result = new Chord
            {
                ChordKind = knownChordKind,
                Extensions = extensions,
                ExtensionBase = extensionBase,
                RootNote = new Note(root,accidental),                
                BassNoteOrInversion = inversionOrBass !=null? new Note(inversionOrBass.Item1,inversionOrBass.Item2) : null               
            };

            result.AddFactors(extensions);

            return result;
        }

        private void AddFactors(IEnumerable<ChordExtensions.Extension> extensions)
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static partial class Chords
    {
        public struct ParsedChord : IEquatable<ParsedChord>
        {
            public bool Equals(ParsedChord other)
            {
                return NoteValue == other.NoteValue &&
                       Accidental == other.Accidental &&
                       ChordKind == other.ChordKind &&
                       ExtensionBase.Equals(other.ExtensionBase) &&
                       (Extensions?.SequenceEqual(other.Extensions) ??
                        ReferenceEquals(Extensions,
                            other.Extensions)) &&
                       Equals(BassNoteOrInversion, other.BassNoteOrInversion);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is ParsedChord chord && Equals(chord);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int) NoteValue;
                    hashCode = (hashCode * 397) ^ (int) Accidental;
                    hashCode = (hashCode * 397) ^ (int) ChordKind;
                    hashCode = (hashCode * 397) ^ ExtensionBase.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Extensions != null ? Extensions.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ BassNoteOrInversion.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(ParsedChord left, ParsedChord right) => 
                left.Equals(right);

            public static bool operator !=(ParsedChord left, ParsedChord right) => 
                !left.Equals(right);

            public NoteValue NoteValue { get; }
            public Accidental Accidental { get; }
            public KnownChordKind ChordKind { get; }
            public ChordExtensions.ExtensionBase ExtensionBase { get; }
            public IEnumerable<ChordExtensions.Extension> Extensions { get; }
            public Tuple<NoteValue, Accidental> BassNoteOrInversion { get; }

            public ParsedChord(NoteValue noteValue,
                Accidental accidental,
                KnownChordKind chordKind,
                ChordExtensions.ExtensionBase extensionBase,
                IEnumerable<ChordExtensions.Extension> extensions,
                Tuple<NoteValue, Accidental> bassNoteOrInversion)
            {
                NoteValue = noteValue;
                Accidental = accidental;
                ChordKind = chordKind;
                ExtensionBase = extensionBase;
                Extensions = extensions;
                BassNoteOrInversion = bassNoteOrInversion;
            }

            public override string ToString()
            {
                return $"{NoteValue} {Accidental} {ChordKind}; {ExtensionBase} {string.Join(", ",Extensions)} " +
                       $"{(BassNoteOrInversion!=null ? $"/{BassNoteOrInversion}" : string.Empty)}";
            }
        }
    }
}
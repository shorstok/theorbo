using System;

namespace theorbo.MusicTheory.Domain
{
    public class Note : IEquatable<Note>
    {
        public const int MiddleCMidi = 60;
        public const int C0Midi = 12;

        public int MidiValue { get; }

        public int Octave => (MidiValue - C0Midi) / Interval.SemitonesInOctave;
        public NoteValue NoteValue => (NoteValue) (MidiValue%Interval.SemitonesInOctave);

        public Note(NoteValue value, Accidental accidental = Accidental.None) => 
            MidiValue = (int) (MiddleCMidi + value + AccidentalToSemitones(accidental));

        public Note(NoteValue value, Accidental accidental, int octave) => 
            MidiValue = (int) (C0Midi + octave * Interval.SemitonesInOctave + value + AccidentalToSemitones(accidental));

        public Note(int midiNoteValue) => MidiValue = midiNoteValue;

        

        public static int AccidentalToSemitones(Accidental accidental)
        {
            switch (accidental)
            {
                case Accidental.None:
                    return 0;                  

                case Accidental.Sharp:
                    return 1;

                case Accidental.Flat:
                    return -1;
                case Accidental.DoubleSharp:
                    return 2;
                case Accidental.DoubleFlat:
                    return -2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accidental), accidental, null);
            }
        }
        
        public Note Transpose(int semitones) => 
            new Note(MidiValue + semitones);

        public Note UpAnOctave() => Transpose(Interval.SemitonesInOctave);

        public override string ToString()
        {
            return $"{NoteValue}{Octave}";
        }

        

        #region Equality

        public bool Equals(Note other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MidiValue == other.MidiValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Note) obj);
        }

        public override int GetHashCode() => MidiValue;

        public static bool operator ==(Note left, Note right) => Equals(left, right);
        public static bool operator !=(Note left, Note right) => !Equals(left, right);

        #endregion


    }
}
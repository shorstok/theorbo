using System;

namespace theorbo.MusicTheory.Domain
{
    public class Interval
    {
        public const int SemitonesInOctave = 12 * 2;

        public static Interval Unity { get; } = new Interval();

        public enum Quality
        {
            Perfect,
            Minor,
            Major,
            Diminished,
            Augmented
        }

        public int Semitones { get; }

        public Interval()
        {
            Semitones = 0;
        }

        public Interval(int semitones)
        {
            Semitones = semitones;
        }
        
        //todo: use major scale as reference
        public static Interval FromQuality(Quality quality, int value) => 
            throw new NotImplementedException();

        public static Interval FromChordExtension(int value, Accidental accidental) => 
            BuiltinScales.Mixolydian.GetScaleDegree(value, accidental);

        public static Interval FromSemitones(int semitones) => 
            new Interval ((semitones+SemitonesInOctave)%SemitonesInOctave);

        public Note ApplyTo(Note note)
        {
            return note.Transpose(Semitones);
        }

        public Interval ApplyAccidental(Accidental accidental) => 
            FromSemitones(Semitones + Note.AccidentalToSemitones(accidental));
    }
}
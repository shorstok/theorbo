using System;

namespace theorbo.MusicTheory.Domain
{
    public class Interval : IEquatable<Interval>
    {
        public const int SemitonesInOctave = 12;

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
              
        public static Interval FromQuality(Quality quality, int value)
        {
            var degree = BuiltinScales.Major.GetScaleDegree(value,Accidental.None);
 
            switch (quality)
            {
                case Quality.Major:
                case Quality.Perfect:
                    return degree;
                case Quality.Minor:
                    return degree.ApplyAccidental(Accidental.Flat);
                case Quality.Diminished:
                    return BuiltinScales.Major.GetScaleDegree(value-1, Accidental.None).
                        AddSemitones(value % 7 == 5 ? 1 : 0);
                case Quality.Augmented:
                    return degree.ApplyAccidental(Accidental.Sharp);
                default:
                    throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
            }
        }

        public Interval AddSemitones(int quantity)
        {
            return new Interval(Semitones + quantity);
        }

        public static Interval FromChordExtension(int value, Accidental accidental) => 
            BuiltinScales.Mixolydian.GetScaleDegree(value, accidental);

        public static Interval FromSemitones(int semitones) => 
            new Interval (semitones);

        public Note ApplyTo(Note note)
        {
            return note.Transpose(Semitones);
        }

        public Interval ApplyAccidental(Accidental accidental) => 
            FromSemitones(Semitones + Note.AccidentalToSemitones(accidental));


        #region Equality

        public bool Equals(Interval other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Semitones == other.Semitones;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Interval) obj);
        }

        public override int GetHashCode() => Semitones;
        public static bool operator ==(Interval left, Interval right) => Equals(left, right);
        public static bool operator !=(Interval left, Interval right) => !Equals(left, right);
        

        #endregion
    }
}
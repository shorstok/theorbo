using System;

namespace theorbo.MusicTheory.Domain
{
    public class Interval
    {
        public enum Quality
        {
            Perfect,
            Minor,
            Major,
            Diminished,
            Augmented
        }

        public int Semitones { get; set; }

        //todo: use major scale as reference
        public static Interval FromQuality(Quality quality, int value)
        {
            throw new NotImplementedException();
        }
    }
}
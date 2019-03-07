using System;

namespace theorbo.MusicTheory.Domain
{
    public class Note
    {
        public Accidental Accidental { get; set; }
        public NoteValue Value { get; set; }
        public int Octave { get; set; }

        public Interval IntervalBetween(Note other)
        {
            throw new NotImplementedException();
        }
    }
}
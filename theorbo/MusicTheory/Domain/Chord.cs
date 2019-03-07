using System.Collections.Generic;

namespace theorbo.MusicTheory.Domain
{
    public class Chord
    {
        public Note RootNote { get; set; }
        public IList<Interval> Factors { get; set; }
    }
}
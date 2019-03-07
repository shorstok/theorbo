using System.Collections.Generic;

namespace theorbo.MusicTheory.Domain
{
    public class ScaleDegree
    {
        public int Step { get; set; }
        public Accidental Accidental { get; set; }

        public Scale ReferenceScale { get; set; }
        public IList<Interval> Factors { get; set; }
    }
}
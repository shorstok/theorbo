using System;

namespace theorbo.MusicTheory.Domain
{
    public class Scale
    {
        public Interval[] Tones { get; set; }

        public int IanringScaleCode => throw new NotImplementedException();
    }
}
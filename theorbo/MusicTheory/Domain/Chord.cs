using System;
using System.Collections.Generic;
using System.Linq;

namespace theorbo.MusicTheory.Domain
{
    public class Chord : IEquatable<Chord>
    {
        public Note RootNote { get; }
        public IList<Interval> Factors { get; }

        public ChordOrigin Origin { get; } 

        public Chord(ChordOrigin origin)
        {
            Origin = origin;
            RootNote = origin.Root;


        }

        #region Equality

        public bool Equals(Chord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(RootNote, other.RootNote) && 
                   (ReferenceEquals(Factors, other.Factors) || (Factors?.SequenceEqual(other.Factors) ?? false));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Chord) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((RootNote != null ? RootNote.GetHashCode() : 0) * 397) ^ (Factors != null ? Factors.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Chord left, Chord right) => Equals(left, right);
        public static bool operator !=(Chord left, Chord right) => !Equals(left, right);

        #endregion

    }
}
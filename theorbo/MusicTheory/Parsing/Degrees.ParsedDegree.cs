using System;
using System.Collections.Generic;
using System.Linq;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static partial class Degrees
    {
        public struct ParsedDegree : IEquatable<ParsedDegree>
        {
            public bool Equals(ParsedDegree other)
            {
                return Accidental == other.Accidental &&
                       Degree == other.Degree &&
                       DegreeChordKind == other.DegreeChordKind &&
                       BaseExtension.Equals(other.BaseExtension) &&
                       Extensions?.SequenceEqual(other.Extensions) == true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is ParsedDegree && Equals((ParsedDegree) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int) Accidental;
                    hashCode = (hashCode * 397) ^ Degree;
                    hashCode = (hashCode * 397) ^ (int) DegreeChordKind;
                    hashCode = (hashCode * 397) ^ BaseExtension.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Extensions != null ? Extensions.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(ParsedDegree left, ParsedDegree right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ParsedDegree left, ParsedDegree right)
            {
                return !left.Equals(right);
            }

            public ParsedDegree(Accidental accidental,
                int degree,
                KnownChordKind degreeChordKind,
                ChordExtensions.ExtensionBase baseExtension,
                IEnumerable<ChordExtensions.Extension> extensions)
            {
                Accidental = accidental;
                Degree = degree;
                DegreeChordKind = degreeChordKind;
                BaseExtension = baseExtension;
                Extensions = extensions;
            }

            public Accidental Accidental { get; }
            public int Degree { get; }
            public KnownChordKind DegreeChordKind { get; }
            public ChordExtensions.ExtensionBase BaseExtension { get; }
            public IEnumerable<ChordExtensions.Extension> Extensions { get; }

            public override string ToString()
            {
                return $"{nameof(Accidental)}: {Accidental}; " +
                       $"{nameof(Degree)}: {Degree}; " +
                       $"{nameof(DegreeChordKind)}: {DegreeChordKind}; " +
                       $"{nameof(BaseExtension)}: {BaseExtension}; " +
                       $"{nameof(Extensions)}: {string.Join(", ",Extensions)}";
            }
        }

        
    }
}
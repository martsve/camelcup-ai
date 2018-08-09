using System;

namespace Delver.CamelCup
{
    public struct Position 
    {
        public int Location { get; set; }
        public int Height { get; set; }
        
        public Position(int location, int height)
        {
            Location = location;
            Height = height;
        }

        public override string ToString()
        {
            return $"Pos({Location}, {Height})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                var pos = (Position)obj;
                return pos.Height == Height && pos.Location == Location;
            }

            return false;
        }

        public static bool operator ==(Position A, Position B)
        {
            return A.Equals(B);
        }

        public static bool operator !=(Position A, Position B)
        {
            return !A.Equals(B);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Location.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }
    }
}

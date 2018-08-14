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
            return $"{Location},{Height}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Position pos)
            {
                return pos.Height == Height && pos.Location == Location;
            }

            return false;
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Location.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }
    }
}

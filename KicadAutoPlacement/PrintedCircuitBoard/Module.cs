using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement
{
    public class Module: IEquatable<Module>
    {
        public string Name { get; set; }//
        public List<Pad> Pads;
        public Point LeftUpperBound { get; set; }//
        public Point RighLowerBound { get; set; }//
        public Point Position { get; set; }//
        private double _rotate;
        public double Rotate
        {
            get { return _rotate; }
            set { _rotate = value % 360; }
        } //

        public string Path { get; set; }//
        public Module(string name)
        {
            Name = name;
            Position = new Point(0, 0);
            Rotate = 0;
            LeftUpperBound = new Point(0,0);
            RighLowerBound = new Point(0,0);
            Pads = new List<Pad>();
        }
        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Module other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _rotate.Equals(other._rotate) && string.Equals(Name, other.Name) && Equals(Position, other.Position) && string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Module) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _rotate.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Position != null ? Position.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}

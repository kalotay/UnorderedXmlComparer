using System;
using System.Linq;

namespace XmlComparer
{
    public class FlatXmlNode: IComparable<FlatXmlNode>
    {
        public int Depth;
        public int Position;

        public string Name;
        public string Value;
        private readonly Func<FlatXmlNode, int>[] _compares;

        public FlatXmlNode()
        {
            _compares = new Func<FlatXmlNode, int>[]
                {
                    DepthCompare,
                    NameCompare,
                    ValueCompare,
                    PositionCompare
                };
        }

        public int CompareTo(FlatXmlNode other)
        {
            if (other == null)
            {
                return -1;
            }

            return _compares.Select(compare => compare(other)).FirstOrDefault(compareResult => compareResult != 0);
        }

        public int DepthCompare(FlatXmlNode other)
        {
            return Depth.CompareTo(other.Depth);
        }

        public int PositionCompare(FlatXmlNode other)
        {
            return Position.CompareTo(other.Position);
        }

        public int NameCompare(FlatXmlNode other)
        {
            return string.CompareOrdinal(Name, other.Name);
        }

        public int ValueCompare(FlatXmlNode other)
        {
            return string.CompareOrdinal(Value, other.Value);
        }

    }
}
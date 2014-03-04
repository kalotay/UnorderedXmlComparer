using System;
using System.Collections.Generic;

namespace UnorderedXmlComparer
{
    public class FlatXml: IEquatable<FlatXml>
    {
        private readonly SortedSet<FlatXmlNode> _nodes;

        public FlatXml(SortedSet<FlatXmlNode> nodes)
        {
            _nodes = nodes;
        }

        public bool Equals(FlatXml other)
        {
            if (other == null)
            {
                return false;
            }

            if (_nodes.Count != other._nodes.Count)
            {
                return false;
            }

            using (var thisEnumerator = _nodes.GetEnumerator())
            using (var otherEnumerator = other._nodes.GetEnumerator())
            {
                while (thisEnumerator.MoveNext())
                {
                    otherEnumerator.MoveNext();
                    var thisCurrent = thisEnumerator.Current;
                    var otherCurrent = otherEnumerator.Current;
                    if (thisCurrent == null)
                    {
                        return otherCurrent == null;
                    }

                    if (thisCurrent.DepthCompare(otherCurrent) != 0)
                    {
                        return false;
                    }

                    if (thisCurrent.NameCompare(otherCurrent) != 0)
                    {
                        return false;
                    }

                    if (thisCurrent.ValueCompare(otherCurrent) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
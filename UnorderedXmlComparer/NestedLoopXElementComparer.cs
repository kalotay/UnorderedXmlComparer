using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UnorderedXmlComparer
{
    public class NestedLoopXElementComparer : IEqualityComparer<XElement>
    {
        public bool Equals(XElement a, XElement b)
        {
            if (XNode.DeepEquals(a, b))
            {
                return true;
            }

            if (!a.HasElements)
            {
                return false;
            }

            if (!b.HasElements)
            {
                return false;
            }

            var aElements = a.Elements().ToList();
            var bElements = b.Elements().ToList();

            if (aElements.Count != bElements.Count)
            {
                return false;
            }

            var commonCount = 0;
            foreach (var aElement in aElements)
            {
                foreach (var bElement in bElements)
                {
                    if (Equals(aElement, bElement))
                    {
                        commonCount += 1;
                    }
                }
            }

            return commonCount == aElements.Count;
        }

        public int GetHashCode(XElement obj)
        {
            return obj.GetHashCode();
        }
    }
}
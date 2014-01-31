using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XmlComparer
{
    public class SortMergeXElementComparer : IEqualityComparer<XElement>
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
            var bElements = a.Elements().ToList();


            if (aElements.Count != bElements.Count)
            {
                return false;
            }

            aElements.Sort();
            bElements.Sort();

            for (var i = 0; i < aElements.Count; i++)
            {
                var aElement = aElements[i];
                var bElement = bElements[i];
                if (!Equals(aElement, bElement))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(XElement obj)
        {
            return obj.GetHashCode();
        }
    }
}
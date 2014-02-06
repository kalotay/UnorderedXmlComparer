using System.Collections.Generic;
using System.Linq;

namespace XmlComparer
{
    public class Element: IXmlNode
    {
        public ICollection<IXmlNode> Children { get; private set; }
        public string Name { get; set; }

        public Element()
        {
            Children = new List<IXmlNode>();
        }

        public override string ToString()
        {
            return GetType().Name + ": " + GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Element;
            if (other == null)
            {
                return false;
            }

            if (GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            if (Children.Count != other.Children.Count)
            {
                return false;
            }

            using (var thisSorted = Children.OrderBy(node => node.GetHashCode()).GetEnumerator())
            {
                using (var otherSorted = Children.OrderBy(node => node.GetHashCode()).GetEnumerator())
                {
                    for (var i = 0; i < Children.Count; i++)
                    {
                        if (thisSorted.MoveNext() != otherSorted.MoveNext())
                        {
                            return false;
                        }
                        if (thisSorted.Current.GetHashCode() != otherSorted.Current.GetHashCode())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Children.Aggregate(Name.GetHashCode(), (current, xmlNode) => current ^ xmlNode.GetHashCode());
        }
    }
}
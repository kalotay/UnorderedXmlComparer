namespace XmlComparer
{
    public class Attribute: IXmlNode
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return GetType().Name + ": " + Name + "=" + Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Value == null && Name == null;
            }

            var other = obj as Attribute;
            if (other == null)
            {
                return false;
            }

            return Value == other.Value && Name == other.Name;

        }

        public override int GetHashCode()
        {
            return (Name + Value).GetHashCode();
        }
    }
}
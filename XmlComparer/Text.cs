namespace XmlComparer
{
    public class Text: IXmlNode
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return GetType().Name + ": " + Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Value == null;
            }

            var other = obj as Text;
            if (other == null)
            {
                return false;
            }

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
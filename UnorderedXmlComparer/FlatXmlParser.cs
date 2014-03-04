using System.Collections.Generic;
using System.Xml;

namespace UnorderedXmlComparer
{
    public class FlatXmlParser
    {
        public FlatXml Parse(XmlReader reader)
        {
            var nodes = new SortedSet<FlatXmlNode>();
            var position = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var element = new FlatXmlNode {Name = reader.Name, Depth = reader.Depth, Position = position};
                        nodes.Add(element);
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                position += 1;
                                var attribute = new FlatXmlNode {Name = reader.Name, Value = reader.Value, Position = position, Depth = reader.Depth};
                                nodes.Add(attribute);
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        var t = new FlatXmlNode {Value = reader.Value, Depth = reader.Depth, Position = position};
                        nodes.Add(t);
                        break;
                }
                position += 1;
            }
            return new FlatXml(nodes);
        }
    }
}
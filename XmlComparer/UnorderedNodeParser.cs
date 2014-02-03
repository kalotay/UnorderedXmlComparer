using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmlComparer
{
    public class UnorderedNodeParser
    {
        public Element Parse(XmlReader reader)
        {
            var root = new Element();
            var elementStack = new Stack<Element>();
            elementStack.Push(root);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var element = new Element {Name = reader.Name};
                        var isEmpty = reader.IsEmptyElement;
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                var attribute = new Attribute {Name = reader.Name, Value = reader.Value};
                                element.Children.Add(attribute);
                            }
                        }
                        elementStack.Peek().Children.Add(element);
                        if (!isEmpty)
                        {
                            elementStack.Push(element);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        elementStack.Pop();
                        break;
                    case XmlNodeType.Text:
                        var t = new Text {Value = reader.Value};
                        elementStack.Peek().Children.Add(t);
                        break;
                }
            }
            return (Element)root.Children.First();
        }
    }
}
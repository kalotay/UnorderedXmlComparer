using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UnorderedXmlComparer
{
    public class XmlFuzzer
    {
        public XDocument Fuzz(XDocument document)
        {
            var root = document.Root;
            var fuzzedRoot = Fuzz(root);
            var fuzzedDoc = new XDocument(fuzzedRoot);
            return fuzzedDoc;
        }

        public XElement Fuzz(XElement element)
        {
            var name = element.Name;
            var fuzzedElement = new XElement(name);

            if (element.HasAttributes)
            {
                var shuffledAttributes = Shuffle(element.Attributes().ToList());
                foreach (var attribute in shuffledAttributes)
                {
                    fuzzedElement.SetAttributeValue(attribute.Name, attribute.Value);
                }
            }

            if (element.HasElements)
            {
                var shuffledElements = Shuffle(element.Elements().ToList());
                foreach (var fuzzed in shuffledElements.Select(Fuzz))
                {
                    fuzzedElement.Add(fuzzed);
                }
            }
            else
            {
                fuzzedElement.Value = element.Value;
            }

            return fuzzedElement;
        }

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            var rng = new Random();
            var shuffled = new List<T>(list);
            for (var i = 1; i < list.Count; i++)
            {
                var randomInt = rng.Next(0, i + 1);
                if (randomInt != i)
                {
                    shuffled[i] = shuffled[randomInt];
                }
                shuffled[randomInt] = list[i];
            }
            return shuffled;
        }
    }
}
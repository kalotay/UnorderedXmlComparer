using System.IO;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace XmlComparer
{
    [TestFixture]
    public class TheTests
    {
        private const string XmlA = @"<report><bodyCollection>
<item currency=""EUR"" amount = ""1.0""/>
<t>hello</t>
<item currency=""GBP"" amount = ""2.0""/>
</bodyCollection></report>";

        private const string XmlB = @"<report><bodyCollection>
<item currency=""GBP"" amount = ""2.0""/>
<item currency=""EUR"" amount = ""1.0""/>
<t>hello</t>
</bodyCollection></report>";

        private const string XmlC = @"<report><bodyCollection>
<item currency=""GBP"" amount = ""2.0""/>
<item currency=""EUR"" amount = ""3.0""/>
<t>hello</t>
</bodyCollection></report>";

        [Test]
        public void FuzzingCausesDeepEqualsToFail()
        {
            var xml = XDocument.Parse(XmlA);
            var fuzzed = new XmlFuzzer().Fuzz(xml);
            Assert.That(XNode.DeepEquals(xml.Root, fuzzed.Root), Is.False);
        }

        [Test]
        public void FuzzingCausesNestedLoopEqualToFailDueToAttributeReordering()
        {
            var xml = XDocument.Parse(XmlA);
            var fuzzed = new XmlFuzzer().Fuzz(xml);
            var nestedLoopComparer = new NestedLoopXElementComparer();
            Assert.That(nestedLoopComparer.Equals(xml.Root, fuzzed.Root), Is.False);
        }

        [Test]
        public void FuzzingHasNoEffectOnUnorderedNodeParser()
        {
            var xml = XDocument.Parse(XmlA);
            var fuzzed = new XmlFuzzer().Fuzz(xml);
            var parser = new UnorderedNodeParser();
            using (var reader = xml.CreateReader())
            {
                var source = parser.Parse(reader);
                using (var fuzzedReader = fuzzed.CreateReader())
                {
                    var fuzz = parser.Parse(fuzzedReader);
                    Assert.That(source, Is.EqualTo(fuzz));
                }
            }
        }

        [Test]
        public void FuzzingHasNoEffectOnFlatXmlParser()
        {
            var xml = XDocument.Parse(XmlA);
            var fuzzed = new XmlFuzzer().Fuzz(xml);
            var parser = new FlatXmlParser();
            using (var reader = xml.CreateReader())
            {
                var source = parser.Parse(reader);
                using (var fuzzedReader = fuzzed.CreateReader())
                {
                    var fuzz = parser.Parse(fuzzedReader);
                    Assert.That(source, Is.EqualTo(fuzz));
                }
            }
        }

        [Test]
        public void XNodeDeepEqualsWorksAsExpected()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlB = XDocument.Parse(XmlB);
            Assert.That(XNode.DeepEquals(xmlA.Root, xmlB.Root), Is.False);
        }

        [Test]
        public void NestedLoopDeepEqualsWorksAsExpectedWhenElementsAreReordered()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlB = XDocument.Parse(XmlB);
            Assert.That(new NestedLoopXElementComparer().Equals(xmlA.Root, xmlB.Root), Is.True);
        }

        [Test]
        public void NestedLoopDeepEqualsWorksAsExpectedWhenXmlIsDifferent()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlC = XDocument.Parse(XmlC);
            Assert.That(new NestedLoopXElementComparer().Equals(xmlA.Root, xmlC.Root), Is.False);
        }

        [Test]
        public void ParserComparesEqual()
        {
            var parser = new UnorderedNodeParser();
            var xmlA = parser.Parse(XmlReader.Create(new StringReader(XmlA)));
            var xmlB = parser.Parse(XmlReader.Create(new StringReader(XmlB)));
            Assert.That(xmlA, Is.EqualTo(xmlB));
        }

        [Test]
        public void ParserComparesUnEqual()
        {
            var parser = new UnorderedNodeParser();
            var xmlA = parser.Parse(XmlReader.Create(new StringReader(XmlA)));
            var xmlC = parser.Parse(XmlReader.Create(new StringReader(XmlC)));
            Assert.That(xmlA, Is.Not.EqualTo(xmlC));
        }

        [Test]
        public void FlatXmlParserComparesEqual()
        {
            var parser = new FlatXmlParser();
            var xmlA = parser.Parse(XmlReader.Create(new StringReader(XmlA)));
            var xmlB = parser.Parse(XmlReader.Create(new StringReader(XmlB)));
            Assert.That(xmlA, Is.EqualTo(xmlB));
        }

        [Test]
        public void FlatParserComparesUnEqual()
        {
            var parser = new FlatXmlParser();
            var xmlA = parser.Parse(XmlReader.Create(new StringReader(XmlA)));
            var xmlC = parser.Parse(XmlReader.Create(new StringReader(XmlC)));
            Assert.That(xmlA, Is.Not.EqualTo(xmlC));
        }


        public static bool Equals(XElement a, XElement b)
        {
            return new NestedLoopXElementComparer().Equals(a, b);
        }
    }
}

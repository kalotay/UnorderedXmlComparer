using System.IO;
using System.Xml;
using NUnit.Framework;

namespace XmlComparer
{
    [TestFixture]
    public class UnorderedNodeParserEdgeCases
    {
                private const string XmlA = @"<report><bodyCollection>
<item currency=""EUR"" amount = ""1.0""/>
<t>hello</t>
<item currency=""GBP"" amount = ""2.0""/>
</bodyCollection></report>";

        private const string XmlB = @"<report><collection>
<item currency=""GBP"" amount = ""2.0""/>
<item currency=""EUR"" amount = ""1.0""/>
<t>hello</t>
</collection></report>";

        private const string XmlC = @"<report><bodyCollection>
<item currency=""EUR"" amount = ""1.0""/>
<item currency=""EUR"" amount = ""1.0""/>
<t>hello</t>
<item currency=""GBP"" amount = ""2.0""/>
<item currency=""EUR"" amount = ""1.0""/>
</bodyCollection></report>";


        private UnorderedNodeParser _unorderedNodeParser;
        private FlatXmlParser _flatXmlParser;

        [SetUp]
        public void SetUp()
        {
            _unorderedNodeParser = new UnorderedNodeParser();
            _flatXmlParser = new FlatXmlParser();
        }

        [Test]
        public void ElementsWithDifferentNameShouldNotCompareEqual()
        {
            using (var readerA = new StringReader(XmlA))
            using (var readerB = new StringReader(XmlB))
            using (var xmlReaderA = XmlReader.Create(readerA))
            using (var xmlReaderB = XmlReader.Create(readerB))
            {
                var a = _unorderedNodeParser.Parse(xmlReaderA);
                var b = _unorderedNodeParser.Parse(xmlReaderB);
                Assert.That(a, Is.Not.EqualTo(b));
            }
        }

        [Test]
        public void RepeatedElementsCauseNoProblem()
        {
            using (var readerA = new StringReader(XmlA))
            using (var readerC = new StringReader(XmlC))
            using (var xmlReaderA = XmlReader.Create(readerA))
            using (var xmlReaderC = XmlReader.Create(readerC))
            {
                var a = _unorderedNodeParser.Parse(xmlReaderA);
                var c = _unorderedNodeParser.Parse(xmlReaderC);
                Assert.That(a, Is.Not.EqualTo(c));
            }
        }

        [Test]
        public void ElementsWithDifferentNameShouldNotCompareEqualForFlatXml()
        {
            using (var readerA = new StringReader(XmlA))
            using (var readerB = new StringReader(XmlB))
            using (var xmlReaderA = XmlReader.Create(readerA))
            using (var xmlReaderB = XmlReader.Create(readerB))
            {
                var a = _flatXmlParser.Parse(xmlReaderA);
                var b = _flatXmlParser.Parse(xmlReaderB);
                Assert.That(a, Is.Not.EqualTo(b));
            }
        }

        [Test]
        public void RepeatedElementsCauseNoProblemForFlatXml()
        {
            using (var readerA = new StringReader(XmlA))
            using (var readerC = new StringReader(XmlC))
            using (var xmlReaderA = XmlReader.Create(readerA))
            using (var xmlReaderC = XmlReader.Create(readerC))
            {
                var a = _flatXmlParser.Parse(xmlReaderA);
                var c = _flatXmlParser.Parse(xmlReaderC);
                Assert.That(a, Is.Not.EqualTo(c));
            }
        }
    }
}
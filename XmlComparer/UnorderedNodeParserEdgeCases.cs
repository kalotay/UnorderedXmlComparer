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

        private UnorderedNodeParser _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new UnorderedNodeParser();
        }

        [Test]
        public void ElementsWithDifferentNameShouldNotCompareEqual()
        {
            using (var readerA = new StringReader(XmlA))
            using (var readerB = new StringReader(XmlB))
            using (var xmlReaderA = XmlReader.Create(readerA))
            using (var xmlReaderB = XmlReader.Create(readerB))
            {
                var a = _subject.Parse(xmlReaderA);
                var b = _subject.Parse(xmlReaderB);
                Assert.That(a, Is.Not.EqualTo(b));
            }
        }
    }
}
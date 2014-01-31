using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace XmlComparer
{
    [TestFixture]
    public class TheTests
    {
        private const string XmlA = @"<report><bodyCollection>
<item currency=""EUR"" amount = ""1.0""/>
<item currency=""GBP"" amount = ""2.0""/>
</bodyCollection></report>";

        private const string XmlB = @"<report><bodyCollection>
<item currency=""GBP"" amount = ""2.0""/>
<item currency=""EUR"" amount = ""1.0""/>
</bodyCollection></report>";

        [Test]
        public void XNodeDeepEqualsWorksAsExpected()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlB = XDocument.Parse(XmlB);
            Assert.That(XNode.DeepEquals(xmlA.Root, xmlB.Root), Is.False);
        }

        [Test]
        public void NestedLoopDeepEqualsWorksAsExpected()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlB = XDocument.Parse(XmlB);
            Assert.That(new NestedLoopXElementComparer().Equals(xmlA.Root, xmlB.Root), Is.True);
        }

        [Test]
        public void SortMergeDeepEqualsWorksAsExpected()
        {
            var xmlA = XDocument.Parse(XmlA);
            var xmlB = XDocument.Parse(XmlB);
            Assert.That(new SortMergeXElementComparer().Equals(xmlA.Root, xmlB.Root), Is.True);
        }

        public static bool Equals(XElement a, XElement b)
        {
            return new NestedLoopXElementComparer().Equals(a, b);
        }
    }
}

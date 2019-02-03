using NUnit.Framework;

namespace SharpToml.Tests
{
    public class BasicTests
    {
        [Test]
        public void TestEmptyComment()
        {
            var input = "#\n";
            var doc = Toml.Parse(input);
            var docAsStr = doc.ToString();
            Assert.AreEqual(input, docAsStr);
        }

        [Test]
        public void SimpleTest()
        {
            var test = @"[table-1]
key1 = ""some string""    # This is a comment
key2 = 123
Key3 = true
Key4 = false
Key5 = +inf

[table-2]
key1 = ""another string""
key2 = 456
";
            var doc = Toml.Parse(test);
            Assert.AreEqual(test, doc.ToString());
        }
    }
}
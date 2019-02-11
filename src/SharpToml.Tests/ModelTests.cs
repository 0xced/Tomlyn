// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.

using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using SharpToml.Model;

namespace SharpToml.Tests
{
    public class ModelTests
    {
        [Test]
        public static void TestPrimitives()
        {
            var input = @"a = 1
b = true
c = 1.0
d = 'yo'
e = 1980-01-20
";
            var syntax = Toml.Parse(input);
            Assert.False(syntax.HasErrors, "The document should not have any errors");

            var model = syntax.ToModel();

            Assert.True(model.ContainsKey("a"));
            Assert.True(model.ContainsKey("b"));
            Assert.True(model.ContainsKey("c"));
            Assert.True(model.ContainsKey("d"));
            Assert.True(model.ContainsKey("e"));

            Assert.AreEqual(5, model.Count);
            Assert.AreEqual((long)1, model["a"]);
            Assert.AreEqual(true, model["b"]);
            Assert.AreEqual(1.0, model["c"]);
            Assert.AreEqual("yo", model["d"]);
            Assert.AreEqual(new DateTime(1980, 01, 20), model["e"]);
        }

        [Test]
        public static void TestBasic()
        {
            var input = @"a = 1
b = true
c.d = 'yo'
";
            var syntax = Toml.Parse(input);
            Assert.False(syntax.HasErrors, "The document should not have any errors");

            var model = syntax.ToModel();

            Assert.True(model.ContainsKey("a"));
            Assert.True(model.ContainsKey("b"));
            Assert.True(model.ContainsKey("c"));
            Assert.AreEqual(3, model.Count);
            Assert.AreEqual((long)1, model["a"]);
            Assert.AreEqual(true, model["b"]);
            Assert.IsInstanceOf<TomlTable>(model["c"]);
            var subTable = ((TomlTable) model["c"]);
            Assert.True(subTable.ContainsKey("d"));
            Assert.AreEqual(1, subTable.Count);
            Assert.AreEqual("yo", subTable["d"]);
        }

        [Test]
        public static void TestTable()
        {
            var input = @"a = 1
[b]
c = 1
d = true
";
            var syntax = Toml.Parse(input);
            Assert.False(syntax.HasErrors, "The document should not have any errors");

            var model = syntax.ToModel();

            Assert.True(model.ContainsKey("a"));
            Assert.True(model.ContainsKey("b"));
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual((long)1, model["a"]);
            Assert.IsInstanceOf<TomlTable>(model["b"]);
            var subTable = ((TomlTable)model["b"]);
            Assert.True(subTable.ContainsKey("c"));
            Assert.True(subTable.ContainsKey("d"));
            Assert.AreEqual(2, subTable.Count);
            Assert.AreEqual((long)1, subTable["c"]);
            Assert.AreEqual((bool)true, subTable["d"]);
        }


        [Test]
        public static void TestTableArray()
        {
            var input = @"a = 1
[[b]]
a = 1
b = true
[[b]]
c = 1
d = true
";
            var syntax = Toml.Parse(input);
            Assert.False(syntax.HasErrors, "The document should not have any errors");

            var model = syntax.ToModel();

            Assert.True(model.ContainsKey("a"));
            Assert.True(model.ContainsKey("b"));
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual((long)1, model["a"]);
            Assert.IsInstanceOf<TomlTableArray>(model["b"]);
            var tableArray = ((TomlTableArray)model["b"]);
            Assert.AreEqual(2, tableArray.Count);

            // first [[b]]
            Assert.IsInstanceOf<TomlTable>(tableArray[0]);
            var bTable0 = tableArray[0];
            Assert.True(bTable0.ContainsKey("a"));
            Assert.True(bTable0.ContainsKey("b"));
            Assert.AreEqual((long)1, bTable0["a"]);
            Assert.AreEqual((bool)true, bTable0["b"]);

            // second [[b]]
            Assert.IsInstanceOf<TomlTable>(tableArray[1]);
            var bTable1 = tableArray[1];
            Assert.True(bTable1.ContainsKey("c"));
            Assert.True(bTable1.ContainsKey("d"));
            Assert.AreEqual((long)1, bTable1["c"]);
            Assert.AreEqual((bool)true, bTable1["d"]);
        }

        [Test]
        public static void TestNestedTableArray()
        {
            var input = @"[[fruit]]
  name = 'apple'

  [fruit.physical]
    color = 'red'
    shape = 'round'

  [[fruit.variety]]
    name = 'red delicious'

  [[fruit.variety]]
    name = 'granny smith'

[[fruit]]
  name = 'banana'

  [[fruit.variety]]
    name = 'plantain'
";

            var json = @"{
  ""fruit"": [
    {
      ""name"": ""apple"",
      ""physical"": {
        ""color"": ""red"",
        ""shape"": ""round""
      },
      ""variety"": [
        {
          ""name"": ""red delicious""
        },
        {
          ""name"": ""granny smith""
        }
      ]
    },
    {
      ""name"": ""banana"",
      ""variety"": [
        {
          ""name"": ""plantain""
        }
      ]
    }
  ]
}";
            AssertJson(input, json);
        }


        [Test]
        public void TestArrayAndInline()
        {
            var input = @"points = [ { x = 1, y = 2, z = 3 },
           { x = 7, y = 8, z = 9 },
           { x = 2, y = 4, z = 8 } ]";

            var json = @"{
  ""points"": [
    {
      ""x"": 1,
      ""y"": 2,
      ""z"": 3
    },
    {
      ""x"": 7,
      ""y"": 8,
      ""z"": 9
    },
    {
      ""x"": 2,
      ""y"": 4,
      ""z"": 8
    }
  ]
}";
            AssertJson(input, json);
        }

        [Test]
        public void TestImplicitAndExplicitTable()
        {
            var input = @"[a.b.c]
answer = 42

[a]
better = 43
";
            var json = @"{
  ""a"": {
    ""b"": {
      ""c"": {
        ""answer"": 42
      }
    },
    ""better"": 43
  }
}";
            AssertJson(input, json);
        }

        private static void AssertJson(string input, string expectedJson)
        {
            var syntax = Toml.Parse(input);
            Assert.False(syntax.HasErrors, "The document should not have any errors");

            StandardTests.Dump(input, syntax, syntax.ToString());

            var model = syntax.ToModel();

            var serializer = JsonSerializer.Create(new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var writer = new StringWriter();
            serializer.Serialize(writer, model);
            var jsonResult = NormalizeEndOfLine(writer.ToString());
            expectedJson = NormalizeEndOfLine(expectedJson);

            StandardTests.DisplayHeader("json");
            Console.WriteLine(jsonResult);

            Assert.AreEqual(expectedJson, jsonResult);
        }

        private static string NormalizeEndOfLine(string text)
        {
            return text.Replace("\r\n", "\n");
        }


    }
}
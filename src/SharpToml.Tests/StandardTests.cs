// Copyright (c) 2019 - Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace SharpToml.Tests
{
    public class StandardTests
    {
        private const string RelativeTomlTestsDirectory = @"../../../../../ext/toml-test/tests";

        [TestCaseSource("ListTomlFiles", new object[] { "valid" }, Category = "toml-test")]
        public static void CheckValid(string inputName, string toml, string json)
        {
            var doc = Toml.Parse(toml);

            foreach (var syntaxMessage in doc.Messages)
            {
                Console.WriteLine(syntaxMessage);
            }

            Assert.False(doc.HasErrors, "Unexpected parsing errors");

            var docAsStr = doc.ToString();


            Console.WriteLine();
            DisplayHeader("input");
            Console.WriteLine(toml);

            Console.WriteLine();
            DisplayHeader("round-trip");
            Console.WriteLine(docAsStr);


            Assert.AreEqual(toml, docAsStr, "The roundtrip doesn't match");
            // TODO: Add tests for 
        }

        private static void DisplayHeader(string name)
        {
            Console.WriteLine($"// ----------------------------------------------------------");
            Console.WriteLine($"// {name}");
            Console.WriteLine($"// ----------------------------------------------------------");
        }

        public static IEnumerable ListTomlFiles(string type)
        {
            var directory = Path.GetFullPath(Path.Combine(BaseDirectory, RelativeTomlTestsDirectory, type));

            Assert.True(Directory.Exists(directory), $"The folder `{directory}` does not exist");

            var tests = new List<TestCaseData>();
            foreach (var file in Directory.EnumerateFiles(directory, "*.toml", SearchOption.AllDirectories))
            {
                var functionName = Path.GetFileName(file);

                var input = File.ReadAllText(file);

                string json = null;
                if (type == "valid")
                {
                    var jsonFile = Path.ChangeExtension(file, "json");
                    Assert.True(File.Exists(jsonFile), $"The json file `{jsonFile}` does not exist");
                    json = File.ReadAllText(jsonFile);
                }
                tests.Add(new TestCaseData(functionName, input, json));
            }
            return tests;
        }

        private static string BaseDirectory
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var codebase = new Uri(assembly.CodeBase);
                var path = codebase.LocalPath;
                return Path.GetDirectoryName(path);
            }
        }
    }
}
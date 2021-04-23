using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuidelineXmlToMD.Test
{
    [TestClass]
    public class GuidelineXmlFileReaderTest
    {
        [TestMethod]
        public void ReadExisitingGuidelinesFile_ExistingFile_ReadsGuidlines()
        {
            // Arrange
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var testPath = Path.Combine(projectPath, @"Data\", "TestGuidelines.xml");

            // Act
            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(testPath);

            // Assert
            var actual = guidelines.Single();
            var expected = new Guideline("name", "My name is Inigo Montoya.", "CONSIDER", "Fencing", "Who");
            Assert.AreEqual(expected, actual);

        }
    }
}

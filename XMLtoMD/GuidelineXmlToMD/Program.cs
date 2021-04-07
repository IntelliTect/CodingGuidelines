using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MarkdownOut;


namespace GuidelineXmlToMD
{
    class Program
    {
        /// <summary>
        /// A simple tool to convert coding guidelines xml to the website formatted markdown.
        /// </summary>
        /// <param name="xmlInputFile">The xml file to process.</param>
        /// <param name="markdownOutputFile">The md file to create.</param> 
        /// <param name="console">Injected by System.CommandLine</param>
        static void Main(FileInfo xmlInputFile, FileInfo markdownOutputFile, IConsole console)
        {
            if (xmlInputFile is null)
            {
                throw new ArgumentNullException(nameof(xmlInputFile));
            }

            if (markdownOutputFile is null)
            {
                throw new ArgumentNullException(nameof(markdownOutputFile));
            }

            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }


            if (!xmlInputFile.Exists)
            {
                throw new FileNotFoundException("Could not find input file", xmlInputFile.FullName);
            }

            string markDownOutputFilePath = markdownOutputFile.FullName;
            string xmlInputFilePath = xmlInputFile.FullName;

            console.Out.WriteLine($"Converting {xmlInputFilePath} to {markDownOutputFilePath}");

            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(xmlInputFilePath);

            using var mdWriter = new MdWriter(markDownOutputFilePath);
            mdWriter.WriteLine("C# Guidelines", format: MdFormat.Heading1);
            PrintSections(guidelines, mdWriter, console);
            mdWriter.WriteLine("Guidelines", format: MdFormat.Heading1);
            mdWriter.WriteLine("");
            PrintGuidelinesBySection(guidelines, mdWriter, console);
        }

        private static void PrintGuidelinesBySection(
            ICollection<Guideline> guidelines, 
            MdWriter mdWriter,
            IConsole console)
        {
            foreach (string section in GetSections(guidelines))
            {
                mdWriter.WriteLine("");
                console.Out.WriteLine(section);
                mdWriter.WriteLine(section, format: MdFormat.Heading2, style: MdStyle.BoldItalic);

                IEnumerable<Guideline> guidelinesInSections = (from guideline in guidelines
                                                               where string.Equals(guideline.Section, section, StringComparison.OrdinalIgnoreCase)
                                                               select guideline);

                foreach (string subsection in GetSubSections(guidelines))
                {

                    IEnumerable<Guideline> guidelinesInSubsection = (from guideline in guidelinesInSections
                                                                     where string.Equals(guideline.Subsection, subsection, StringComparison.OrdinalIgnoreCase)
                                                                     select guideline);

                    if (guidelinesInSubsection.Any())
                    {
                        console.Out.WriteLine($"     { subsection}");
                        mdWriter.WriteLine(subsection, format: MdFormat.Heading3, numNewLines: 1);

                        foreach (Guideline guideline in guidelinesInSubsection)
                        {
                            mdWriter.WriteUnorderedListItem(GetGuidelineEmoji(guideline) + " " + guideline.Text.Trim('"'), listIndent: 0);
                        }
                    }
                    mdWriter.WriteLine("", numNewLines: 1);

                }
            }
        }

        private static string GetGuidelineEmoji(Guideline guideline)
        {
            string emoji = "";
            switch (guideline.Severity)
            {
                case "AVOID":
                    emoji = ":no_entry:";
                    break;
                case "DO NOT":
                    emoji = ":x:";
                    break;
                case "DO":
                    emoji = ":heavy_check_mark:";
                    break;
                case "CONSIDER":
                    emoji = ":grey_question:";
                    break;

                default:
                    break;
            }
            return emoji;
        }

        private static void PrintSections(
            ICollection<Guideline> guidelines,
            MdWriter mdWriter,
            IConsole console)
        {
            mdWriter.WriteLine("Sections", format: MdFormat.Heading2);

            List<string> subSections = new List<string>();

            List<string> sections = GetSections(guidelines);
            foreach (string section in sections)
            {
                console.Out.WriteLine(section);

                mdWriter.WriteUnorderedListItem(section, format: MdFormat.InternalLink, listIndent: 0);


                subSections = (from guideline in guidelines
                               where string.Equals(guideline.Section, section, System.StringComparison.OrdinalIgnoreCase)
                               select guideline.Subsection).Distinct().OrderBy(x => x).ToList();

                foreach (string subsection in subSections)
                {
                    console.Out.WriteLine($"     { subsection}");
                    mdWriter.WriteUnorderedListItem(subsection, format: MdFormat.InternalLink, listIndent: 1);
                }
                mdWriter.WriteLine("", numNewLines: 1);
            }
        }

        public static List<string> GetSections(ICollection<Guideline> guidelines)
        {

            List<string> sections = (from guideline in guidelines
                                     select guideline.Section).Distinct().OrderBy(x => x).ToList();
            return sections;
        }

        public static List<string> GetSubSections(ICollection<Guideline> guidelines)
        {

            List<string> subSections = (from guideline in guidelines
                                        select guideline.Subsection).Distinct().OrderBy(x => x).ToList();
            return subSections;
        }
    }
}

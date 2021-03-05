using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MarkdownOut;


namespace GuidelineXmlToMD
{
    class Program
    {
        static MarkdownOut.MdWriter _MdWriter;

        /// <summary>
        /// A simple to to convert coding guidelines xml to the website formatted markdown.
        /// </summary>
        /// <param name="args">It is implied that args[0] is the name of the xml file that is stored at CodingGuidelines/docs/ This can also be explicitly passed with --xml-file-name</param>
        /// <param name="xmlFileName">The name of the xml file that is stored at CodingGuidelines/docs/</param>
        /// <param name="i">The xml file to process when using the -m option</param>
        /// <param name="o">The md file to create when using the -m option </param>
        /// <param name="m">Manual Run - Use xmlFilePath and outputMDFilePath</param>
        static void Main(string[] args, string xmlFileName, string i = "null", string o = "null", bool m = false)
        {
            string markDownOutputFilePath;
            string xmlInputFilePath;

            if (m)
            {
                if (Regex.Match(i, @".*.xml").Success)
                {
                    Console.WriteLine($"Converting {i} to {o}");
                }
                else
                {
                    Console.WriteLine($"Invalid xml file name: {i}");
                    return;
                }

                markDownOutputFilePath = Path.GetFullPath(o);
                xmlInputFilePath = Path.GetFullPath(i);
            }
            else
            { // run in based on the repo file structure
              // example structure of repo:
              // CodingGuidelines\XMLtoMD\GuidelineXmlToMD\bin\Debug\netcoreapp3.1
              // CodingGuidelines\docs


                if (args.Length != 0)
                {  // check for input fileName being passed without parameter name specified
                    if (Regex.Match(args[0], @".*.xml").Success)
                    {
                        xmlFileName = args[0];
                    }
                }

                Match repoRefFolder = Regex.Match(AssemblyDirectory, @$".*CodingGuidelines");
                string[] defaultXmlFilePath = { repoRefFolder.Value, "docs", xmlFileName };
                xmlInputFilePath = Path.Combine(defaultXmlFilePath);

                string mdFileName = "csharp.md";
                string[] mdFilePath = { repoRefFolder.Value, "docs", "coding", mdFileName };
                markDownOutputFilePath = Path.Combine(mdFilePath);

            }


            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(xmlInputFilePath);

            using (_MdWriter = new MdWriter(markDownOutputFilePath))
            {


                _MdWriter.WriteLine("C# Guidelines", format: MdFormat.Heading1);
                PrintSections(guidelines);
                _MdWriter.WriteLine("Guidelines", format: MdFormat.Heading1);
                _MdWriter.WriteLine("");
                PrintGuidelinesBySection(guidelines);
            }


        }

        private static void PrintGuidelinesBySection(ICollection<Guideline> guidelines)
        {
            foreach (string section in GetSections(guidelines))
            {
                _MdWriter.WriteLine("");
                Console.WriteLine(section);
                _MdWriter.WriteLine(section, format: MdFormat.Heading2, style: MdStyle.BoldItalic);



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
                        Console.WriteLine($"     { subsection}");
                        _MdWriter.WriteLine(subsection, format: MdFormat.Heading3, numNewLines: 1);

                        foreach (Guideline guideline in guidelinesInSubsection)
                        {
                            _MdWriter.WriteUnorderedListItem(GetGuidelineEmoji(guideline) + " " + guideline.Text.Trim('"'), listIndent: 0);
                        }
                    }
                    _MdWriter.WriteLine("", numNewLines: 1);

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

        private static void PrintSections(ICollection<Guideline> guidelines)
        {
            _MdWriter.WriteLine("Sections", format: MdFormat.Heading2);

            List<string> subSections = new List<string>();

            List<string> sections = GetSections(guidelines);
            foreach (string section in sections)
            {
                Console.WriteLine(section);

                _MdWriter.WriteUnorderedListItem(section, format: MdFormat.InternalLink, listIndent: 0);


                subSections = (from guideline in guidelines
                               where string.Equals(guideline.Section, section, System.StringComparison.OrdinalIgnoreCase)
                               select guideline.Subsection).Distinct().OrderBy(x => x).ToList();

                foreach (string subsection in subSections)
                {
                    Console.WriteLine($"     { subsection}");
                    _MdWriter.WriteUnorderedListItem(subsection, format: MdFormat.InternalLink, listIndent: 1);
                }
                _MdWriter.WriteLine("", numNewLines: 1);



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

        public static string AssemblyDirectory
        {
            get
            {
#pragma warning disable SYSLIB0012
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
#pragma warning restore SYSLIB0012
            }
        }
    }





}

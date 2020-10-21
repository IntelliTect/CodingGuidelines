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
        static void Main(string[] args)
        {
            string xmlFileName = "Guidelines(8th Edition).xml";
            if (args.Length != 0) {  //check for input fileName
                if (Regex.Match(AssemblyDirectory, @$"*.xml").Success) {
                    xmlFileName = args[0];
                }
            }

            Match repoRefFolder = Regex.Match(AssemblyDirectory, @$".*CodingGuidelines");

            string[] xmlFilePath = { repoRefFolder.Value, "docs", xmlFileName};
            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(Path.Combine(xmlFilePath));
            
            
            string mdFileName = "csharp.md";
            string[] mdFilePath = { repoRefFolder.Value, "docs", "coding", mdFileName};

            using (_MdWriter = new MdWriter(Path.Combine(mdFilePath)))
            {


                _MdWriter.WriteLine("C# Guidelines", format: MdFormat.Heading1);
                PrintSections(guidelines);
                _MdWriter.WriteLine("Guidelines", format: MdFormat.Heading1);
                _MdWriter.WriteLine("");
                PrintGuidelinesBySection(guidelines);
            }
        



            //C: \Users\saffron\source\repos\CodingGuidelines\XMLtoMD\GuidelineXmlToMD\bin\Debug\netcoreapp3.1
            //C: \Users\saffron\source\repos\CodingGuidelines\docs


        }

        private static void PrintGuidelinesBySection(ICollection<Guideline> guidelines)
        {
            foreach (string section in GetSections(guidelines))
            {
                _MdWriter.WriteLine("");
                Console.WriteLine(section);
                _MdWriter.WriteLine(section, format: MdFormat.Heading2,style: MdStyle.BoldItalic);



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
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }





}

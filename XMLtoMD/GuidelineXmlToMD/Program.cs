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
            char slash = Path.DirectorySeparatorChar;
            Console.WriteLine(AssemblyDirectory);
            Match match = Regex.Match(AssemblyDirectory, @$".*CodingGuidelines");
            string guidelineXmlLocation = match.Value + @$"{slash}docs{slash}Guidelines(8th Edition).xml";

            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(guidelineXmlLocation);
            _MdWriter = new MdWriter(match.Value + @$"{slash}docs{slash}coding{slash}csharp.md");

            PrintSections(guidelines);
            _MdWriter.WriteLine("Guidelines", format: MdFormat.Heading1);
            _MdWriter.WriteLine("");
            PrintGuidelinesBySection(guidelines);

            _MdWriter.Close();
            _MdWriter.Dispose();



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
                                                               where string.Equals(guideline.Section, section)
                                                               select guideline);

                foreach (string subsection in GetSubSections(guidelines))
                {

                    IEnumerable<Guideline> guidelinesInSubsection = (from guideline in guidelinesInSections
                                                                     where string.Equals(guideline.Subsection, subsection)
                                                                     select guideline);

                    if (guidelinesInSubsection.Count() > 0)
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
                               where string.Equals(guideline.Section, section)
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

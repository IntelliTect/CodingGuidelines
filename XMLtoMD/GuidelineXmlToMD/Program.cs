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
            Console.WriteLine(AssemblyDirectory);
            Match match = Regex.Match(AssemblyDirectory, @".*CodingGuidelines");
            string guidelineXmlLocation = match.Value + @"\\docs\\Guidelines(8th Edition).xml";

            ICollection<Guideline> guidelines = GuidelineXmlFileReader.ReadExisitingGuidelinesFile(guidelineXmlLocation);
            _MdWriter = new MdWriter(match.Value + @"\\docs\\coding\\CSharpGuidelines.md");

            PrintSections(guidelines);
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
                Console.WriteLine(section);
                _MdWriter.WriteLine(section, format: MdFormat.Heading2);


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
                        _MdWriter.WriteLine(subsection, format: MdFormat.Heading3);

                        foreach (Guideline guideline in guidelinesInSubsection)
                        {
                            _MdWriter.WriteUnorderedListItem(guideline.Text.Trim('"'), listIndent: 0);
                        }
                    }


                    

                }

                

            }
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

                

            }

           

        }

        public static List<string> GetSections(ICollection<Guideline> guidelines) {

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

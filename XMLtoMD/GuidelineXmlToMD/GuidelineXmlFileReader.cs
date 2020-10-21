using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GuidelineXmlToMD
{
    static class GuidelineXmlFileReader
    {
        public const string _Guideline = "guideline";
        public const string _Key = "key";
        public const string _Severity = "severity";

        public const string _Section = "section";
        public const string _Subsection = "subsection";
        public const string _Comments = "comments";


        public static ICollection<Guideline> ReadExisitingGuidelinesFile(string pathToExistingGuidelinesXml)
        {

            XDocument previousGuidelines = XDocument.Load(pathToExistingGuidelinesXml);

            HashSet<Guideline> guidelines = new HashSet<Guideline>();

            foreach (XElement guidelineFromXml in previousGuidelines.Root.DescendantNodes().OfType<XElement>())
            {
                Guideline guideline = new Guideline();
                guideline.Severity = guidelineFromXml.Attribute(_Severity)?.Value;
                guideline.Subsection = guidelineFromXml.Attribute(_Subsection)?.Value;
                guideline.Section = guidelineFromXml.Attribute(_Section)?.Value;
                guideline.Text = guidelineFromXml?.Value;
                guideline.Key = guidelineFromXml.Attribute(_Key)?.Value;

                guidelines.Add(guideline);
            }
            return guidelines;
        }

    }
}

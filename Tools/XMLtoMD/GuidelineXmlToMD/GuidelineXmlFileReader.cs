using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GuidelineXmlToMD
{
    public static class GuidelineXmlFileReader
    {
        private const string _Key = "key";
        private const string _Severity = "severity";
        private const string _Section = "section";
        private const string _Subsection = "subsection";


        public static ICollection<Guideline> ReadExisitingGuidelinesFile(string pathToExistingGuidelinesXml)
        {

            XDocument previousGuidelines = XDocument.Load(pathToExistingGuidelinesXml);

            HashSet<Guideline> guidelines = new HashSet<Guideline>();

            foreach (XElement guidelineFromXml in previousGuidelines.Root.DescendantNodes().OfType<XElement>())
            {
                Guideline guideline = new Guideline(
                    Key: guidelineFromXml.Attribute(_Key)?.Value,
                    Text: guidelineFromXml?.Value,
                    Severity: guidelineFromXml.Attribute(_Severity)?.Value,
                    Section: guidelineFromXml.Attribute(_Section)?.Value,
                    Subsection: guidelineFromXml.Attribute(_Subsection)?.Value
                );
                guidelines.Add(guideline);
            }
            return guidelines;
        }

    }
}

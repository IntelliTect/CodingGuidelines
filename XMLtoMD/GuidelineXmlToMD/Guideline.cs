using System.Collections.Generic;


namespace GuidelineXmlToMD
{
    public class Guideline
    {
        public string Key { get; set; } = "";
        public string Text { get; set; } = "";

        public string Severity { get; set; } = "";

        public string Section { get; set; } = "";
        public string Subsection { get; set; } = "";

        public List<string> Comments { get; set; } = new List<string>();

        public override int GetHashCode()
        {
            return Subsection.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Guideline otherGuideline = obj as Guideline;

            return otherGuideline != null && string.Equals(otherGuideline.Key, this.Key);
        }

    }

}

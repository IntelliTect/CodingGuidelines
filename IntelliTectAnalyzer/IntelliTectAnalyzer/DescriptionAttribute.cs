using System;

namespace IntelliTectAnalyzer
{
    [AttributeUsage(AttributeTargets.All)]
    internal class DescriptionAttribute : Attribute
    {
        public static DescriptionAttribute Default { get; } = new DescriptionAttribute();
        
        public string Description { get; }

        public DescriptionAttribute() : this(string.Empty)
        {
        }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (obj is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description == Description;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}

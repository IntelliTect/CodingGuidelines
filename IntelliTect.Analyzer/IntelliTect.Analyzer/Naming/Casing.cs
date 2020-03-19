using System.Collections.Generic;

namespace IntelliTect.Analyzer.Naming
{
    internal static class Casing
    {
        public static bool IsPascalCase(IEnumerable<char> name)
        {
            bool isFirst = true;
            foreach (char character in name)
            {
                if (isFirst && !char.IsUpper(character))
                {
                    return false;
                }

                isFirst = false;
                if (char.IsUpper(character) ||
                    char.IsLower(character) ||
                    char.IsNumber(character))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}

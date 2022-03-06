using System;
using System.Collections.Generic;
using System.Text;

namespace Unity3D_Arabic
{
    internal class Numbers
    {
        internal static List<char> Arabic = new List<char>()
        {
            '0','1','2','3','4','5','6','7','8','9'
        };

        internal static List<char> Hindu = new List<char>()
        {
            '٠','١','٢','٣','٤','٥','٦','٧','٨','٩'
        };

        internal static List<char> Persian_Urdu = new List<char>()
        {
            '۰','۱','۲','٣','۴','۵','۶','٧','٨','٩'
        };

        public static char Correct(char charNumber, NumberStyles style)
        {
            bool isNumber = char.IsNumber(charNumber);
            if (!isNumber)
                return charNumber;
            int intNumber = (int)char.GetNumericValue(charNumber);

            if (style == NumberStyles.Arabic)
                return Arabic[intNumber];
            if (style == NumberStyles.Persian_Urdu)
                return Persian_Urdu[intNumber];
            if (style == NumberStyles.Hindu)
                return Hindu[intNumber];

            return charNumber;
        }
    }
}

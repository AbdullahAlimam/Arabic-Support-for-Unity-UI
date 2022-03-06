using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Unity3D_Arabic
{
    internal class Diacritics
    {
        /// <summary>
        /// this function has been tested on strings have hundred of characters and it responses on most 
        /// of them in less than a millisecond
        /// testString = "بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ" X 500
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

    }
}

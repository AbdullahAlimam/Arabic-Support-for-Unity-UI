using System;
using System.Collections.Generic;
using System.Linq;

namespace Unity3D_Arabic
{
    /// <summary>
    /// convert General Unicode (ISO/IEC 8859-6) to Isolated Code (Contextual forms)
    /// to represen End form        => Isolated Code + 1
    /// to represen Middle form     => Isolated Code + 2
    /// to represen Beginning form  => Isolated Code + 3
    /// Refrence: https://en.wikipedia.org/wiki/Arabic_script_in_Unicode
    /// All Chars Refrence: https://www.ssec.wisc.edu/~tomw/java/unicode.html#x0600
    /// </summary>
    internal static class UnicodeContextualMapping
    {
        private static Dictionary<int, ArabicLetter> _generalUnicodeMap = new Dictionary<int, ArabicLetter>();
        private static List<ArabicLigatureLetter> _ligatureUnicodeMap = new List<ArabicLigatureLetter>();
        private static Dictionary<int, int> _bracketOppositeUnicodeMap = new Dictionary<int, int>();
        static List<int> _ligatureFirstLetters = new List<int>();
       
        static UnicodeContextualMapping()
        {
            BuildArabicLetterMap();
            BuildLigatureMap();
            BuildBracketMap();
        }
        static void BuildArabicLetterMap()
        {
            // Basic Arabic Alphabet :
            _generalUnicodeMap[0x0621] = new ArabicLetter(0xFE80, ArabicLetterShapes.End);  // key=ء | value=ﺀ
            _generalUnicodeMap[0x0622] = new ArabicLetter(0xFE81, ArabicLetterShapes.End);  // key=آ | value=ﺁ
            _generalUnicodeMap[0x0623] = new ArabicLetter(0xFE83, ArabicLetterShapes.End);  // key=أ | value=ﺃ
            _generalUnicodeMap[0x0624] = new ArabicLetter(0xFE85, ArabicLetterShapes.End);  // key=ؤ | value=ﺅ
            _generalUnicodeMap[0x0625] = new ArabicLetter(0xFE87, ArabicLetterShapes.End);  // key=إ | value=ﺇ
            _generalUnicodeMap[0x0626] = new ArabicLetter(0xFE89, ArabicLetterShapes.All);  // key=ئ | value=ﺉ

            _generalUnicodeMap[0x0627] = new ArabicLetter(0xFE8D, ArabicLetterShapes.End);  // key=ا | value=ﺍ
            _generalUnicodeMap[0x0628] = new ArabicLetter(0xFE8F, ArabicLetterShapes.All);  // key=ب | value=ﺏ
            _generalUnicodeMap[0x0629] = new ArabicLetter(0xFE93, ArabicLetterShapes.End);  // key=ة | value=ﺓ
            _generalUnicodeMap[0x062A] = new ArabicLetter(0xFE95, ArabicLetterShapes.All);  // key=ت | value=ﺕ
            _generalUnicodeMap[0x062B] = new ArabicLetter(0xFE99, ArabicLetterShapes.All);  // key=ث | value=ﺙ
            _generalUnicodeMap[0x062C] = new ArabicLetter(0xFE9D, ArabicLetterShapes.All);  // key=ج | value=ﺝ
            _generalUnicodeMap[0x062D] = new ArabicLetter(0xFEA1, ArabicLetterShapes.All);  // key=ح | value=ﺡ
            _generalUnicodeMap[0x062E] = new ArabicLetter(0xFEA5, ArabicLetterShapes.All);  // key=خ | value=ﺥ
            _generalUnicodeMap[0x062F] = new ArabicLetter(0xFEA9, ArabicLetterShapes.End);  // key=د | value=ﺩ
            _generalUnicodeMap[0x0630] = new ArabicLetter(0xFEAB, ArabicLetterShapes.End);  // key=ذ | value=ﺫ
            _generalUnicodeMap[0x0631] = new ArabicLetter(0xFEAD, ArabicLetterShapes.End);  // key=ر | value=ﺭ
            _generalUnicodeMap[0x0632] = new ArabicLetter(0xFEAF, ArabicLetterShapes.End);  // key=ز | value=ﺯ
            _generalUnicodeMap[0x0633] = new ArabicLetter(0xFEB1, ArabicLetterShapes.All);  // key=س | value=ﺱ
            _generalUnicodeMap[0x0634] = new ArabicLetter(0xFEB5, ArabicLetterShapes.All);  // key=ش | value=ﺵ
            _generalUnicodeMap[0x0635] = new ArabicLetter(0xFEB9, ArabicLetterShapes.All);  // key=ص | value=ﺹ
            _generalUnicodeMap[0x0636] = new ArabicLetter(0xFEBD, ArabicLetterShapes.All);  // key=ض | value=ﺽ
            _generalUnicodeMap[0x0637] = new ArabicLetter(0xFEC1, ArabicLetterShapes.All);  // key=ط | value=ﻁ
            _generalUnicodeMap[0x0638] = new ArabicLetter(0xFEC5, ArabicLetterShapes.All);  // key=ظ | value=ﻅ
            _generalUnicodeMap[0x0639] = new ArabicLetter(0xFEC9, ArabicLetterShapes.All);  // key=ع | value=ﻉ
            _generalUnicodeMap[0x063A] = new ArabicLetter(0xFECD, ArabicLetterShapes.All);  // key=غ | value=ﻍ
            _generalUnicodeMap[0x0641] = new ArabicLetter(0xFED1, ArabicLetterShapes.All);  // key=ف | value=ﻑ
            _generalUnicodeMap[0x0642] = new ArabicLetter(0xFED5, ArabicLetterShapes.All);  // key=ق | value=ﻕ
            _generalUnicodeMap[0x0643] = new ArabicLetter(0xFED9, ArabicLetterShapes.All);  // key=ك | value=ﻙ
            _generalUnicodeMap[0x0644] = new ArabicLetter(0xFEDD, ArabicLetterShapes.All);  // key=ل | value=ﻝ
            _generalUnicodeMap[0x0645] = new ArabicLetter(0xFEE1, ArabicLetterShapes.All);  // key=م | value=ﻡ
            _generalUnicodeMap[0x0646] = new ArabicLetter(0xFEE5, ArabicLetterShapes.All);  // key=ن | value=ﻥ
            _generalUnicodeMap[0x0647] = new ArabicLetter(0xFEE9, ArabicLetterShapes.All);  // key=ه | value=ﻩ
            _generalUnicodeMap[0x0648] = new ArabicLetter(0xFEED, ArabicLetterShapes.End);  // key=و | value=ﻭ
            _generalUnicodeMap[0x0649] = new ArabicLetter(0xFEEF, ArabicLetterShapes.End);  // key=ى | value=ﻯ
            _generalUnicodeMap[0x064A] = new ArabicLetter(0xFEF1, ArabicLetterShapes.All);  // key=ي | value=ﻱ

            // Arabic Punctuation and ornaments

            // Other languages
            _generalUnicodeMap[0x067E] = new ArabicLetter(0xFB56, ArabicLetterShapes.All);  // key=پ | value=ﭖ
            _generalUnicodeMap[0x0686] = new ArabicLetter(0xFB7A, ArabicLetterShapes.All);  // key=چ | value=ﭺ
            _generalUnicodeMap[0x0698] = new ArabicLetter(0xFB8A, ArabicLetterShapes.End);  // key=ژ | value=ﮊ
            _generalUnicodeMap[0x06AF] = new ArabicLetter(0xFB92, ArabicLetterShapes.Isolated);  // key=گ | value=ﮒ
        }

        static void BuildLigatureMap()
        {
            _ligatureUnicodeMap.Add(new ArabicLigatureLetter(0xFEF5, ArabicLetterShapes.End, 0x0644, 0x0622)); // ﻵ
            _ligatureUnicodeMap.Add(new ArabicLigatureLetter(0xFEF7, ArabicLetterShapes.End, 0x0644, 0x0623)); // ﻷ
            _ligatureUnicodeMap.Add(new ArabicLigatureLetter(0xFEF9, ArabicLetterShapes.End, 0x0644, 0x0625)); // ﻹ
            _ligatureUnicodeMap.Add(new ArabicLigatureLetter(0xFEFB, ArabicLetterShapes.End, 0x0644, 0x0627)); // ﻻ

            //_ligatureUnicodeMap.Add(new ArabicLigatureLetter(0xFDF2, ArabicLetterShapes.Isolated, 0x0627, 0x0644, 0x0644, 0x0647)); // الله


            _ligatureFirstLetters.AddRange(_ligatureUnicodeMap.Select(x=>x.Letters.First()).Distinct().ToList());
        }

        static void BuildBracketMap()
        {
            _bracketOppositeUnicodeMap[0x0028] = 0x0029;  // key=( | value=)
            _bracketOppositeUnicodeMap[0x0029] = 0x0028;  // key=) | value=(

            _bracketOppositeUnicodeMap[0x003C] = 0x003E;  // key=< | value=>
            _bracketOppositeUnicodeMap[0x003E] = 0x003C;  // key=> | value=<

            _bracketOppositeUnicodeMap[0x005B] = 0x005D;  // key=[ | value=]
            _bracketOppositeUnicodeMap[0x005D] = 0x005B;  // key=] | value=[

            _bracketOppositeUnicodeMap[0x007B] = 0x007D;  // key={ | value=}
            _bracketOppositeUnicodeMap[0x007D] = 0x007B;  // key=} | value={

            //Halfwidth and Fullwidth Forms
            _bracketOppositeUnicodeMap[0xFF08] = 0xFF09;  // key=（ | value=）
            _bracketOppositeUnicodeMap[0xFF09] = 0xFF08;  // key=） | value=（

            _bracketOppositeUnicodeMap[0xFF1C] = 0xFF1E;  // key=＜ | value=＞
            _bracketOppositeUnicodeMap[0xFF1E] = 0xFF1C;  // key=＞ | value=＜

            _bracketOppositeUnicodeMap[0xFF3B] = 0xFF3D;  // key=［ | value=］
            _bracketOppositeUnicodeMap[0xFF3D] = 0xFF3B;  // key=］ | value=［

            _bracketOppositeUnicodeMap[0xFF5B] = 0xFF5D;  // key=｛ | value=］
            _bracketOppositeUnicodeMap[0xFF5D] = 0xFF5B;  // key=］ | value=｛
        }

        public static ArabicLetter GetArabicLetter(int leter)
        {
            if (_generalUnicodeMap.Keys.Contains(leter))
                return _generalUnicodeMap[leter];
            else
                return null;
        }

        public static bool IsBracket(int leter)
        {
            return _bracketOppositeUnicodeMap.Keys.Contains(leter);
        }



        public static bool IsGeneralIsolatedArabic(int leter)
        {
            return _generalUnicodeMap.Keys.Contains(leter);
        }

        public static bool CheckIfCouldBeLigature(int leter)
        {
            return _ligatureFirstLetters.Contains(leter);
        }

        internal static bool LigatureConvert(CharacterInfoCollection letters, int index) // max should be about 5 to 8
        {
            int generalUnicode = letters[index].Character;

            int maxSubStringLength = Math.Min(5, letters.Count - (index));

            for (int i = maxSubStringLength; i >= 0; i--)
            {
                List<int> _string = new List<int>();
                for (int j = index; j < letters.Count && j < (index + i); j++)
                    _string.Add((int)letters[j].Character);
                //_string.AddRange(letters.Skip(index).Take(i).ToList().Select(s=> (int)s).ToList());
                ArabicLigatureLetter ligature = _ligatureUnicodeMap.Where(x => x.Letters[0] == generalUnicode && x.Letters.SequenceEqual(_string)).FirstOrDefault();
                if (ligature != null)
                {
                    letters[index].Character = ligature.LigatureUnicode;
                    letters[index].IsLigature = true;
                    letters[index].Shapes = ligature.Shapes;

                    // is some leeters are Ligature then we will get new char to represent them and need to skip others in array
                    for (int j = 1; j < i; j++)
                        letters.RemoveAt(index + 1);

                    return true;
                }
            }

            return false;
        }

        internal static void Convert(CharacterInfoCollection characters, int index)
        {
            char character = characters[index].Character;
            if (_generalUnicodeMap.ContainsKey(character))
            {
                characters[index].Character = _generalUnicodeMap[character].IsolatedUnicode;
                characters[index].Shapes = _generalUnicodeMap[character].Shapes;
            }
        }

        internal static void ConvertBracke(CharacterInfoCollection characters, int index)
        {
            char character = characters[index].Character;
            if (_bracketOppositeUnicodeMap.ContainsKey(character))
            {
                characters[index].Character = (char)_bracketOppositeUnicodeMap[character];
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Unity3D_Arabic
{
    internal static class Correcter
    {
        internal static string Repair(string text)
        {
            string cleanText = Diacritics.RemoveDiacritics(text);
            CharacterInfoCollection CharactersInfo = new CharacterInfoCollection(cleanText);
            
            // re-shabing arabic letters
            for (int i = 0; i < CharactersInfo.Count; ++i)
            {                
                if (CharactersInfo[i].IsArabic)
                {
                    // fix Ligature
                    if (i < CharactersInfo.Count - 1 && UnicodeContextualMapping.CheckIfCouldBeLigature(CharactersInfo[i].Character))
                    {
                        UnicodeContextualMapping.LigatureConvert(CharactersInfo, i);
                    }

                    if (!CharactersInfo[i].IsLigature)
                    {
                        UnicodeContextualMapping.Convert(CharactersInfo, i);
                    }

                    if (CharactersInfo[i].Shapes != ArabicLetterShapes.Isolated)
                    {
                        CharacterInfo c = CharactersInfo[i];
                        CharacterInfo pc = CharactersInfo[i - 1];
                        if (pc != null && !pc.IsArabic)
                            pc = null;
                        CharacterInfo nc = CharactersInfo[i + 1];
                        if (nc != null && !nc.IsArabic)
                            nc = null;

                        if (c.Shapes == ArabicLetterShapes.All && nc != null && (pc == null || pc.Shapes != ArabicLetterShapes.All))
                            CharactersInfo[i].ToLeadingLetter();
                        else if (c.Shapes == ArabicLetterShapes.All && pc != null && pc.Shapes == ArabicLetterShapes.All && nc != null && nc.Shapes != ArabicLetterShapes.Isolated)
                                CharactersInfo[i].ToMiddleLetter();
                        else if ((pc != null && pc.Shapes == ArabicLetterShapes.All) && (nc == null || nc.Shapes != ArabicLetterShapes.All || c.Shapes == ArabicLetterShapes.End))
                            CharactersInfo[i].ToFinishingLetter();
                    }
                }

                // fix numbers
                if (CharactersInfo[i].IsNumber)
                    CharactersInfo[i].Character = Numbers.Correct(CharactersInfo[i].Character, Settings.NumberStyle);
            }

            // fix problem of combination with non arabic letters
            List<char> arabicCharList = new List<char>();
            List<char> notArabicCharList = new List<char>();

            for (int i = CharactersInfo.Count - 1; i >= 0; --i)
            {
                bool inRange = i > 0 && i < CharactersInfo.Count - 1;
                CharacterInfo c = CharactersInfo[i];
                CharacterInfo pc = CharactersInfo[i - 1];
                CharacterInfo nc = CharactersInfo[i + 1];

                if(c.IsBracket && Settings.FlipBracket)
                    UnicodeContextualMapping.ConvertBracke(CharactersInfo, i);

                if (c.IsArabic || (c.IsSpace && pc != null && pc.IsArabic))
                {
                    ConcatenateNotArabicWithArabic(arabicCharList, notArabicCharList);
                    arabicCharList.Add(c.Character);
                }
                else
                    notArabicCharList.Add(c.Character);
            }

            ConcatenateNotArabicWithArabic(arabicCharList, notArabicCharList);

            text = new string(arabicCharList.ToArray());
            return text;
        }

        internal static void ConcatenateNotArabicWithArabic(List<char> arabicCharList, List<char> notArabicCharList)
        {
            notArabicCharList.Reverse();
            arabicCharList.AddRange(notArabicCharList);
            notArabicCharList.Clear();
        }
    }
}
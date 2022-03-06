using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3D_Arabic
{
    public class CharacterInfoCollection
    {
        public List<CharacterInfo> CharactersInfo;

        public CharacterInfoCollection(string text)
        {
            if (CharactersInfo == null)
                CharactersInfo = new List<CharacterInfo>();

            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];

                ArabicLetter al = UnicodeContextualMapping.GetArabicLetter(letter);

                CharactersInfo.Add(new CharacterInfo()
                {
                    Character = letter,
                    IsArabic = al != null,
                    IsNumber = char.IsNumber(letter),
                    IsPunctuation = char.IsPunctuation(letter),
                    IsBracket = UnicodeContextualMapping.IsBracket(letter),
                    Shapes = al != null ? al.Shapes : ArabicLetterShapes.Isolated,
                });
            }
        }

        public CharacterInfo this[int i]
        {
            get
            {
                if (CharactersInfo == null || CharactersInfo.Count == 0 || i < 0 || i >= CharactersInfo.Count)
                    return null;

                return CharactersInfo[i];
            }
        }

        public void RemoveAt(int index)
        {
            CharactersInfo.RemoveAt(index);
        }

        public int Count {  get { return CharactersInfo.Count; } }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3D_Arabic
{
    public class CharacterInfo
    {
        public ArabicLetterShapes Shapes { get; set; }
        public char Character { get; set; }
        public bool IsArabic { get; set; }
        public bool IsLigature { get; set; }
        public bool IsNumber { get; set; }
        public bool IsPunctuation { get; set; }
        public bool IsBracket { get; set; }
        public bool IsSpace { get { return Character == 0x0020; } }

        public void ToLeadingLetter()
        {
            Character = (char)(Character + 2U);
        }
        public void ToMiddleLetter()
        {
            Character = (char)(Character + 3U);
        }
        public void ToFinishingLetter()
        {
            Character = (char)(Character + 1U);
        }
    }


}

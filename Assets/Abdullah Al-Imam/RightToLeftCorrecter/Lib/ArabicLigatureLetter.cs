using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3D_Arabic
{
    internal class ArabicLigatureLetter
    {
        public List<int> Letters = new List<int>();
        public ArabicLigatureLetter(char ligatureUnicode, ArabicLetterShapes letterShapes, params char[] letters)
            : this((int)ligatureUnicode, letterShapes)
        {
            for (int i = 0; i < letters.Length; i++)
                Letters.Add((int)letters[i]);
        }

        public ArabicLigatureLetter(int ligatureUnicode, ArabicLetterShapes letterShapes, params int[] letters)

        {
            Shapes = letterShapes;
            LigatureUnicode = (char)ligatureUnicode;
            if (letters != null)
                Letters.AddRange(letters);
        }


        public char LigatureUnicode { get; private set; }
        public ArabicLetterShapes Shapes { get; private set; }
    }


}

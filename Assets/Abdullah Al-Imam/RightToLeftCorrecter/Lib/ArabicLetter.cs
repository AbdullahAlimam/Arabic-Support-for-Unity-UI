using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3D_Arabic
{
    internal class ArabicLetter
    {
        public ArabicLetter(char isolatedUnicode, ArabicLetterShapes letterShapes)
        {
            IsolatedUnicode = isolatedUnicode;
            Shapes = letterShapes;
        }

        public ArabicLetter(int isolatedUnicode, ArabicLetterShapes letterShapes)
            : this((char)isolatedUnicode, letterShapes)
        {

        }


        public char IsolatedUnicode { get; private set; }
        public ArabicLetterShapes Shapes { get; private set; }
    }


}

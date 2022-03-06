using System;
using System.Collections.Generic;
using System.Text;

namespace Unity3D_Arabic
{
    // arabic letters some of them have just Isolated shape or
    // Isolated + End Or
    // All shapes
    public enum ArabicLetterShapes
    {
        Isolated = 0x000,
        End = 0x001,
        //Middle = 0x010,
        //Beginning = 0x100,
        All = 0x111
    }
}

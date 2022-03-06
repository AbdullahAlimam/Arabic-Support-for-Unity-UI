using System;

namespace Unity3D_Arabic
{
    public class RTLLCorrecter
    {
        public const Char NewLine = Settings.NewLine;
        public static string Correct(string text, bool flipBracket, NumberStyles numberStyle = NumberStyles.Arabic)
        {
            if (text == null)
                return "";

            Settings.NumberStyle = numberStyle;
            Settings.FlipBracket = flipBracket;

            if (text.Contains("\r"))
                text = text.Replace("\r", "");

            string[] lines = text.Split(Settings.NewLine);

            string textLines = "";
            int index = 0;
            do
            {
                if(index == 0)
                    textLines = Correcter.Repair(lines[index]);
                else
                    textLines += Settings.NewLine + Correcter.Repair(lines[index]);
                index++;
            } while (index < lines.Length);
            
            return textLines;
        }
    }
}

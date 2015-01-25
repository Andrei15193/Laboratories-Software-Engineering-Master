using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;

namespace BillPath.ViewModels
{
    public class NamedColors
    {
        public static NamedColor GetByName(string name)
        {
            return _namedColors[name];
        }
        public static NamedColor GetByColor(Color color)
        {
            return _namedColors.Values.First(namedColor => namedColor.Color == color);
        }

        public static IEnumerable<NamedColor> GetAllNamedColors()
        {
                return _namedColors.Values;
        }
        public static IEnumerable<string> GetAllColorNames()
        {
                return _namedColors.Keys;
        }

        public IEnumerable<NamedColor> AllNamedColors
        {
            get
            {
                return GetAllNamedColors();
            }
        }
        public IEnumerable<string> AllColorNames
        {
            get{
                return GetAllColorNames();
            }
        }

        private static IReadOnlyDictionary<string, NamedColor> _namedColors = _GetNamedColors();

        private static IReadOnlyDictionary<string, NamedColor> _GetNamedColors()
        {
            IDictionary<string, NamedColor> namedColors = new SortedDictionary<string, NamedColor>(StringComparer.OrdinalIgnoreCase);

            namedColors.Add("AliceBlue", new NamedColor("AliceBlue", Colors.AliceBlue));
            namedColors.Add("AntiqueWhite", new NamedColor("AntiqueWhite", Colors.AntiqueWhite));
            namedColors.Add("Aqua", new NamedColor("Aqua", Colors.Aqua));
            namedColors.Add("Aquamarine", new NamedColor("Aquamarine", Colors.Aquamarine));
            namedColors.Add("Azure", new NamedColor("Azure", Colors.Azure));
            namedColors.Add("Beige", new NamedColor("Beige", Colors.Beige));
            namedColors.Add("Bisque", new NamedColor("Bisque", Colors.Bisque));
            namedColors.Add("Black", new NamedColor("Black", Colors.Black));
            namedColors.Add("BlanchedAlmond", new NamedColor("BlanchedAlmond", Colors.BlanchedAlmond));
            namedColors.Add("Blue", new NamedColor("Blue", Colors.Blue));
            namedColors.Add("BlueViolet", new NamedColor("BlueViolet", Colors.BlueViolet));
            namedColors.Add("Brown", new NamedColor("Brown", Colors.Brown));
            namedColors.Add("BurlyWood", new NamedColor("BurlyWood", Colors.BurlyWood));
            namedColors.Add("CadetBlue", new NamedColor("CadetBlue", Colors.CadetBlue));
            namedColors.Add("Chartreuse", new NamedColor("Chartreuse", Colors.Chartreuse));
            namedColors.Add("Chocolate", new NamedColor("Chocolate", Colors.Chocolate));
            namedColors.Add("Coral", new NamedColor("Coral", Colors.Coral));
            namedColors.Add("CornflowerBlue", new NamedColor("CornflowerBlue", Colors.CornflowerBlue));
            namedColors.Add("Cornsilk", new NamedColor("Cornsilk", Colors.Cornsilk));
            namedColors.Add("Crimson", new NamedColor("Crimson", Colors.Crimson));
            namedColors.Add("Cyan", new NamedColor("Cyan", Colors.Cyan));
            namedColors.Add("DarkBlue", new NamedColor("DarkBlue", Colors.DarkBlue));
            namedColors.Add("DarkCyan", new NamedColor("DarkCyan", Colors.DarkCyan));
            namedColors.Add("DarkGoldenrod", new NamedColor("DarkGoldenrod", Colors.DarkGoldenrod));
            namedColors.Add("DarkGray", new NamedColor("DarkGray", Colors.DarkGray));
            namedColors.Add("DarkGreen", new NamedColor("DarkGreen", Colors.DarkGreen));
            namedColors.Add("DarkKhaki", new NamedColor("DarkKhaki", Colors.DarkKhaki));
            namedColors.Add("DarkMagenta", new NamedColor("DarkMagenta", Colors.DarkMagenta));
            namedColors.Add("DarkOliveGreen", new NamedColor("DarkOliveGreen", Colors.DarkOliveGreen));
            namedColors.Add("DarkOrange", new NamedColor("DarkOrange", Colors.DarkOrange));
            namedColors.Add("DarkOrchid", new NamedColor("DarkOrchid", Colors.DarkOrchid));
            namedColors.Add("DarkRed", new NamedColor("DarkRed", Colors.DarkRed));
            namedColors.Add("DarkSalmon", new NamedColor("DarkSalmon", Colors.DarkSalmon));
            namedColors.Add("DarkSeaGreen", new NamedColor("DarkSeaGreen", Colors.DarkSeaGreen));
            namedColors.Add("DarkSlateBlue", new NamedColor("DarkSlateBlue", Colors.DarkSlateBlue));
            namedColors.Add("DarkSlateGray", new NamedColor("DarkSlateGray", Colors.DarkSlateGray));
            namedColors.Add("DarkTurquoise", new NamedColor("DarkTurquoise", Colors.DarkTurquoise));
            namedColors.Add("DarkViolet", new NamedColor("DarkViolet", Colors.DarkViolet));
            namedColors.Add("DeepPink", new NamedColor("DeepPink", Colors.DeepPink));
            namedColors.Add("DeepSkyBlue", new NamedColor("DeepSkyBlue", Colors.DeepSkyBlue));
            namedColors.Add("DimGray", new NamedColor("DimGray", Colors.DimGray));
            namedColors.Add("DodgerBlue", new NamedColor("DodgerBlue", Colors.DodgerBlue));
            namedColors.Add("Firebrick", new NamedColor("Firebrick", Colors.Firebrick));
            namedColors.Add("FloralWhite", new NamedColor("FloralWhite", Colors.FloralWhite));
            namedColors.Add("ForestGreen", new NamedColor("ForestGreen", Colors.ForestGreen));
            namedColors.Add("Fuchsia", new NamedColor("Fuchsia", Colors.Fuchsia));
            namedColors.Add("Gainsboro", new NamedColor("Gainsboro", Colors.Gainsboro));
            namedColors.Add("GhostWhite", new NamedColor("GhostWhite", Colors.GhostWhite));
            namedColors.Add("Gold", new NamedColor("Gold", Colors.Gold));
            namedColors.Add("Goldenrod", new NamedColor("Goldenrod", Colors.Goldenrod));
            namedColors.Add("Gray", new NamedColor("Gray", Colors.Gray));
            namedColors.Add("Green", new NamedColor("Green", Colors.Green));
            namedColors.Add("GreenYellow", new NamedColor("GreenYellow", Colors.GreenYellow));
            namedColors.Add("Honeydew", new NamedColor("Honeydew", Colors.Honeydew));
            namedColors.Add("HotPink", new NamedColor("HotPink", Colors.HotPink));
            namedColors.Add("IndianRed", new NamedColor("IndianRed", Colors.IndianRed));
            namedColors.Add("Indigo", new NamedColor("Indigo", Colors.Indigo));
            namedColors.Add("Ivory", new NamedColor("Ivory", Colors.Ivory));
            namedColors.Add("Khaki", new NamedColor("Khaki", Colors.Khaki));
            namedColors.Add("Lavender", new NamedColor("Lavender", Colors.Lavender));
            namedColors.Add("LavenderBlush", new NamedColor("LavenderBlush", Colors.LavenderBlush));
            namedColors.Add("LawnGreen", new NamedColor("LawnGreen", Colors.LawnGreen));
            namedColors.Add("LemonChiffon", new NamedColor("LemonChiffon", Colors.LemonChiffon));
            namedColors.Add("LightBlue", new NamedColor("LightBlue", Colors.LightBlue));
            namedColors.Add("LightCoral", new NamedColor("LightCoral", Colors.LightCoral));
            namedColors.Add("LightCyan", new NamedColor("LightCyan", Colors.LightCyan));
            namedColors.Add("LightGoldenrodYellow", new NamedColor("LightGoldenrodYellow", Colors.LightGoldenrodYellow));
            namedColors.Add("LightGray", new NamedColor("LightGray", Colors.LightGray));
            namedColors.Add("LightGreen", new NamedColor("LightGreen", Colors.LightGreen));
            namedColors.Add("LightPink", new NamedColor("LightPink", Colors.LightPink));
            namedColors.Add("LightSalmon", new NamedColor("LightSalmon", Colors.LightSalmon));
            namedColors.Add("LightSeaGreen", new NamedColor("LightSeaGreen", Colors.LightSeaGreen));
            namedColors.Add("LightSkyBlue", new NamedColor("LightSkyBlue", Colors.LightSkyBlue));
            namedColors.Add("LightSlateGray", new NamedColor("LightSlateGray", Colors.LightSlateGray));
            namedColors.Add("LightSteelBlue", new NamedColor("LightSteelBlue", Colors.LightSteelBlue));
            namedColors.Add("LightYellow", new NamedColor("LightYellow", Colors.LightYellow));
            namedColors.Add("Lime", new NamedColor("Lime", Colors.Lime));
            namedColors.Add("LimeGreen", new NamedColor("LimeGreen", Colors.LimeGreen));
            namedColors.Add("Linen", new NamedColor("Linen", Colors.Linen));
            namedColors.Add("Magenta", new NamedColor("Magenta", Colors.Magenta));
            namedColors.Add("Maroon", new NamedColor("Maroon", Colors.Maroon));
            namedColors.Add("MediumAquamarine", new NamedColor("MediumAquamarine", Colors.MediumAquamarine));
            namedColors.Add("MediumBlue", new NamedColor("MediumBlue", Colors.MediumBlue));
            namedColors.Add("MediumOrchid", new NamedColor("MediumOrchid", Colors.MediumOrchid));
            namedColors.Add("MediumPurple", new NamedColor("MediumPurple", Colors.MediumPurple));
            namedColors.Add("MediumSeaGreen", new NamedColor("MediumSeaGreen", Colors.MediumSeaGreen));
            namedColors.Add("MediumSlateBlue", new NamedColor("MediumSlateBlue", Colors.MediumSlateBlue));
            namedColors.Add("MediumSpringGreen", new NamedColor("MediumSpringGreen", Colors.MediumSpringGreen));
            namedColors.Add("MediumTurquoise", new NamedColor("MediumTurquoise", Colors.MediumTurquoise));
            namedColors.Add("MediumVioletRed", new NamedColor("MediumVioletRed", Colors.MediumVioletRed));
            namedColors.Add("MidnightBlue", new NamedColor("MidnightBlue", Colors.MidnightBlue));
            namedColors.Add("MintCream", new NamedColor("MintCream", Colors.MintCream));
            namedColors.Add("MistyRose", new NamedColor("MistyRose", Colors.MistyRose));
            namedColors.Add("Moccasin", new NamedColor("Moccasin", Colors.Moccasin));
            namedColors.Add("NavajoWhite", new NamedColor("NavajoWhite", Colors.NavajoWhite));
            namedColors.Add("Navy", new NamedColor("Navy", Colors.Navy));
            namedColors.Add("OldLace", new NamedColor("OldLace", Colors.OldLace));
            namedColors.Add("Olive", new NamedColor("Olive", Colors.Olive));
            namedColors.Add("OliveDrab", new NamedColor("OliveDrab", Colors.OliveDrab));
            namedColors.Add("Orange", new NamedColor("Orange", Colors.Orange));
            namedColors.Add("OrangeRed", new NamedColor("OrangeRed", Colors.OrangeRed));
            namedColors.Add("Orchid", new NamedColor("Orchid", Colors.Orchid));
            namedColors.Add("PaleGoldenrod", new NamedColor("PaleGoldenrod", Colors.PaleGoldenrod));
            namedColors.Add("PaleGreen", new NamedColor("PaleGreen", Colors.PaleGreen));
            namedColors.Add("PaleTurquoise", new NamedColor("PaleTurquoise", Colors.PaleTurquoise));
            namedColors.Add("PaleVioletRed", new NamedColor("PaleVioletRed", Colors.PaleVioletRed));
            namedColors.Add("PapayaWhip", new NamedColor("PapayaWhip", Colors.PapayaWhip));
            namedColors.Add("PeachPuff", new NamedColor("PeachPuff", Colors.PeachPuff));
            namedColors.Add("Peru", new NamedColor("Peru", Colors.Peru));
            namedColors.Add("Pink", new NamedColor("Pink", Colors.Pink));
            namedColors.Add("Plum", new NamedColor("Plum", Colors.Plum));
            namedColors.Add("PowderBlue", new NamedColor("PowderBlue", Colors.PowderBlue));
            namedColors.Add("Purple", new NamedColor("Purple", Colors.Purple));
            namedColors.Add("Red", new NamedColor("Red", Colors.Red));
            namedColors.Add("RosyBrown", new NamedColor("RosyBrown", Colors.RosyBrown));
            namedColors.Add("RoyalBlue", new NamedColor("RoyalBlue", Colors.RoyalBlue));
            namedColors.Add("SaddleBrown", new NamedColor("SaddleBrown", Colors.SaddleBrown));
            namedColors.Add("Salmon", new NamedColor("Salmon", Colors.Salmon));
            namedColors.Add("SandyBrown", new NamedColor("SandyBrown", Colors.SandyBrown));
            namedColors.Add("SeaGreen", new NamedColor("SeaGreen", Colors.SeaGreen));
            namedColors.Add("SeaShell", new NamedColor("SeaShell", Colors.SeaShell));
            namedColors.Add("Sienna", new NamedColor("Sienna", Colors.Sienna));
            namedColors.Add("Silver", new NamedColor("Silver", Colors.Silver));
            namedColors.Add("SkyBlue", new NamedColor("SkyBlue", Colors.SkyBlue));
            namedColors.Add("SlateBlue", new NamedColor("SlateBlue", Colors.SlateBlue));
            namedColors.Add("SlateGray", new NamedColor("SlateGray", Colors.SlateGray));
            namedColors.Add("Snow", new NamedColor("Snow", Colors.Snow));
            namedColors.Add("SpringGreen", new NamedColor("SpringGreen", Colors.SpringGreen));
            namedColors.Add("SteelBlue", new NamedColor("SteelBlue", Colors.SteelBlue));
            namedColors.Add("Tan", new NamedColor("Tan", Colors.Tan));
            namedColors.Add("Teal", new NamedColor("Teal", Colors.Teal));
            namedColors.Add("Thistle", new NamedColor("Thistle", Colors.Thistle));
            namedColors.Add("Tomato", new NamedColor("Tomato", Colors.Tomato));
            namedColors.Add("Transparent", new NamedColor("Transparent", Colors.Transparent));
            namedColors.Add("Turquoise", new NamedColor("Turquoise", Colors.Turquoise));
            namedColors.Add("Violet", new NamedColor("Violet", Colors.Violet));
            namedColors.Add("Wheat", new NamedColor("Wheat", Colors.Wheat));
            namedColors.Add("White", new NamedColor("White", Colors.White));
            namedColors.Add("WhiteSmoke", new NamedColor("WhiteSmoke", Colors.WhiteSmoke));
            namedColors.Add("Yellow", new NamedColor("Yellow", Colors.Yellow));
            namedColors.Add("YellowGreen", new NamedColor("YellowGreen", Colors.YellowGreen));

            return new ReadOnlyDictionary<string, NamedColor>(namedColors);
        }
    }
}
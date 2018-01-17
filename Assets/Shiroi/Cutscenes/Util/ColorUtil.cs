using UnityEngine;

namespace Shiroi.Cutscenes.Util {
    public static class ColorUtil {
        public static Color FromHex(string hex, byte alpha = byte.MaxValue) {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            var r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            return new Color32(r, g, b, alpha);
        }

        public static string ToHex(this Color color) {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }
    }
}
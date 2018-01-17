using System;
using System.Text;

namespace Shiroi.Cutscenes.Util {
    public static class Base64Util {
        public static byte[] GetBytes(this string str, Encoding enconding) {
            return enconding.GetBytes(str);
        }

        public static byte[] GetBytes(this string str) {
            return str.GetBytes(Encoding.UTF8);
        }

        public static string GetString(this byte[] str, Encoding enconding) {
            return enconding.GetString(str);
        }

        public static string GetString(this byte[] str) {
            return str.GetString(Encoding.UTF8);
        }

        public static byte[] Base64Encode(string plainText) {
            var plainTextBytes = plainText.GetBytes();
            return Convert.ToBase64String(plainTextBytes).GetBytes();
        }

        public static string Base64Decode(byte[] base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData.GetString());
            return base64EncodedBytes.GetString();
        }
    }
}
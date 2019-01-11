using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shiroi.Cutscenes.Util {
    public static class TypeUtil {
        public static bool IsInstanceOfGenericType(Type genericType, Type type) {
            while (type != null) {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType) {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Type name alias lookup.
        /// TypeNameAlternatives["Single"] will give you "float", "UInt16" will give you "ushort", "Boolean[]" will give you "bool[]" etc..
        /// </summary>
        public static readonly Dictionary<string, string> TypeNameAlternatives = new Dictionary<string, string> {
            {"Single", "float"},
            {"Double", "double"},
            {"SByte", "sbyte"},
            {"Int16", "short"},
            {"Int32", "int"},
            {"Int64", "long"},
            {"Byte", "byte"},
            {"UInt16", "ushort"},
            {"UInt32", "uint"},
            {"UInt64", "ulong"},
            {"Decimal", "decimal"},
            {"String", "string"},
            {"Char", "char"},
            {"Boolean", "bool"},
            {"Single[]", "float[]"},
            {"Double[]", "double[]"},
            {"SByte[]", "sbyte[]"},
            {"Int16[]", "short[]"},
            {"Int32[]", "int[]"},
            {"Int64[]", "long[]"},
            {"Byte[]", "byte[]"},
            {"UInt16[]", "ushort[]"},
            {"UInt32[]", "uint[]"},
            {"UInt64[]", "ulong[]"},
            {"Decimal[]", "decimal[]"},
            {"String[]", "string[]"},
            {"Char[]", "char[]"},
            {"Boolean[]", "bool[]"},
        };

        private static string TypeNameGauntlet(this Type type) {
            string typeName = type.Name;

            string altTypeName = string.Empty;

            if (TypeNameAlternatives.TryGetValue(typeName, out altTypeName)) {
                typeName = altTypeName;
            }

            return typeName;
        }

        public static string GetNiceName(this Type type) {
            if (type.IsArray) {
                int rank = type.GetArrayRank();
                return type.GetElementType().GetNiceName() + (rank == 1 ? "[]" : "[,]");
            }

            if (type.InheritsFrom(typeof(Nullable<>))) {
                return type.GetGenericArguments()[0].GetNiceName() + "?";
            }

            if (type.IsByRef) {
                return "ref " + type.GetElementType().GetNiceName();
            }

            if (type.IsGenericParameter || !type.IsGenericType) {
                return TypeNameGauntlet(type);
            }

            var builder = new StringBuilder();
            var name = type.Name;
            var index = name.IndexOf("`");

            if (index != -1) {
                builder.Append(name.Substring(0, index));
            } else {
                builder.Append(name);
            }

            builder.Append('<');
            var args = type.GetGenericArguments();

            for (int i = 0; i < args.Length; i++) {
                var arg = args[i];

                if (i != 0) {
                    builder.Append(", ");
                }

                builder.Append(GetNiceName(arg));
            }

            builder.Append('>');
            return builder.ToString();
        }

        public static bool InheritsFrom(this Type type, Type baseType) {
            if (baseType.IsAssignableFrom(type)) {
                return true;
            }

            if (type.IsInterface && baseType.IsInterface == false) {
                return false;
            }

            if (baseType.IsInterface) {
                return type.GetInterfaces().Any(tt => baseType == tt);
            }

            var t = type;
            while (t != null) {
                if (t == baseType) {
                    return true;
                }

                if (baseType.IsGenericTypeDefinition && t.IsGenericType && t.GetGenericTypeDefinition() == baseType) {
                    return true;
                }

                t = t.BaseType;
            }

            return false;
        }
    }
}
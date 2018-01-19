using System;

namespace Shiroi.Cutscenes.Editor.Util {
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
    }
}
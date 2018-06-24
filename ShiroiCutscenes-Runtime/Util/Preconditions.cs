using System;
using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Util {
    public static class Preconditions {
        public static void AssertIs<T>(object obj, string message) {
            if (!(obj is T)) {
                throw new ArgumentException(message);
            }
        }

        public static void AssertIs<T>(string paramName, object obj) {
            if (!(obj is T)) {
                throw new ArgumentException("Parameter " + paramName + " needs to implement " + typeof(T).Name);
            }
        }
    }
}
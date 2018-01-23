using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public static class SerializationUtil {
        public static FieldInfo[] GetSerializedMembers(object obj, bool publicOnly = false) {
            return GetSerializedMembers(obj.GetType(), publicOnly);
        }

        public static FieldInfo[] GetSerializedMembers(Type type, bool publicOnly = false) {
            BindingFlags flags;
            if (publicOnly) {
                flags = BindingFlags.Public | BindingFlags.Instance;
            } else {
                flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            }
            var members = type.GetFields(flags);
            var result = new List<FieldInfo>();
            foreach (var memberInfo in members) {
                if (ShouldSerialize(memberInfo)) {
                    result.Add(memberInfo);
                }
            }
            return result.ToArray();
        }

        private static bool ShouldSerialize(FieldInfo memberInfo) {
            return !memberInfo.IsPrivate || Attribute.GetCustomAttribute(memberInfo, typeof(SerializeField)) != null;
        }
    }
}
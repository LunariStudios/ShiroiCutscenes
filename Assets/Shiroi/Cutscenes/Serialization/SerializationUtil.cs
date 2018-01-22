using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public static class SerializationUtil {
        public static FieldInfo[] GetSerializedMembers(object obj) {
            return GetSerializedMembers(obj.GetType());
        }

        public static FieldInfo[] GetSerializedMembers(Type type) {
            var members = type.GetFields();
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
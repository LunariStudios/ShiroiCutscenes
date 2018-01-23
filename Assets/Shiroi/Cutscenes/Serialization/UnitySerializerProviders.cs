using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public class ExposedReferenceSerializerProvider : SerializerProvider {
        private static readonly Type ExposedReferenceType = typeof(ExposedReferenceSerializer<>);

        public override bool Supports(Type type) {
            return type.IsGenericType && TypeUtil.IsInstanceOfGenericType(typeof(ExposedReference<>), type);
        }

        public override Serializer Provide(Type type) {
            var genericType = type.GetGenericArguments()[0];
            return (Serializer) Activator.CreateInstance(ExposedReferenceType.MakeGenericType(genericType));
        }
    }
}
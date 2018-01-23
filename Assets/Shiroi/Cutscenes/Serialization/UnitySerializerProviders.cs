using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public class GenericSerializerProvider : SerializerProvider {
        private readonly Type supportedType;
        private readonly Type serializerType;
        public GenericSerializerProvider(Type supportedType, Type serializerType) {
            this.supportedType = supportedType;
            this.serializerType = serializerType;
        }

        public override bool Supports(Type type) {
            return type.IsGenericType && TypeUtil.IsInstanceOfGenericType(supportedType, type);
        }

        public override Serializer Provide(Type type) {
            var genericType = type.GetGenericArguments()[0];
            return (Serializer) Activator.CreateInstance(serializerType.MakeGenericType(genericType));
        }
    }
}
using System;

namespace Shiroi.Cutscenes.Serialization {
    public abstract class SerializerProvider {
        public abstract bool Supports(Type type);
        public abstract Serializer Provide(Type type);
    }
}
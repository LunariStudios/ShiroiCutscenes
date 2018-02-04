using System;

namespace Shiroi.Cutscenes.Serialization {
    public abstract class Serializer : IComparable<Serializer> {
        public abstract bool Supports(Type type);
        public abstract void Serialize(object value, string name, SerializedObject destination);
        public abstract object Deserialize(string key, SerializedObject obj, Type fieldType);

        public virtual int GetPriority() {
            return 0;
        }

        public int CompareTo(Serializer other) {
            return GetPriority().CompareTo(other.GetPriority());
        }
    }

    public abstract class Serializer<T> : Serializer {
        private readonly Type supportedType;

        public override bool Supports(Type type) {
            return supportedType.IsAssignableFrom(type);
        }

        protected Serializer() {
            this.supportedType = typeof(T);
        }

        public override void Serialize(object value, string name, SerializedObject destination) {
            Serialize((T) value, name, destination);
        }


        public abstract void Serialize(T value, string name, SerializedObject destination);
    }
}
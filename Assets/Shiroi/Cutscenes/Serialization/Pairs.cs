using System;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Serialization {
    [Serializable]
    public class BooleanPair : Pair<bool> {
        public BooleanPair() { }
        public BooleanPair(string key, bool value) : base(key, value) { }
    }

    [Serializable]
    public class IntPair : Pair<int> {
        public IntPair() { }
        public IntPair(string key, int value) : base(key, value) { }
    }

    [Serializable]
    public class FloatPair : Pair<float> {
        public FloatPair() { }
        public FloatPair(string key, float value) : base(key, value) { }
    }

    [Serializable]
    public class StringPair : Pair<string> {
        public StringPair() { }
        public StringPair(string key, string value) : base(key, value) { }
    }

    [Serializable]
    public class ObjectPair : Pair<SerializedObject> {
        public ObjectPair() { }
        public ObjectPair(string key, SerializedObject value) : base(key, value) { }
    }

    [Serializable]
    public class ArrayPair : Pair<SerializedObject[]> {
        public ArrayPair() { }
        public ArrayPair(string key, SerializedObject[] value) : base(key, value) { }
    }

    [Serializable]
    public class UnityPair : Pair<Object> {
        public UnityPair() { }
        public UnityPair(string key, Object value) : base(key, value) { }
    }
}
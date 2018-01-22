using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Cutscenes.Util;

namespace Shiroi.Cutscenes.Serialization {
    [Serializable]
    public sealed class SerializedObject {
        //Pairs
        public class BooleanPair : Pair<bool> {
            public BooleanPair() { }
            public BooleanPair(string key, bool value) : base(key, value) { }
        }

        public class IntPair : Pair<int> {
            public IntPair() { }
            public IntPair(string key, int value) : base(key, value) { }
        }

        public class FloatPair : Pair<float> {
            public FloatPair() { }
            public FloatPair(string key, float value) : base(key, value) { }
        }

        public class StringPair : Pair<string> {
            public StringPair() { }
            public StringPair(string key, string value) : base(key, value) { }
        }

        public class ObjectPair : Pair<SerializedObject> {
            public ObjectPair() { }
            public ObjectPair(string key, SerializedObject value) : base(key, value) { }
        }

        public abstract class Pair<T> {
            protected Pair() { }

            protected Pair(string key, T value) {
                Key = key;
                Value = value;
            }

            public string Key;
            public T Value;
        }

        //Lists
        public List<BooleanPair> Booleans = new List<BooleanPair>();

        public List<IntPair> Ints = new List<IntPair>();

        public List<FloatPair> Floats = new List<FloatPair>();

        public List<StringPair> Strings = new List<StringPair>();

        public List<ObjectPair> Objects = new List<ObjectPair>();

        //Implementation
        public void SetBoolean(string key, bool value) {
            Booleans.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new BooleanPair(key, value)).Value = value;
        }

        public void SetInt(string key, int value) {
            Ints.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new IntPair(key, value)).Value = value;
        }

        public void SetFloat(string key, float value) {
            Floats.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new FloatPair(key, value)).Value = value;
        }

        public void SetString(string key, string value) {
            Strings.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new StringPair(key, value)).Value = value;
        }

        public void SetObject(string key, SerializedObject value) {
            Objects.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new ObjectPair(key, value)).Value = value;
        }

        public bool GetBoolean(string key) {
            return Booleans.First(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public int GetInt(string key) {
            return Ints.First(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public float GetFloat(string key) {
            return Floats.First(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public string GetString(string key) {
            return Strings.First(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public SerializedObject GetObject(string key) {
            return Objects.First(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }
    }
}
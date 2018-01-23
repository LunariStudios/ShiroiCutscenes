﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Serialization {
    [Serializable]
    public sealed class SerializedObject {
        //Pairs
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

        public List<UnityPair> UnityObjects = new List<UnityPair>();
        public List<ArrayPair> Arrays = new List<ArrayPair>();

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

        public void SetUnity(string key, Object value) {
            UnityObjects.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new UnityPair(key, value)).Value = value;
        }

        public void SetArray(string key, IEnumerable<SerializedObject> value) {
            SetArray(key, value.ToArray());
        }

        public void SetArray(string key, SerializedObject[] value) {
            Arrays.GetOrPut(pair => pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase),
                () => new ArrayPair(key, value)).Value = value;
        }

        public bool GetBoolean(string key) {
            BooleanPair first = null;
            foreach (var pair in Booleans) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<bool>(key);
            }
            return first.Value;
        }

        private static T NotifyMissing<T>(string key) {
            var value = default(T);
            Debug.LogWarningFormat("Couldn't find {2} for '{0}' returning '{1}' ...", key, value, typeof(T).Name);
            return value;
        }

        public int GetInt(string key) {
            IntPair first = null;
            foreach (var pair in Ints) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<int>(key);
            }
            return first.Value;
        }

        public float GetFloat(string key) {
            FloatPair first = null;
            foreach (var pair in Floats) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<float>(key);
            }
            return first.Value;
        }

        public string GetString(string key) {
            StringPair first = null;
            foreach (var pair in Strings) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<string>(key);
            }
            return first.Value;
        }

        public SerializedObject[] GetArray(string key) {
            ArrayPair first = null;
            foreach (var pair in Arrays) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<SerializedObject[]>(key);
            }
            return first.Value;
        }

        public SerializedObject GetObject(string key) {
            ObjectPair first = null;
            foreach (var pair in Objects) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<SerializedObject>(key);
            }
            return first.Value;
        }

        public Object GetUnityObject(string key) {
            UnityPair first = null;
            foreach (var pair in UnityObjects) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }
            if (first == null) {
                return NotifyMissing<Object>(key);
            }
            return first.Value;
        }

        public void Deserialize(IToken token) {
            var type = token.GetType();
            foreach (var member in SerializationUtil.GetSerializedMembers(type)) {
                var fieldType = member.FieldType;
                var name = member.Name;
                var serializer = Serializers.For(fieldType);
                if (serializer == null) {
                    Debug.LogWarningFormat("Couldn't find serializer for member '{0}' of type '{1}'", name,
                        fieldType.FullName);
                    continue;
                }
                var value = serializer.Deserialize(name, this);
                member.SetValue(token, value);
            }
        }

        public static SerializedObject From(object loadedToken) {
            var type = loadedToken.GetType();
            var obj = new SerializedObject();
            foreach (var member in SerializationUtil.GetSerializedMembers(type)) {
                var serializer = Serializers.For(member.FieldType);
                if (serializer == null) {
                    continue;
                }
                var value = member.GetValue(loadedToken);
                serializer.Serialize(value, member.Name, obj);
            }
            return obj;
        }
    }
}
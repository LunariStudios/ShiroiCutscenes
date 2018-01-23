﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Serialization {
    public class JsonSerializer : Serializer {
        public const string TypeKey = "Type";
        public const string DataKey = "Data";

        public override bool Supports(Type type) {
            return true;
        }

        public override int GetPriority() {
            return -1;
        }

        public override void Serialize(object value, string name, SerializedObject destination) {
            if (value == null) {
                return;
            }
            var obj = new SerializedObject();
            string json;
#if UNITY_EDITOR
            json = EditorJsonUtility.ToJson(value);
#else
json = JsonUtility.ToJson(value);
#endif

            obj.SetString(TypeKey, value.GetType().AssemblyQualifiedName);
            obj.SetString(DataKey, json);
            destination.SetObject(name, obj);
        }

        public override object Deserialize(string key, SerializedObject obj) {
            var data = obj.GetObject(key);
            if (data == null) {
                Debug.LogWarningFormat("Couldn't find data for object '{0}'", key);
                return null;
            }
            var typeName = data.GetString(TypeKey);
            if (string.IsNullOrEmpty(typeName)) {
                Debug.LogWarningFormat("Couldn't find the type name for object '{0}'", key);
                return null;
            }
            var type = Type.GetType(typeName);
            if (type == null) {
                Debug.LogWarningFormat("Couldn't find type '{0}' for object '{1}'", typeName, key);
                return null;
            }
            var json = data.GetString(DataKey);
            return JsonUtility.FromJson(json, type);
        }
    }

    public class ExposedReferenceSerializer<T> : Serializer<ExposedReference<T>> where T : Object {
        public const string IdKey = "Id";
        public const string DefaultKey = "Default";

        public override int GetPriority() {
            return 1;
        }

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedReference = obj.GetObject(key);
            var id = serializedReference.GetInt(IdKey);
            var property = new PropertyName(id);
            var defaultObj = serializedReference.GetUnityObject(DefaultKey);
            return new ExposedReference<T> {
                defaultValue = defaultObj,
                exposedName = property
            };
        }

        public override void Serialize(ExposedReference<T> value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetInt(IdKey, value.exposedName.GetHashCode());
            obj.SetUnity(DefaultKey, value.defaultValue);
            destination.SetObject(name, obj);
        }
    }

    public class ObjectSerializer : Serializer<Object> {
        public override object Deserialize(string key, SerializedObject obj) {
            return obj.GetUnityObject(key);
        }

        public override void Serialize(Object value, string name, SerializedObject destination) {
            destination.SetUnity(name, value);
        }
    }

    public class Vector2Serializer : Serializer<Vector2> {
        public const string XKey = "x";
        public const string YKey = "y";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedVector = obj.GetObject(key);
            var x = serializedVector.GetFloat(XKey);
            var y = serializedVector.GetFloat(YKey);
            return new Vector2(x, y);
        }

        public override void Serialize(Vector2 value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(XKey, value.x);
            obj.SetFloat(YKey, value.y);
            destination.SetObject(name, obj);
        }
    }

    public class Vector3Serializer : Serializer<Vector3> {
        public const string ZKey = "z";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedVector = obj.GetObject(key);
            var x = serializedVector.GetFloat(Vector2Serializer.XKey);
            var y = serializedVector.GetFloat(Vector2Serializer.YKey);
            var z = serializedVector.GetFloat(ZKey);
            return new Vector3(x, y, z);
        }

        public override void Serialize(Vector3 value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(Vector2Serializer.XKey, value.x);
            obj.SetFloat(Vector2Serializer.YKey, value.y);
            obj.SetFloat(ZKey, value.z);
            destination.SetObject(name, obj);
        }
    }

    public class Vector4Serializer : Serializer<Vector4> {
        public const string WKey = "w";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedVector = obj.GetObject(key);
            var x = serializedVector.GetFloat(Vector2Serializer.XKey);
            var y = serializedVector.GetFloat(Vector2Serializer.YKey);
            var z = serializedVector.GetFloat(Vector3Serializer.ZKey);
            var w = serializedVector.GetFloat(WKey);
            return new Vector4(x, y, z, w);
        }

        public override void Serialize(Vector4 value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(Vector2Serializer.XKey, value.x);
            obj.SetFloat(Vector2Serializer.YKey, value.y);
            obj.SetFloat(Vector3Serializer.ZKey, value.z);
            obj.SetFloat(WKey, value.w);
            destination.SetObject(name, obj);
        }
    }

    public class Vector2IntSerializer : Serializer<Vector2Int> {
        public override object Deserialize(string key, SerializedObject obj) {
            var serializedVector = obj.GetObject(key);
            var x = serializedVector.GetInt(Vector2Serializer.XKey);
            var y = serializedVector.GetInt(Vector2Serializer.YKey);
            return new Vector2Int(x, y);
        }

        public override void Serialize(Vector2Int value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetInt(Vector2Serializer.XKey, value.x);
            obj.SetInt(Vector2Serializer.YKey, value.y);
            destination.SetObject(name, obj);
        }
    }

    public class Vector3IntSerializer : Serializer<Vector3Int> {
        public override object Deserialize(string key, SerializedObject obj) {
            var serializedVector = obj.GetObject(key);
            var x = serializedVector.GetInt(Vector2Serializer.XKey);
            var y = serializedVector.GetInt(Vector2Serializer.YKey);
            var z = serializedVector.GetInt(Vector3Serializer.ZKey);
            return new Vector3Int(x, y, z);
        }

        public override void Serialize(Vector3Int value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetInt(Vector2Serializer.XKey, value.x);
            obj.SetInt(Vector2Serializer.YKey, value.y);
            obj.SetInt(Vector3Serializer.ZKey, value.z);
            destination.SetObject(name, obj);
        }
    }

    public class QuaternionSerializer : Serializer<Quaternion> {
        public const string XKey = "x";
        public const string YKey = "y";
        public const string ZKey = "z";
        public const string WKey = "w";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedQuaternion = obj.GetObject(key);
            float x, y, z, w;
            x = serializedQuaternion.GetFloat(XKey);
            y = serializedQuaternion.GetFloat(YKey);
            z = serializedQuaternion.GetFloat(ZKey);
            w = serializedQuaternion.GetFloat(WKey);
            return new Quaternion(x, y, z, w);
        }

        public override void Serialize(Quaternion value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(XKey, value.x);
            obj.SetFloat(YKey, value.y);
            obj.SetFloat(ZKey, value.z);
            obj.SetFloat(WKey, value.w);
            destination.SetObject(name, obj);
        }
    }

    public class ColorSerializer : Serializer<Color> {
        public const string RKey = "r";
        public const string GKey = "g";
        public const string BKey = "b";
        public const string AKey = "a";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedColor = obj.GetObject(key);
            float r, g, b, a;
            r = serializedColor.GetFloat(RKey);
            g = serializedColor.GetFloat(GKey);
            b = serializedColor.GetFloat(BKey);
            a = serializedColor.GetFloat(AKey);
            return new Color(r, g, b, a);
        }

        public override void Serialize(Color value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(RKey, value.r);
            obj.SetFloat(GKey, value.g);
            obj.SetFloat(BKey, value.b);
            obj.SetFloat(AKey, value.a);
            destination.SetObject(name, obj);
        }
    }

    public class AnimationCurveSerializer : Serializer<AnimationCurve> {
        public const string KeyFramesKey = "KeyFrames";
        public const string TimeKey = "Time";
        public const string ValueKey = "Value";
        public const string InTangentKey = "InTangent";
        public const string OutTangentKey = "OutTangent";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedFrames = obj.GetArray(KeyFramesKey);
            var frames = from frame in serializedFrames select DeserializeFrame(frame);
            return new AnimationCurve(frames.ToArray());
        }

        public override void Serialize(AnimationCurve value, string name, SerializedObject destination) {
            var frames = from frame in value.keys select SerializeFrame(frame);
            destination.SetArray(KeyFramesKey, frames);
        }

        private static Keyframe DeserializeFrame(SerializedObject frame) {
            var time = frame.GetFloat(TimeKey);
            var value = frame.GetFloat(ValueKey);
            var inTangent = frame.GetFloat(InTangentKey);
            var outTangent = frame.GetFloat(OutTangentKey);
            return new Keyframe(time, value, inTangent, outTangent);
        }

        private static SerializedObject SerializeFrame(Keyframe frame) {
            var obj = new SerializedObject();
            obj.SetFloat(TimeKey, frame.time);
            obj.SetFloat(ValueKey, frame.value);
            obj.SetFloat(InTangentKey, frame.inTangent);
            obj.SetFloat(OutTangentKey, frame.outTangent);
            return obj;
        }
    }

    public class RectSerializer : Serializer<Rect> {
        public const string XKey = "x";
        public const string YKey = "y";
        public const string WKey = "w";
        public const string HKey = "h";

        public override object Deserialize(string key, SerializedObject obj) {
            var serializedRect = obj.GetObject(key);
            var x = serializedRect.GetFloat(XKey);
            var y = serializedRect.GetFloat(YKey);
            var w = serializedRect.GetFloat(WKey);
            var h = serializedRect.GetFloat(HKey);
            return new Rect(x, y, w, h);
        }

        public override void Serialize(Rect value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(XKey, value.x);
            obj.SetFloat(YKey, value.y);
            obj.SetFloat(WKey, value.width);
            obj.SetFloat(HKey, value.height);
            destination.SetObject(name, obj);
        }
    }

    public class RectIntSerializer : Serializer<RectInt> {
        public override object Deserialize(string key, SerializedObject obj) {
            var serializedRect = obj.GetObject(key);
            var x = serializedRect.GetInt(RectSerializer.XKey);
            var y = serializedRect.GetInt(RectSerializer.YKey);
            var w = serializedRect.GetInt(RectSerializer.WKey);
            var h = serializedRect.GetInt(RectSerializer.HKey);
            return new RectInt(x, y, w, h);
        }

        public override void Serialize(RectInt value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetInt(RectSerializer.XKey, value.x);
            obj.SetInt(RectSerializer.YKey, value.y);
            obj.SetInt(RectSerializer.WKey, value.width);
            obj.SetInt(RectSerializer.HKey, value.height);
            destination.SetObject(name, obj);
        }
    }
}
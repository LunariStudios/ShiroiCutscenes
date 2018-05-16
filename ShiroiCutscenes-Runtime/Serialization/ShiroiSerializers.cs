using System;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Util;
using Shiroi.Serialization;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Serialization {
    public class FutureReferenceSerializer<T> : Serializer<FutureReference<T>> where T : Object {
        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            return new FutureReference<T>(obj.GetInt(key));
        }

        public override void Serialize(FutureReference<T> value, string name, SerializedObject destination) {
            destination.SetInt(name, value.Id);
        }
    }

    public class ReferenceSerializer<T> : Serializer<Reference<T>> where T : Object {
        public const string ReferenceTypeKey = "Type";
        public const string ReferenceValueKey = "Value";

        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            var serializedReference = obj.GetObject(key);
            var type = (Reference.ReferenceType) serializedReference.GetInt(ReferenceTypeKey);
            var value = serializedReference.GetInt(ReferenceValueKey);
            return new Reference<T>(type, value);
        }

        public override void Serialize(Reference<T> value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            var type = value.Type;
            obj.SetInt(ReferenceValueKey, value.Id);
            obj.SetInt(ReferenceTypeKey, (int) type);
            destination.SetObject(name, obj);
        }
    }
}
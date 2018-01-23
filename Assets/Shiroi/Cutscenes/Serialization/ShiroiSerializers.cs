using Shiroi.Cutscenes.Futures;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    public class FutureReferenceSerializer<T> : Serializer<FutureReference<T>> where T : Object {
        public override object Deserialize(string key, SerializedObject obj) {
            return new FutureReference<T>(obj.GetInt(key));
        }

        public override void Serialize(FutureReference<T> value, string name, SerializedObject destination) {
            destination.SetInt(name, value.Id);
        }
    }
}
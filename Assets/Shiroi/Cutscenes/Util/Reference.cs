using UnityEngine;

namespace Shiroi.Cutscenes.Util {
    public struct Reference<T> where T : Object{
        public enum ReferenceType : byte{
            Future = 0,
            Exposed = 1
        }

        public ReferenceType Type;
        public int Id;

        public Reference(ReferenceType type, int id) : this() {
            Type = type;
            Id = id;
        }
        
        public T Resolve(CutscenePlayer player) {
            switch (Type) {
                case ReferenceType.Exposed:
                    bool idValid;
                    Object referenceValue = player.GetReferenceValue(PropertyName, out idValid);
                    if (idValid) { 
                        return referenceValue as T;
                    } else { 
                        return null;
                    }
                case ReferenceType.Future:
                    return player.RequestFuture<T>(Id);
            }
            return null;
        }

        public PropertyName PropertyName {
            get {
                return new PropertyName(Id); 
            } 
        }

        public static implicit operator ExposedReference<T>(Reference<T> reference) {
            var refe = new ExposedReference<T>();
            refe.exposedName = reference.PropertyName;
            return refe;
        }
    }
}
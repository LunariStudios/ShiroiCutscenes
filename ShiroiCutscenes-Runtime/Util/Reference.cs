using UnityEngine;

namespace Shiroi.Cutscenes.Util {
    public class Reference {
        public enum ReferenceType : byte{
            Future = 0,
            Exposed = 1
        }

        public ReferenceType Type;
        public int Id;

        public Reference(ReferenceType type, int id)  {
            Type = type;
            Id = id;
        }
        public Object Resolve(CutscenePlayer player) {
            switch (Type) {
                case ReferenceType.Exposed:
                    bool idValid;
                    Object referenceValue = player.GetReferenceValue(PropertyName, out idValid);
                    if (idValid) { 
                        return referenceValue;
                    } else { 
                        return null;
                    }
                case ReferenceType.Future:
                    return player.RequestFuture(Id);
            }
            return null;
        }

        public PropertyName PropertyName {
            get {
                return new PropertyName(Id); 
            } 
        }
    }
    
    public sealed class Reference<T> : Reference where T : Object{
        public Reference(ReferenceType type, int id) : base(type, id) { }

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


        public static implicit operator ExposedReference<T>(Reference<T> reference) {
            var refe = new ExposedReference<T>();
            refe.exposedName = reference.PropertyName;
            return refe;
        }

    }
}
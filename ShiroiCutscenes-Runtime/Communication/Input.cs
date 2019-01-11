using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Communication {
    public abstract class Input {
        [SerializeField]
        private string name;

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        public abstract bool IsCompatibleWith(Output output);
        public abstract bool Get(Context context, out object result);
    }

    public abstract class Input<T> : Input {
        public override bool IsCompatibleWith(Output output) {
            var vt = output.GetOutputType();
            var tt = typeof(T);
            return tt.IsAssignableFrom(vt);
        }

        public override bool Get(Context context, out object result) {
            T temp;
            var found = Get(context, out temp);
            result = temp;
            return found;
        }

        public bool Get(Context context, out T result) {
            var exposed = context.FindExposedWithName(Name);
            if (exposed != null) {
                return exposed.Extract(out result);
            }

            result = default(T);
            return false;
        }
    }

}
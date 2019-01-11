using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Communication {
    /// <summary>
    /// An object that can expose objects to a <see cref="Context"/> using a <see cref="Exposed"/>.
    /// </summary>
    public abstract class Output {
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

        public abstract Type GetOutputType();
    }

    public abstract class Output<T> : Output {
        public override Type GetOutputType() {
            return typeof(T);
        }

        public void Apply(T value, Context context) {
            context.Enqueue(Name, value);
        }
    }
}
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Communication {
    /// <summary>
    /// An object that contains all the information about the execution of a cutscene.
    /// </summary>
    public sealed class Context {
        private readonly List<Exposed> exposedObjects = new List<Exposed>();

        public void Enqueue(Exposed exposed) {
            exposedObjects.Add(exposed);
        }

        public Exposed FindExposedWithName(string name) {
            foreach (var exposed in exposedObjects) {
                if (exposed == name) {
                    return exposed;
                }
            }

            return null;
        }
    }

    public static class ContextExtensions {
        public static void Enqueue(this Context context, string name, object value) {
            context.Enqueue(new Exposed(name, value));
        }
    }

    /// <summary>
    /// Wraps an object (<see cref="obj"/>) using a uniquely identifiable key (<see cref="name"/>) which can be then
    /// queried by <see cref="Shiroi.Cutscenes.Tokens.Token"/>s for runtime information on the <see cref="Cutscene"/>
    /// being executed.
    /// </summary>
    public sealed class Exposed : IEquatable<Exposed> {
        private readonly PropertyName name;
        private readonly object obj;

        public Exposed(PropertyName name, object obj) {
            this.name = name;
            this.obj = obj;
        }

        public static bool operator ==([NotNull] Exposed a, string b) {
            return a.name == (PropertyName) b;
        }

        public static bool operator !=(Exposed a, string b) {
            return !(a == b);
        }

        public PropertyName Name {
            get {
                return name;
            }
        }

        public object Object {
            get {
                return obj;
            }
        }

        public bool Extract<T>(out T result) {
            if (obj is T) {
                result = (T) obj;
                return true;
            }

            result = default(T);
            return false;
        }

        public bool Equals(Exposed other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return name.Equals(other.name);
        }

        public override bool Equals(object other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return other is Exposed && Equals((Exposed) other);
        }

        public override int GetHashCode() {
            return name.GetHashCode();
        }
    }
}
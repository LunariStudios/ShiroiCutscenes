using System;
using NUnit.Compatibility;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public delegate void Setter(object value);

    public abstract class TypeDrawer {
        public abstract bool Supports(Type type);
        public abstract void Draw(Rect rect, string name, object value, Type valueType, Setter setter);
    }

    public abstract class TypeDrawer<T> : TypeDrawer {
        private readonly Type supportedType;

        public override void Draw(Rect rect, string name, object value, Type valueType, Setter setter) {
            Draw(rect, name, (T) value, valueType, setter);
        }

        protected TypeDrawer() {
            supportedType = typeof(T);

        }

        public override bool Supports(Type type) {
            return supportedType.IsAssignableFrom(type);
        }

        public abstract void Draw(Rect rect, string name, T value, Type valueType, Setter setter);
    }
}
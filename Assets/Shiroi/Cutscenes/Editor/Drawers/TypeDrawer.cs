using System;
using NUnit.Compatibility;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public delegate void Setter(object value);

    public abstract class TypeDrawer {
        public abstract bool Supports(Type type);
        public abstract void Draw(CutscenePlayer player, Rect rect, string name, object value, Type valueType, Setter setter);
    }

    public abstract class TypeDrawer<T> : TypeDrawer {
        private readonly Type supportedType;

        public override void Draw(CutscenePlayer player, Rect rect, string name, object value, Type valueType, Setter setter) {
            T finalV;
            if (value == null || value is T) {
                finalV = (T) value;
            } else {
                var msg = string.Format("[{2}] Expected an object of type {0} but got {1}! Using default value...",
                    supportedType, value, GetType().Name);
                Debug.LogWarning(msg);
                finalV = default(T);
            }
            Draw(player, rect, name, finalV, valueType, setter);
        }

        protected TypeDrawer() {
            supportedType = typeof(T);
        }

        public override bool Supports(Type type) {
            return supportedType.IsAssignableFrom(type);
        }

        public abstract void Draw(CutscenePlayer player, Rect rect, string name, T value, Type valueType, Setter setter);
    }
}
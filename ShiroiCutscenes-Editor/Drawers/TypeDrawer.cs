using System;
using System.Reflection;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public delegate void Setter(object value);

    public abstract class TypeDrawer : IComparable<TypeDrawer> {
        public abstract bool Supports(Type type);

        public abstract void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, object value, Type valueType, FieldInfo fieldInfo, Setter setter);

        public virtual int GetPriority() {
            return 0;
        }

        public virtual uint GetTotalLines() {
            return 1;
        }

        public int CompareTo(TypeDrawer other) {
            return GetPriority().CompareTo(other.GetPriority());
        }
    }

    public abstract class TypeDrawer<T> : TypeDrawer {
        private readonly Type supportedType;

        public override void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, object value, Type valueType, FieldInfo fieldInfo, Setter setter) {
            T finalV;
            if (value == null || value is T) {
                finalV = (T) value;
            } else {
                var msg = string.Format("[{2}] Expected an object of type {0} but got {1}! Using default value...",
                    supportedType, value, GetType().Name);
                Debug.LogWarning(msg);
                finalV = default(T);
            }
            Draw(editor, player, cutscene, rect, tokenIndex, name, finalV, valueType, fieldInfo, setter);
        }

        protected TypeDrawer() {
            supportedType = typeof(T);
        }

        public override bool Supports(Type type) {
            return supportedType.IsAssignableFrom(type);
        }

        public abstract void Draw(CutsceneEditor editor, CutscenePlayer player, Cutscene cutscene, Rect rect, int tokenIndex, GUIContent name, T value, Type valueType, FieldInfo fieldInfo, Setter setter);
    }
}
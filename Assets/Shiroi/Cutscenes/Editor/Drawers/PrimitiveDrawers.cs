using System;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class EnumDrawer : TypeDrawer {
        public override bool Supports(Type type) {
            return type.IsEnum;
        }

        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, object value, Type valueType, Setter setter) {
            setter(EditorGUI.EnumPopup(rect, name, (Enum) value));
        }
    }

    public class BooleanDrawer : TypeDrawer<bool> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, bool value, Type valueType, Setter setter) {
            setter(EditorGUI.Toggle(rect, name, value));
        }
    }

    public class ByteDrawer : TypeDrawer<byte> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, byte value, Type valueType, Setter setter) {
            setter((byte) EditorGUI.IntField(rect, name, value));
        }
    }

    public class CharDrawer : TypeDrawer<char> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, char value, Type valueType, Setter setter) {
            setter((char) EditorGUI.IntField(rect, name, value));
        }
    }

    public class DecimalDrawer : TypeDrawer<decimal> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, decimal value, Type valueType, Setter setter) {
            setter((decimal) EditorGUI.DoubleField(rect, name, (double) value));
        }
    }

    public class DoubleDrawer : TypeDrawer<double> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, double value, Type valueType, Setter setter) {
            setter(EditorGUI.DoubleField(rect, name, value));
        }
    }

    public class Int16Drawer : TypeDrawer<short> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, short value, Type valueType, Setter setter) {
            setter((short) EditorGUI.IntField(rect, name, value));
        }
    }

    public class Int32Drawer : TypeDrawer<int> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, int value, Type valueType, Setter setter) {
            setter(EditorGUI.IntField(rect, name, value));
        }
    }

    public class Int64Drawer : TypeDrawer<long> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, long value, Type valueType, Setter setter) {
            setter(EditorGUI.LongField(rect, name, value));
        }
    }

    public class SByteDrawer : TypeDrawer<sbyte> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, sbyte value, Type valueType, Setter setter) {
            setter((sbyte) EditorGUI.IntField(rect, name, value));
        }
    }

    public class FloatDrawer : TypeDrawer<float> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, float value, Type valueType, Setter setter) {
            setter(EditorGUI.FloatField(rect, name, value));
        }
    }

    public class StringDrawer : TypeDrawer<string> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, string value, Type valueType, Setter setter) {
            setter(EditorGUI.TextField(rect, name, value));
        }
    }

    public class UInt16Drawer : TypeDrawer<ushort> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, ushort value, Type valueType, Setter setter) {
            setter((ushort) EditorGUI.IntField(rect, name, value));
        }
    }

    public class UInt32Drawer : TypeDrawer<uint> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, uint value, Type valueType, Setter setter) {
            setter((uint) EditorGUI.LongField(rect, name, value));
        }
    }

    public class UInt64Drawer : TypeDrawer<ulong> {
        public override void Draw(CutscenePlayer player, Cutscene cutscene, Rect rect, string name, ulong value, Type valueType, Setter setter) {
            setter((ulong) EditorGUI.LongField(rect, name, (long) value));
        }
    }
}
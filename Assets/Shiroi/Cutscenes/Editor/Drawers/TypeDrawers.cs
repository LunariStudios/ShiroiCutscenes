using System;
using System.Collections.Generic;
using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Drawers {
    [InitializeOnLoad]
    public static class TypeDrawers {
        private static List<TypeDrawer> knownDrawers = new List<TypeDrawer>();

        static TypeDrawers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterPrimitivesDrawers();
            RegisterUnityDrawers();
        }

        private static void RegisterUnityDrawers() {
            RegisterDrawer(new Vector2Drawer());
            RegisterDrawer(new Vector3Drawer());
            RegisterDrawer(new Vector4Drawer());
            RegisterDrawer(new Vector2IntDrawer());
            RegisterDrawer(new Vector3IntDrawer());
            RegisterDrawer(new BoundsDrawer());
            RegisterDrawer(new BoundsIntDrawer());
            RegisterDrawer(new ColorDrawer());
            RegisterDrawer(new AnimationCurveDrawer());
            RegisterDrawer(new LayerMaskDrawer());
        }

        private static void RegisterPrimitivesDrawers() {
            RegisterDrawer(new EnumDrawer());
            RegisterDrawer(new BooleanDrawer());
            RegisterDrawer(new ByteDrawer());
            RegisterDrawer(new CharDrawer());
            RegisterDrawer(new DecimalDrawer());
            RegisterDrawer(new DoubleDrawer());
            RegisterDrawer(new Int16Drawer());
            RegisterDrawer(new Int32Drawer());
            RegisterDrawer(new Int64Drawer());
            RegisterDrawer(new SByteDrawer());
            RegisterDrawer(new FloatDrawer());
            RegisterDrawer(new StringDrawer());
            RegisterDrawer(new UInt16Drawer());
            RegisterDrawer(new UInt32Drawer());
            RegisterDrawer(new UInt64Drawer());
        }

        public static void RegisterDrawer(TypeDrawer drawer) {
            knownDrawers.Add(drawer);
        }

        public static TypeDrawer GetDrawerFor(Type type) {
            var t = typeof(TypeDrawer<>).MakeGenericType(type);
            foreach (var knownDrawer in knownDrawers) {
                if (t.IsInstanceOfType(knownDrawer)) {
                    return knownDrawer;
                }
            }
            return null;
        }
    }
}
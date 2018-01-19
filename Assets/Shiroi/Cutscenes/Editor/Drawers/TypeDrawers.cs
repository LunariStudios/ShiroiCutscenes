using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    [InitializeOnLoad]
    public static class TypeDrawers {
        private static readonly List<TypeDrawer> KnownDrawers = new List<TypeDrawer>();
        private static readonly List<TypeDrawerProvider> KnownProviders = new List<TypeDrawerProvider>();

        static TypeDrawers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterDrawers();
            RegisterDrawerProviders();
        }

        private static void RegisterDrawerProviders() {
            RegisterDrawerProvider(new ExposedReferenceDrawerProvider());
        }

        private static void RegisterDrawers() {
            RegisterPrimitivesDrawers();
            RegisterUnityDrawers();
        }

        private static void RegisterUnityDrawers() {
            RegisterDrawer(new ObjectDrawer());
            RegisterDrawer(new QuaternionDrawer());
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
            KnownDrawers.Add(drawer);
        }

        public static void RegisterDrawerProvider(TypeDrawerProvider provider) {
            KnownProviders.Add(provider);
        }

        public static TypeDrawer GetDrawerFor(Type type) {
            foreach (var knownDrawer in KnownDrawers) {
                if (knownDrawer.Supports(type)) {
                    return knownDrawer;
                }
            }
            foreach (var provider in KnownProviders) {
                if (provider.Supports(type)) {
                    return CreateAndRegisterDrawer(type, provider);
                }
            }
            return null;
        }

        private static TypeDrawer CreateAndRegisterDrawer(Type type, TypeDrawerProvider provider) {
            var drawer = provider.Provide(type);
            RegisterDrawer(drawer);
            return drawer;
        }
    }
}
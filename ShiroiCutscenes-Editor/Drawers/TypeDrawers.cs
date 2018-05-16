using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    [InitializeOnLoad]
    public static class TypeDrawers {
        private static readonly List<TypeDrawer> KnownDrawers = new List<TypeDrawer>();
        private static readonly List<TypeDrawerProvider> KnownProviders = new List<TypeDrawerProvider>();
        private static readonly Dictionary<Type, TypeDrawer> AssignedDrawersCache = new Dictionary<Type, TypeDrawer>();
        static TypeDrawers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterDrawers();
            RegisterDrawerProviders();
        }

        private static void RegisterDrawerProviders() {
            RegisterDrawerProvider(new GenericDrawerProvider(typeof(ExposedReference<>), typeof(ExposedReferenceDrawer<>)));
            RegisterDrawerProvider(new GenericDrawerProvider(typeof(FutureReference<>), typeof(FutureDrawer<>)));
            RegisterDrawerProvider(new GenericDrawerProvider(typeof(Reference<>), typeof(ReferenceDrawer<>)));
        }

        private static void RegisterDrawers() {
            RegisterPrimitivesDrawers();
            RegisterUnityDrawers();
            RegisterShiroiDrawers();
        }

        private static void RegisterShiroiDrawers() {
            RegisterDrawer(new NotFoundDrawer());
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
            RegisterDrawer(new RectDrawer());
            RegisterDrawer(new RectIntDrawer());
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
            if (AssignedDrawersCache.ContainsKey(type)) {
                return AssignedDrawersCache[type];
            }
            var supportedDrawers = new List<TypeDrawer>();
            foreach (var knownDrawer in KnownDrawers) {
                if (knownDrawer.Supports(type)) {
                    supportedDrawers.Add(knownDrawer);
                }
            }
            foreach (var provider in KnownProviders) {
                if (provider.Supports(type)) {
                    supportedDrawers.Add(CreateAndRegisterDrawer(type, provider));
                }
            }
            if (supportedDrawers.Count == 0) {
                return null;
            }
            var selected = supportedDrawers.Max();
            AssignedDrawersCache[type] = selected;
            return selected;
        }

        private static TypeDrawer CreateAndRegisterDrawer(Type type, TypeDrawerProvider provider) {
            var drawer = provider.Provide(type);
            RegisterDrawer(drawer);
            return drawer;
        }
    }
}
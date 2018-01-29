using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Util;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Shiroi.Cutscenes.Serialization {
#if UNITY_EDITOR
    [InitializeOnLoad]
#else
[RuntimeInitializeOnLoadMethod]
#endif
    public static class Serializers {
        private static readonly List<Serializer> KnownSerializers = new List<Serializer>();

        private static readonly Dictionary<Type, Serializer> AssignedSerializersCache =
            new Dictionary<Type, Serializer>();

        private static readonly List<SerializerProvider> KnownProviders = new List<SerializerProvider>();

        static Serializers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterSerializers();
            RegisterProviders();
        }

        private static void RegisterProviders() {
            RegisterProvider(new GenericSerializerProvider(typeof(ExposedReference<>),
                typeof(ExposedReferenceSerializer<>)));
            RegisterProvider(new GenericSerializerProvider(typeof(FutureReference<>),
                typeof(FutureReferenceSerializer<>)));
            RegisterProvider(new GenericSerializerProvider(typeof(Reference<>), typeof(ReferenceSerializer<>)));
        }

        private static void RegisterProvider(SerializerProvider provider) {
            KnownProviders.Add(provider);
        }

        private static void RegisterSerializers() {
            RegisterPrimitiveSerializers();
            RegisterUnitySerializers();
        }

        private static void RegisterUnitySerializers() {
            //RegisterSerializer(new JsonSerializer()); Unity doesn't support this on callback receiver?
            RegisterSerializer(new ObjectSerializer());
            RegisterSerializer(new Vector2Serializer());
            RegisterSerializer(new Vector3Serializer());
            RegisterSerializer(new Vector4Serializer());
            RegisterSerializer(new Vector2IntSerializer());
            RegisterSerializer(new Vector3IntSerializer());
            RegisterSerializer(new QuaternionSerializer());
            RegisterSerializer(new ColorSerializer());
            RegisterSerializer(new AnimationCurveSerializer());
            RegisterSerializer(new RectSerializer());
            RegisterSerializer(new RectIntSerializer());
            RegisterSerializer(new BoundsSerializer());
            RegisterSerializer(new BoundsIntSerializer());
            RegisterSerializer(new LayerMaskSerializer());
        }

        public static void RegisterPrimitiveSerializers() {
            //Main Primitives
            RegisterSerializer(new IntSerializer());
            RegisterSerializer(new FloatSerializer());
            RegisterSerializer(new BooleanSerializer());
            RegisterSerializer(new StringSerializer());

            //Other primitives
            RegisterSerializer(new EnumSerializer());
            RegisterSerializer(new ByteSerializer());
            RegisterSerializer(new SignedByteSerializer());
            RegisterSerializer(new ShortSerializer());
            RegisterSerializer(new UnsignedShortSerializer());
            RegisterSerializer(new UnsignedIntSerializer());
            RegisterSerializer(new LongSerializer());
            RegisterSerializer(new UnsignedLongSerializer());
            RegisterSerializer(new DoubleSerializer());
        }

        public static void RegisterSerializer(Serializer serializer) {
            KnownSerializers.Add(serializer);
        }

        public static Serializer For(Type type) {
            if (AssignedSerializersCache.ContainsKey(type)) {
                return AssignedSerializersCache[type];
            }
            var supportedSerializers = new List<Serializer>();
            foreach (var knownSerializer in KnownSerializers) {
                if (knownSerializer.Supports(type)) {
                    supportedSerializers.Add(knownSerializer);
                }
            }
            foreach (var provider in KnownProviders) {
                if (provider.Supports(type)) {
                    supportedSerializers.Add(CreateAndRegisterSerializer(type, provider));
                }
            }
            if (supportedSerializers.Count == 0) {
                return null;
            }
            var selected = supportedSerializers.Max();
            AssignedSerializersCache[type] = selected;
            return selected;
        }

        private static Serializer CreateAndRegisterSerializer(Type type, SerializerProvider provider) {
            var drawer = provider.Provide(type);
            RegisterSerializer(drawer);
            return drawer;
        }
    }
}
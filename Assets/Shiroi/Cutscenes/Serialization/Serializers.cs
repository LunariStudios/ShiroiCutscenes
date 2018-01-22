using System.Collections.Generic;
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
        private static readonly List<Serializer> KnownDrawers = new List<Serializer>();

        static Serializers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterPrimitiveSerializers();
        }

        public static void RegisterPrimitiveSerializers() {
            //Main Primitives
            RegisterSerializer(new IntSerializer());
            RegisterSerializer(new FloatSerializer());
            RegisterSerializer(new BooleanSerializer());
            RegisterSerializer(new StringSerializer());

            //Other primitives
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
            KnownDrawers.Add(serializer);
        }
    }
}
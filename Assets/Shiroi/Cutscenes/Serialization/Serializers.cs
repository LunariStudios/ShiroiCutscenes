using System.Collections.Generic;

namespace Shiroi.Cutscenes.Serialization {
    public class Serializers {
        private static readonly List<Serializer> KnownDrawers = new List<Serializer>();

        public static void RegisterPrimitiveSerializers() {
            RegisterSerializer(new ByteSerializer());
            RegisterSerializer(new SignedByteSerializer());
            RegisterSerializer(new CharSerializer());
            RegisterSerializer(new ShortSerializer());
            RegisterSerializer(new UnsignedShortSerializer());
            RegisterSerializer(new IntSerializer());
            RegisterSerializer(new UnsignedIntSerializer());
            RegisterSerializer(new LongSerializer());
            RegisterSerializer(new UnsignedLongSerializer());
            RegisterSerializer(new FloatSerializer());
            RegisterSerializer(new DoubleSerializer());
            RegisterSerializer(new StringSerializer());
        }

        public static void RegisterSerializer(Serializer serializer) {
            KnownDrawers.Add(serializer);
        }
    }
}
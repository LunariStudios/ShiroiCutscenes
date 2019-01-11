using System;

namespace Shiroi.Cutscenes.Communication {
    [Serializable]
    public sealed class BoolInput : Input<bool> { }

    [Serializable]
    public sealed class StringInput : Input<string> { }

    [Serializable]
    public sealed class Int8Input : Input<sbyte> { }

    [Serializable]
    public sealed class Int16Input : Input<short> { }

    [Serializable]
    public sealed class Int32Input : Input<int> { }

    [Serializable]
    public sealed class Int64Input : Input<long> { }

    [Serializable]
    public sealed class UInt8Input : Input<byte> { }

    [Serializable]
    public sealed class UInt16Input : Input<ushort> { }

    [Serializable]
    public sealed class UInt32Input : Input<uint> { }

    [Serializable]
    public sealed class UInt64Input : Input<ulong> { }

    [Serializable]
    public sealed class FloatInput : Input<float> { }

    [Serializable]
    public sealed class DoubleInput : Input<double> { }

    [Serializable]
    public sealed class BoolOutput : Output<bool> { }

    [Serializable]
    public sealed class StringOutput : Output<string> { }

    [Serializable]
    public sealed class Int8Output : Output<sbyte> { }

    [Serializable]
    public sealed class Int16Output : Output<short> { }

    [Serializable]
    public sealed class Int32Output : Output<int> { }

    [Serializable]
    public sealed class Int64Output : Output<long> { }

    [Serializable]
    public sealed class UInt8Output : Output<byte> { }

    [Serializable]
    public sealed class UInt16Output : Output<ushort> { }

    [Serializable]
    public sealed class UInt32Output : Output<uint> { }

    [Serializable]
    public sealed class UInt64Output : Output<ulong> { }

    [Serializable]
    public sealed class FloatOutput : Output<float> { }

    [Serializable]
    public sealed class DoubleOutput : Output<double> { }
}
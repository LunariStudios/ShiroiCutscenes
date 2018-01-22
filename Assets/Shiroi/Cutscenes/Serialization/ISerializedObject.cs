namespace Shiroi.Cutscenes.Serialization {
    public interface ISerializedObject {
        void SetByte(string key, byte value);

        void SetSignedByte(string key, sbyte value);

        void SetChar(string key, char value);

        void SetShort(string key, short value);

        void SetUnsignedShort(string key, ushort value);

        void SetInt(string key, int value);

        void SetUnsignedInt(string key, uint value);

        void SetLong(string key, long value);

        void SetUnsignedLong(string key, ulong value);

        void SetFloat(string key, float value);

        void SetDouble(string key, float value);

        void SetString(string key, string value);

        byte GetByte(string key);

        sbyte GetSignedByte(string key);

        char GetChar(string key);

        short GetShort(string key);

        ushort GetUnsignedShort(string key);

        int GetInt(string key);

        uint GetUnsignedInt(string key);

        long GetLong(string key);

        ulong GetUnsignedLong(string key);

        float GetFloat(string key);

        float GetDouble(string key);

        string GetString(string key);
    }
}
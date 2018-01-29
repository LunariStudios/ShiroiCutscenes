namespace Shiroi.Cutscenes.Util {
    public struct Reference<T> {
        public enum ReferenceType : byte{
            Future = 0,
            Exposed = 1
        }

        public ReferenceType Type;
        public int Id;
    }
}
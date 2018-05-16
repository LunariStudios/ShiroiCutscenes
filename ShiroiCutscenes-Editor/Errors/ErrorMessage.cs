namespace Shiroi.Cutscenes.Editor.Errors {
    public enum ErrorLevel : byte {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public struct ErrorMessage {
        public ErrorMessage(int tokenIndex, int fieldIndex, ErrorLevel level, string[] lines) : this() {
            TokenIndex = tokenIndex;
            FieldIndex = fieldIndex;
            Level = level;
            Lines = lines;
        }

        public int TokenIndex { get; private set; }
        public int FieldIndex { get; private set; }
        public ErrorLevel Level { get; private set; }
        public string[] Lines { get; private set; }
    }
}
namespace Shiroi.Cutscenes.Serialization {
    public abstract class Pair<T> {
        protected Pair() { }

        protected Pair(string key, T value) {
            Key = key;
            Value = value;
        }

        public string Key;
        public T Value;
    }
}
using System;

namespace Shiroi.Cutscenes.Editor.Util {
    public class Lazy<T> {
        private T value;
        private readonly Func<T> creator;

        public Lazy(Func<T> creator) {
            this.creator = creator;
        }

        public T Value {
            get {
                if (value == null) {
                    value = creator();
                }

                return value;
            }
        }
    }
}
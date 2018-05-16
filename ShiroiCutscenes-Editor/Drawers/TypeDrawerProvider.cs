using System;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public abstract class TypeDrawerProvider {
        public abstract bool Supports(Type type);
        public abstract TypeDrawer Provide(Type type);
    }
}
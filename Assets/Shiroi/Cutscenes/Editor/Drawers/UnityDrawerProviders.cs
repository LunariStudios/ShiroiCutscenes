using System;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class ExposedReferenceDrawerProvider : TypeDrawerProvider {
        private static readonly Type ExposedReferenceType = typeof(ExposedReferenceDrawer<>);

        static bool IsInstanceOfGenericType(Type genericType, Type type) {
            while (type != null) {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType) {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public override bool Supports(Type type) {
            if (!type.IsGenericType) {
                return false;
            }
            return IsInstanceOfGenericType(typeof(ExposedReference<>), type);
        }

        public override TypeDrawer Provide(Type type) {
            //Type in this case is ExposedReference<T>
            var genericType = type.GetGenericArguments()[0];

            return (TypeDrawer) Activator.CreateInstance(ExposedReferenceType.MakeGenericType(genericType));
        }
    }
}
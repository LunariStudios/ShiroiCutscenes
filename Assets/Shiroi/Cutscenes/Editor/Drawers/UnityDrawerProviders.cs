using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class ExposedReferenceDrawerProvider : TypeDrawerProvider {
        private static readonly Type ExposedReferenceType = typeof(ExposedReferenceDrawer<>);

        public override bool Supports(Type type) {
            return type.IsGenericType && TypeUtil.IsInstanceOfGenericType(typeof(ExposedReference<>), type);
        }

        public override TypeDrawer Provide(Type type) {
            //Type in this case is ExposedReference<T>
            var genericType = type.GetGenericArguments()[0];

            return (TypeDrawer) Activator.CreateInstance(ExposedReferenceType.MakeGenericType(genericType));
        }
    }
}
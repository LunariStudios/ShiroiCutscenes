using System;
using Shiroi.Cutscenes.Editor.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Drawers {
    public class GenericDrawerProvider : TypeDrawerProvider {
        private readonly Type supportedType;
        private readonly Type drawerType;
        public GenericDrawerProvider(Type supportedType, Type drawerType) {
            this.supportedType = supportedType;
            this.drawerType = drawerType;
        }

        public override bool Supports(Type type) {
            return type.IsGenericType && TypeUtil.IsInstanceOfGenericType(supportedType, type);
        }

        public override TypeDrawer Provide(Type type) {
            //Type in this case is ExposedReference<T>
            var genericType = type.GetGenericArguments()[0];

            return (TypeDrawer) Activator.CreateInstance(drawerType.MakeGenericType(genericType));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Editor.Errors {
    public class MissingExposedReferenceChecker : ErrorChecker {
        private static readonly Type ExposedReferenceType = typeof(ExposedReference<>);
        public const string PropertyNameFieldName = "exposedName";
        public const string DefaultValueFieldName = "defaultValue";
        private static Dictionary<Type, FieldInfo> propertyNameCache = new Dictionary<Type, FieldInfo>();
        private static Dictionary<Type, FieldInfo> defaultNameCache = new Dictionary<Type, FieldInfo>();

        public static FieldInfo GetInfo(Dictionary<Type, FieldInfo> dict, Type generic, string fieldName) {
            if (dict.ContainsKey(generic)) {
                return dict[generic];
            }
            var info = ExposedReferenceType.MakeGenericType(generic).GetField(fieldName);
            return dict[generic] = info;
        }

        public static FieldInfo GetPropertyNameInfo(Type generic) {
            return GetInfo(propertyNameCache, generic, PropertyNameFieldName);
        }

        public static FieldInfo GetDefaultValueInfo(Type generic) {
            return GetInfo(defaultNameCache, generic, DefaultValueFieldName);
        }

        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex,
            FieldInfo info) {
            if (Attribute.GetCustomAttribute(info, typeof(NullSupportedAttribute)) != null) {
                return;
            }
            var fieldType = info.FieldType;
            if (!fieldType.IsGenericType || fieldType.GetGenericTypeDefinition() != ExposedReferenceType) {
                return;
            }
            var generic = fieldType.GetGenericArguments()[0];
            var propertyNameInfo = GetPropertyNameInfo(generic);
            var defaultValueInfo = GetDefaultValueInfo(generic);
            var propertyName = (PropertyName) propertyNameInfo.GetValue(value);
            var defaultObject = defaultValueInfo.GetValue(value) as Object;
            if (MimicExposedResolve(editor.Player, propertyName, defaultObject) != null) {
                return;
            }
            var msg = string.Format("Couldn't resolve exposed reference of id {0} in field {1}.", propertyName,
                info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, ShiroiStyles.NullSupportedMessage);
        }

        private static Object MimicExposedResolve(CutscenePlayer resolver, PropertyName exposedName,
            Object defaultValue) {
            if (resolver == null) {
                return defaultValue;
            }
            bool idValid;
            var referenceValue = resolver.GetReferenceValue(exposedName, out idValid);
            return idValid ? referenceValue : defaultValue;
        }
    }

    public class MissingFutureChecker : ErrorChecker {
        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info) {
            if (Attribute.GetCustomAttribute(info, typeof(NullSupportedAttribute)) != null) {
                return;
            }
            var reference = value as FutureReference;
            if (reference == null) {
                return;
            }
            var id = reference.Id;
            if (editor.Cutscene.FutureManager.GetFuture(id) != null) {
                return;
            }
            var msg = string.Format("Couldn't find future of id {0} in field {1}.", id, info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, ShiroiStyles.NullSupportedMessage);
        }
    }


    public class MissingReferenceChecker : ErrorChecker {
        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info) {
            var reference = value as Reference;
            var player = editor.Player;
            if (reference == null || player == null) {
                return;
            }
            var id = reference.Id;
            if (Resolve(reference, editor.Cutscene, player)) {
                return;
            }
            var msg = string.Format("Couldn't resolve reference of id {0} in field {1}.", id, info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, ShiroiStyles.NullSupportedMessage);
        }

        private bool Resolve(Reference reference, Cutscene cutscene, CutscenePlayer player) {
            switch (reference.Type) {
                case Reference.ReferenceType.Future:
                    return cutscene.FutureManager.GetFuture(reference.Id) != null;
                case Reference.ReferenceType.Exposed:
                    return reference.Resolve(player) != null;
                default:
                    return false;
            }
        }
    }

    public class NullChecker : ErrorChecker {
        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info) {
            if (Attribute.GetCustomAttribute(info, typeof(NullSupportedAttribute)) != null) {
                return;
            }
            if (value != null) {
                return;
            }
            var msg = string.Format("Field {0} is null!", info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, ShiroiStyles.NullSupportedMessage);
        }
    }

    public class EmptyStringChecker : ErrorChecker {
        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info) {
            if (Attribute.GetCustomAttribute(info, typeof(EmptyStringSupportedAttribute)) != null) {
                return;
            }
            var s = value as string;
            if (s == null) {
                return;
            }
            if (!string.IsNullOrEmpty(s)) {
                return;
            }
            var msg = string.Format("Field {0} has an empty string.", info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.Medium, msg,
                "Please assign it or annotate the field as EmptyStringSupported.");
        }
    }

    public class UnusedFutureChecker : ErrorChecker, IOnBeginCheckListener, IOnEndCheckListener {
        private readonly Dictionary<int, int> uses = new Dictionary<int, int>();

        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex,
            FieldInfo info) {
            if (!(value is Reference) && !(value is FutureReference)) {
                return;
            }
            Increment(ReferenceUtility.GetID(value));
        }

        private void Increment(int futureId) {
            if (uses.ContainsKey(futureId)) {
                uses[futureId]++;
            } else {
                uses[futureId] = 1;
            }
        }

        private int GetUses(int futureId) {
            return uses.ContainsKey(futureId) ? uses[futureId] : 0;
        }


        public void OnEnd(ErrorManager obj, CutsceneEditor editor) {
            var cutscene = editor.Cutscene;
            foreach (var future in cutscene.FutureManager.Futures) {
                var pIndex = future.Provider;
                var provider = cutscene[pIndex];
                var id = future.Id;
                if (GetUses(id) > 0) {
                    continue;
                }
                var msg = string.Format("Future {0} ({1}) is never used!", future.Name, id);
                obj.NotifyError(pIndex, -1, ErrorLevel.Medium, msg);
            }
        }

        public void OnBegin(ErrorManager manager, CutsceneEditor editor) {
            uses.Clear();
        }
    }
}
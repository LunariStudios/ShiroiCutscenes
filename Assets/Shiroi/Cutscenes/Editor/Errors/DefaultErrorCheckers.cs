using System;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Shiroi.Cutscenes.Editor.Errors {
    public class MissingFutureChecker : ErrorChecker {
        public override void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info) {
            var reference = value as FutureReference;
            if (reference == null) {
                return;
            }
            var id = reference.Id;
            if (editor.Cutscene.FutureManager.GetFuture(id) != null) {
                return;
            }
            var msg = string.Format("Couldn't find future of id {0} in field {1}.", id, info.Name);
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, "You probably didn't assign it.");
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
            manager.NotifyError(tokenIndex, fieldIndex, ErrorLevel.High, msg, "You probably didn't assign it.");
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
            manager.NotifyError(tokenIndex, fieldIndex,
                ErrorLevel.High,
                msg, "Please assign it or annotate the field as NullSupported.");
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
}
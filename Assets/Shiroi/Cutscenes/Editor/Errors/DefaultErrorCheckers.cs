using System;
using System.Reflection;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Shiroi.Cutscenes.Editor.Errors {
    public class MissingFutureChecker : ErrorChecker {
        public override void Check(CutsceneEditor check, int tokenIndex, IToken token, object value, FieldInfo info) {
            var reference = value as FutureReference;
            if (reference == null) {
                return;
            }
            var id = reference.Id;
            if (check.Cutscene.GetFuture(id) != null) {
                return;
            }
            var msg = string.Format("Couldn't find future of id {0} in field {1}.", id, info.Name);
            check.NotifyError(tokenIndex, ErrorLevel.High, msg, "You probably didn't assign it.");
        }
    }


    public class MissingReferenceChecker : ErrorChecker {
        public override void Check(CutsceneEditor check, int tokenIndex, IToken token, object value, FieldInfo info) {
            var reference = value as Reference;
            var player = check.Player;
            if (reference == null || player == null) {
                return;
            }
            var id = reference.Id;
            if (reference.Resolve(player) != null) {
                return;
            }
            var msg = string.Format("Couldn't resolve reference of id {0} in field {1}.", id, info.Name);
            check.NotifyError(tokenIndex, ErrorLevel.High, msg, "You probably didn't assign it.");
        }
    }

    public class NullChecker : ErrorChecker {
        public override void Check(CutsceneEditor check, int tokenIndex, IToken token, object value, FieldInfo info) {
            if (Attribute.GetCustomAttribute(info, typeof(NullSupportedAttribute)) != null) {
                return;
            }
            if (value != null) {
                return;
            }
            var msg = string.Format("Field {0} is null!", info.Name);
            check.NotifyError(tokenIndex,
                ErrorLevel.High,
                msg, "Please assign it or annotate the field as NullSupported.");
        }
    }
}
using System.Reflection;
using Shiroi.Cutscenes.Tokens;

namespace Shiroi.Cutscenes.Editor.Errors {
    public abstract class ErrorChecker {
        public abstract void Check(CutsceneEditor editor, ErrorManager manager, int tokenIndex, IToken token,
            object value, int fieldIndex, FieldInfo info);
    }

    public interface IOnBeginCheckListener {
        void OnBegin(ErrorManager manager, CutsceneEditor editor);
    }
    
    public interface IOnEndCheckListener {
        void OnEnd(ErrorManager manager, CutsceneEditor editor);
    }
}
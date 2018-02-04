using System.Collections.Generic;
using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Errors {
    [InitializeOnLoad]
    public static class ErrorCheckers {
        public static readonly List<ErrorChecker> Checkers = new List<ErrorChecker>();

        static ErrorCheckers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterChecker(new NullChecker());
            RegisterChecker(new MissingFutureChecker());
            RegisterChecker(new MissingExposedReferenceChecker());
            RegisterChecker(new MissingReferenceChecker());
            RegisterChecker(new EmptyStringChecker());
            RegisterChecker(new UnusedFutureChecker());
        }

        private static void RegisterChecker(ErrorChecker nullChecker) {
            Checkers.Add(nullChecker);
        }

        public static void CheckErrors(CutsceneEditor editor, List<ErrorMessage> errors) {
            var cutscene = editor.Cutscene;
            var total = cutscene.TotalTokens;
            for (var i = 0; i < total; i++) {
                var token = cutscene[i];
                var mapped = MappedToken.For(token);
                foreach (var checker in Checkers) {
                    for (var fieldIndex = 0; fieldIndex < mapped.SerializedFields.Length; fieldIndex++) {
                        var serializedField = mapped.SerializedFields[fieldIndex];
                        var value = serializedField.GetValue(token);
                        checker.Check(editor, editor.ErrorManager, i, token, value, fieldIndex, serializedField);
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEditor;

namespace Shiroi.Cutscenes.Editor.Errors {
    [InitializeOnLoad]
    public static class ErrorCheckers {
        private static readonly List<ErrorChecker> Checkers = new List<ErrorChecker>();

        static ErrorCheckers() {
            RegisterBuiltIn();
        }

        private static void RegisterBuiltIn() {
            RegisterChecker(new NullChecker());
            RegisterChecker(new MissingFutureChecker());
            RegisterChecker(new MissingReferenceChecker());
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
                    foreach (var serializedField in mapped.SerializedFields) {
                        var value = serializedField.GetValue(token);
                        checker.Check(editor, i, token, value, serializedField);
                    }
                }
            }
        }
    }
}
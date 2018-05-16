using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Errors {
    public class ErrorManager {
        private readonly List<ErrorMessage> errors = new List<ErrorMessage>();

        public bool ShowErrors {
            get;
            private set;
        }

        public void Clear() {
            errors.Clear();
        }

        public ErrorManager() {
            MappedToken.OnFieldDrawn += OnFieldDrawn;
            MappedToken.OnTokenDrawn += OnTokenDrawn;
        }

        private void OnTokenDrawn(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token, int tokenIndex,
            ref GUIContent tokenlabel) {
            if (!Configs.CheckErrors) {
                return;
            }
            var foundErrors = GetErrors(tokenIndex, -1).ToArray();
            if (!foundErrors.Any()) {
                return;
            }
            var highestLevel = (from error in foundErrors select error.Level).Max();
            var icon = ShiroiStyles.GetIcon(highestLevel);
            tokenlabel.image = icon;
        }

        private void OnFieldDrawn(Rect rect, Cutscene cutscene, CutscenePlayer player, IToken token, int tokenIndex,
            FieldInfo field, int fieldIndex, ref GUIContent label) {
            if (!Configs.CheckErrors) {
                return;
            }
            var foundErrors = GetErrors(tokenIndex, fieldIndex).ToArray();
            if (!foundErrors.Any()) {
                return;
            }
            var highestLevel = (from error in foundErrors select error.Level).Max();
            var size = rect.height;
            var icon = ShiroiStyles.GetIcon(highestLevel);
            var errorRect = rect.SubRect(size, size, -size);
            GUI.DrawTexture(errorRect, icon);
        }

        public void NotifyError(int tokenIndex, int fieldIndex, ErrorLevel level, params string[] message) {
            errors.Add(new ErrorMessage(tokenIndex, fieldIndex, level, message));
        }

        public IEnumerable<ErrorMessage> GetErrors(int tokenIndex, int index) {
            return from message in errors
                where message.FieldIndex == index && message.TokenIndex == tokenIndex
                select message;
        }

        public void DrawErrors(CutsceneEditor editor) {
            InvokeOnBeginErrorChecking(editor);
            ErrorCheckers.CheckErrors(editor, errors);
            InvokeOnEndErrorChecking(editor);
            var cutscene = editor.Cutscene;
            var totalErrors = errors.Count;
            if (totalErrors <= 0) {
                return;
            }
            var max = (from message in errors select message.Level).Max();
            ShowErrors = GUILayout.Toggle(ShowErrors, GetErrorContent(totalErrors, max),
                ShiroiStyles.GetErrorStyle(max), GUILayout.MinHeight(ShiroiStyles.SingleLineHeight * 2));
            if (!ShowErrors) {
                GUILayout.Space(ShiroiStyles.SpaceHeight);
                return;
            }
            var init = GUI.backgroundColor;
            foreach (var errorMessage in errors) {
                var lines = errorMessage.Lines;
                var height = (lines.Length + 1) * ShiroiStyles.SingleLineHeight;
                var rect = GUILayoutUtility.GetRect(10, height, ShiroiStyles.ExpandWidthOption);
                Rect iconRect;
                Rect messagesRect;
                rect.Split(ShiroiStyles.IconSize, out iconRect, out messagesRect);

                GUI.backgroundColor = ShiroiStyles.GetColor(errorMessage.Level);
                GUI.Box(rect, GUIContent.none);
                GUI.Box(iconRect, ShiroiStyles.GetContent(errorMessage.Level));
                var index = errorMessage.TokenIndex;
                var token = cutscene[index];
                var label = string.Format("Token #{0} ({1})", index, token.GetType().Name);
                GUI.Label(messagesRect.GetLine(0), label, ShiroiStyles.Bold);
                for (uint i = 0; i < lines.Length; i++) {
                    var pos = messagesRect.GetLine(i + 1);
                    GUI.Label(pos, lines[i]);
                }
            }
            GUILayout.Space(ShiroiStyles.SpaceHeight);
            GUI.backgroundColor = init;
        }


        private GUIContent GetErrorContent(int totalErrors, ErrorLevel maxLevel) {
            var msg = ShowErrors ? "Hide Errors" : "Show Errors";
            if (totalErrors > 0) {
                msg += string.Format(" ({0})", totalErrors);
            }
            return new GUIContent(msg, ShiroiStyles.GetIcon(maxLevel));
        }

        private void InvokeOnBeginErrorChecking(CutsceneEditor editor) {
            foreach (var checker in ErrorCheckers.Checkers) {
                var listener = checker as IOnBeginCheckListener;
                if (listener != null) {
                    listener.OnBegin(this, editor);
                }
            }
        }

        protected virtual void InvokeOnEndErrorChecking(CutsceneEditor editor) {
            foreach (var checker in ErrorCheckers.Checkers) {
                var listener = checker as IOnEndCheckListener;
                if (listener != null) {
                    listener.OnEnd(this, editor);
                }
            }
        }
    }
}
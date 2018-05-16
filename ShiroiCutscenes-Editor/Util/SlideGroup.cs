using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public class SlideGroup {
        private Dictionary<int, Rect> animIDs = new Dictionary<int, Rect>();


        public void Reset() {
            animIDs.Clear();
        }


        public Rect GetRect(UnityEditor.Editor editor, int id, Rect current) {
            if (Event.current.type != EventType.Repaint)
                return current;
            bool changed;
            return GetRect(editor, id, current, out changed);
        }

        private Rect GetRect(UnityEditor.Editor editor, int id, Rect current, out bool changed) {
            if (!animIDs.ContainsKey(id)) {
                animIDs.Add(id, current);
                changed = false;
                return current;
            }
            var animId = animIDs[id];
            if (animId != current) {
                const float t = 0.1f;
                if (Mathf.Abs(animId.y - current.y) > 0.5) {
                    current.y = Mathf.Lerp(animId.y, current.y, t);
                }
                if (Mathf.Abs(animId.height - current.height) > 0.5) {
                    current.height = Mathf.Lerp(animId.height, current.height, t);
                }
                if (Mathf.Abs(animId.x - current.x) > 0.5) {
                    current.x = Mathf.Lerp(animId.x, current.x, t);
                }
                if (Mathf.Abs(animId.width - current.width) > 0.5) {
                    current.width = Mathf.Lerp(animId.width, current.width, t);
                }
                animIDs[id] = current;
                changed = true;
                editor.Repaint();
            } else {
                changed = false;
            }
            return current;
        }
    }
}
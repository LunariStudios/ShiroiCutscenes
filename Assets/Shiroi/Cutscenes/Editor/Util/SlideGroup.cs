using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public class SlideGroup {
        private static SlideGroup current = (SlideGroup) null;
        private Dictionary<int, Rect> animIDs = new Dictionary<int, Rect>();


        public void Reset() {
            current = null;
            animIDs.Clear();
        }


        public Rect GetRect(int id, Rect r) {
            if (Event.current.type != EventType.Repaint)
                return r;
            bool changed;
            return GetRect(id, r, out changed);
        }

        private Rect GetRect(int id, Rect r, out bool changed) {
            if (!animIDs.ContainsKey(id)) {
                animIDs.Add(id, r);
                changed = false;
                return r;
            }
            var animId = animIDs[id];
            if ((double) animId.y != (double) r.y || (double) animId.height != (double) r.height ||
                ((double) animId.x != (double) r.x || (double) animId.width != (double) r.width)) {
                var t = 0.1f;
                if ((double) Mathf.Abs(animId.y - r.y) > 0.5)
                    r.y = Mathf.Lerp(animId.y, r.y, t);
                if ((double) Mathf.Abs(animId.height - r.height) > 0.5)
                    r.height = Mathf.Lerp(animId.height, r.height, t);
                if ((double) Mathf.Abs(animId.x - r.x) > 0.5)
                    r.x = Mathf.Lerp(animId.x, r.x, t);
                if ((double) Mathf.Abs(animId.width - r.width) > 0.5)
                    r.width = Mathf.Lerp(animId.width, r.width, t);
                animIDs[id] = r;
                changed = true;
                HandleUtility.Repaint();
            } else
                changed = false;
            return r;
        }
    }
}
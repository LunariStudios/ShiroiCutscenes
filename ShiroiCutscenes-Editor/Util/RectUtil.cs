using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public static class RectUtil {
        public static Rect GetLine(this Rect rect, uint collum, uint totalLines = 1,
            float collumHeight = ShiroiStyles.SingleLineHeight,
            float yOffset = 0F) {
            var height = totalLines * collumHeight;
            return rect.GetLine(collum, height, collumHeight, yOffset);
        }

        private static Rect GetLine(this Rect rect, uint collum, float height,
            float collumHeight = ShiroiStyles.SingleLineHeight,
            float yOffset = 0F) {
            return rect.SubRect(rect.width, height, yOffset: collum * collumHeight + yOffset);
        }

        public static Rect SubRect(this Rect rect, float width, float height, float xOffset = 0F, float yOffset = 0F) {
            return new Rect(rect.x + xOffset, rect.y + yOffset, width, height);
        }

        public static void Split(this Rect rect, float splitPosition, out Rect a, out Rect b) {
            a = rect.SubRect(splitPosition, rect.height);
            b = rect.SubRect(rect.width - splitPosition, rect.height, splitPosition);
        }
    }
}
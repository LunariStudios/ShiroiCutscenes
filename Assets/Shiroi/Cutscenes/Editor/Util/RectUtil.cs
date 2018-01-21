using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Util {
    public static class RectUtil {
        public const float SingleLineHeight = 16;


        public static Rect GetLine(this Rect rect, uint collum, uint totalLines = 1,
            float collumHeight = SingleLineHeight) {
            var height = totalLines * collumHeight;
            return rect.GetLine(collum, height, collumHeight);
        }

        public static Rect GetLine(this Rect rect, uint collum, float height, float collumHeight = SingleLineHeight) {
            return rect.SubRect(rect.width, height, yOffset: collum * collumHeight);
        }

        public static Rect SubRect(this Rect rect, float width, float height, float xOffset = 0F, float yOffset = 0F) {
            return new Rect(rect.x + xOffset, rect.y + yOffset, width, height);
        }
    }
}
using System.Linq;
using Shiroi.Cutscenes.Editor.Config;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Windows {
    public class ConfigWindow : EditorWindow {
        public const float Margin = 20;

        private void OnEnable() {
            titleContent = new GUIContent("SC-Config");
            var style = GUIStyle.none;
            var maxWidth = (from config in Configs.AllConfigs
                    select style.CalcSize(new GUIContent(config.Description)).x)
                .Max() + Margin;
            var pos = position;
            pos.width = maxWidth;
            position = pos;
        }

        [MenuItem("Tools/ShiroiCutscenes/Configs")]
        public static void ShowWindow() {
            GetWindow(typeof(ConfigWindow)).Show();
        }

        private void OnGUI() {
            foreach (var config in Configs.AllConfigs) {
                config.DrawGUI();
            }
        }
    }
}
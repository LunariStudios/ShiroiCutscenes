using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor {
    public class IconHandler : AssetPostprocessor {
        [InitializeOnLoadMethod]
        private static void OnLoaded() {
            foreach (var effectAssets in AssetDatabase.FindAssets("t:Shiroi.Cutscenes.Cutscene")) {
                foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(effectAssets))) {
                    ProcessObject(asset);
                }
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths) {
            foreach (var importedAsset in importedAssets) {
                foreach (var o in AssetDatabase.LoadAllAssetsAtPath(importedAsset)) {
                    ProcessObject(o);
                }
            }
        }


        private static void ProcessObject(Object o) {
            if (o is Cutscene) {
                o.SetIcon("Shiroi.Cutscenes.Editor.Icons.Cutscene Icon.png");
            }
        }
    }
}
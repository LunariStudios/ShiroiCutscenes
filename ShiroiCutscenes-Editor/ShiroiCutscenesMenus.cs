using System.IO;
using UnityEngine;
using UnityEditor;

namespace Shiroi.Cutscenes.Editor {
    public class ShiroiCutscenesMenus {
        public const string CreateCutsceneMenuPath = "Shiroi/Cutscenes/Cutscene";

        [MenuItem("Assets/Create/Shiroi/Cutscenes/Cutscene")]
        public static void CreateAsset() {
            var asset = ScriptableObject.CreateInstance<Cutscene>();

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New Cutscene.asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
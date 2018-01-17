using System.Collections;
using UnityEngine;

namespace Shiroi.Cutscenes {
    public class CutscenePlayer : MonoBehaviour, IExposedPropertyTable {
        private static CutscenePlayer instance;

        public static CutscenePlayer Instance {
            get { return instance ?? (instance = FindObjectOfType<CutscenePlayer>()); }
        }

        private void Awake() {
            if (!instance) {
                return;
            }
            Debug.LogWarning(
                "[ShiroiCutscenes] There is already an instance of CutscenePlayer loaded! Destroying newly created instance...");
            Destroy(gameObject);
        }

        public void Play(Cutscene cutscene) {
            StartCoroutine(YieldPlay(cutscene));
        }

        public IEnumerator YieldPlay(Cutscene cutscene) {
            foreach (var token in cutscene.Tokens) {
                yield return token.Execute(this);
            }
        }

        public void SetReferenceValue(PropertyName id, Object value) { }

        public Object GetReferenceValue(PropertyName id, out bool idValid) {
            //TODO: implement
            idValid = false;
            return null;
        }

        public void ClearReferenceValue(PropertyName id) { }
    }
}
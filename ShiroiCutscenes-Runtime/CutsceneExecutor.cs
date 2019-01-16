using System;
using System.Collections;
using Lunari.Tsuki;
using Shiroi.Cutscenes.Communication;
using UnityEngine;

namespace Shiroi.Cutscenes {
    public class CutsceneExecutor : IDisposable {
        public CutsceneExecutor(Cutscene cutscene, MonoBehaviour host) {
            Cutscene = cutscene;
            Host = host;
        }

        public MonoBehaviour Host {
            get;
        }

        public int CurrentToken {
            get;
            set;
        }

        public Context Context {
            get;
        } = new Context();

        public Coroutine Coroutine {
            get;
            private set;
        }

        public Cutscene Cutscene {
            get;
        }

        public Coroutine Play() {
            return Coroutine = Host.StartCoroutine(Execute());
        }

        private IEnumerator Execute() {
            for (; CurrentToken < Cutscene.Tokens.Count; CurrentToken++) {
                yield return Cutscene.Tokens[CurrentToken].Execute(this);
            }
        }


        public void Dispose() {
            Coroutine.StopIfNotNull(Host);
        }
    }
}
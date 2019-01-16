using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    [Category(ShiroiCutscenesConstants.CommonCategory)]
    public class WaitForSecondsToken : Token {
        public float Duration;
        public bool Realtime;
        private readonly Dictionary<CutsceneExecutor, Waiter> cache = new Dictionary<CutsceneExecutor, Waiter>();

        public override IEnumerator Execute(CutsceneExecutor executor) {
            var w = new Waiter(Duration, Realtime);
            cache[executor] = w;
            while (w.Tick()) {
                yield return null;
            }
        }

    }

    public class Waiter {
        private float timeLeft;
        private readonly bool realtime;
        public Waiter(float duration, bool realtime) {
            timeLeft = duration;
            this.realtime = realtime;
        }

        public bool Tick() {
            return (timeLeft -= GetDeltaTime()) > 0;
        }

        private float GetDeltaTime() {
            return realtime ? Time.unscaledDeltaTime : Time.deltaTime;
        }

        public void Clear() {
            timeLeft = 0;
        }
    }
}
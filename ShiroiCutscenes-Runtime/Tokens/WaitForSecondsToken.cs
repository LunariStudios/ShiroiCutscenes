using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Shiroi.Cutscenes.Tokens {
    [UsedImplicitly]
    public class WaitForSecondsToken : Token, ISkippable {
        public float Duration;
        public bool Realtime;
        private readonly Dictionary<CutsceneExecutor, Waiter> cache = new Dictionary<CutsceneExecutor, Waiter>();

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var w = new Waiter(Duration, Realtime);
            cache[executor] = w;
            while (w.Tick()) {
                yield return null;
            }
        }

        public void Skip(CutscenePlayer player, CutsceneExecutor executor) { }
    }

    public class Waiter {
        private float timeLeft;
        private readonly bool realtime;
        public Waiter(float duration, bool realtime) {
            this.timeLeft = duration;
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
using System.Collections;
using System.Collections.Generic;

namespace Shiroi.Cutscenes {
    public interface ISkippable {
        void Skip(CutscenePlayer player, CutsceneExecutor executor);
    }

    public class CutsceneExecutor {
        private bool shouldSkip;
        private readonly Cutscene cutscene;
        private readonly CutscenePlayer player;
        private Dictionary<string, object> metadata = new Dictionary<string, object>();

        public Cutscene Cutscene {
            get {
                return cutscene;
            }
        }

        public CutscenePlayer Player {
            get {
                return player;
            }
        }

        public CutsceneExecutor(Cutscene cutscene, CutscenePlayer player) {
            this.cutscene = cutscene;
            this.player = player;
        }

        public IEnumerator Execute() {
            foreach (var token in Cutscene) {
                if (shouldSkip) {
                    var s = token as ISkippable;
                    if (s != null) {
                        s.Skip(player, this);
                        continue;
                    }

                    Consume(token.Execute(Player, this));
                } else {
                    yield return token.Execute(Player, this);
                }
            }
        }

        private static void Consume(IEnumerator execute) {
            while (execute.MoveNext()) {
                var current = execute.Current as IEnumerator;
                if (current != null) {
                    Consume(current);
                }
            }
        }

        public void SetMeta(string key, object value) {
            metadata[key] = value;
        }

        public T GetMeta<T>(string key) where T : class {
            return GetMeta(key) as T;
        }

        public object GetMeta(string key) {
            return metadata.ContainsKey(key) ? key : null;
        }

        public void Skip() {
            shouldSkip = true;
        }
    }
}
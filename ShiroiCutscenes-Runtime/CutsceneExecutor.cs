using System.Collections;

namespace Shiroi.Cutscenes {
    public class CutsceneExecutor {
        private bool shouldSkip;

        public Cutscene Cutscene {
            get;
        }

        public CutscenePlayer Player {
            get;
        }

        public CutsceneExecutor(Cutscene cutscene, CutscenePlayer player) {
            Cutscene = cutscene;
            Player = player;
        }

        public IEnumerator Execute() {
            foreach (var token in Cutscene) {
                if (shouldSkip) {
                    Consume(token.Execute(Player, this));
                } else {
                    yield return token.Execute(Player, this);
                }
            }
        }

        private static void Consume(IEnumerator execute) {
            while (execute.MoveNext()) { }
        }


        public void Skip() {
            shouldSkip = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Shiroi.Cutscenes.Communication;

namespace Shiroi.Cutscenes {
    public class CutsceneExecutor {
        private readonly Cutscene cutscene;
        private readonly Context context;
        public Cutscene Cutscene {
            get {
                return cutscene;
            }
        }

        public Context Context {
            get {
                return context;
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.Cutscenes.Futures {
   

    public interface IFutureProvider {
        void RegisterFutures(Cutscene cutscene);
    }
}
using System;
using Shiroi.Cutscenes.Futures;

namespace Shiroi.Cutscenes.Util {
    public static class ReferenceUtility {
        public static int GetID(object reference) {
            var general = reference as Reference;
            if (general != null) {
                return general.Id;
            }
            var future = reference as FutureReference;
            if (future != null) {
                return future.Id;
            }
            throw new ArgumentOutOfRangeException("reference");
        }
    }
}
using System;
using System.Collections.Generic;

namespace Shiroi.Cutscenes.Util {
    public static class ListUtil {
        public static T GetOrPut<T>(this List<T> list, Predicate<T> predicate, Func<T> creator) {
            foreach (var obj in list) {
                if (predicate(obj)) {
                    return obj;
                }
            }
            var created = creator();
            list.Add(created);
            return created;
        }
    }
}
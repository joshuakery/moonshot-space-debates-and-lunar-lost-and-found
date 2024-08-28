using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ExtensionMethods
{
    public static void ResetAllTriggers(this Animator animator)
    {
        if (animator == null) return;
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }

    //Breadth-first search
     public static Transform FindDeepChild(this Transform aParent, string aName)
     {
         Queue<Transform> queue = new Queue<Transform>();
         queue.Enqueue(aParent);
         while (queue.Count > 0)
         {
             var c = queue.Dequeue();
             if (c.name == aName)
                 return c;
             foreach(Transform t in c)
                 queue.Enqueue(t);
         }
         return null;
     }    
 
     /*
     //Depth-first search
     public static Transform FindDeepChild(this Transform aParent, string aName)
     {
         foreach(Transform child in aParent)
         {
             if(child.name == aName )
                 return child;
             var result = child.FindDeepChild(aName);
             if (result != null)
                 return result;
         }
         return null;
     }
     */

    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    // Dictionary's KeyValuePair
    public static string ToStringExt<K, V>(this KeyValuePair<K, V> kvp)
    {
        return string.Format("[{0}] => {1}", kvp.Key, kvp.Value);
    }
    public static string ToStringExt<K1, K2, V>(this KeyValuePair<K1, Dictionary<K2, V>> kvp)
    {
        return string.Format("[{0}] => {1}", kvp.Key, kvp.Value.ToStringExt());
    }
    public static string ToStringExt<K1, V>(this KeyValuePair<K1, List<V>> kvp)
    {
        return string.Format("[{0}] => {1}", kvp.Key, kvp.Value.ToStringExt());
    }

    // Dictionary
    public static string ToStringExt<K, V>(this Dictionary<K, V> dic)
    {
        return "{" + string.Join(", ", dic.Select((kvp) => kvp.ToStringExt())) + "}";
    }
    public static string ToStringExt<K, K2, V>(this Dictionary<K, Dictionary<K2, V>> dic)
    {
        return "{" + string.Join(", ", dic.Select((kvp) => kvp.ToStringExt())) + "}";
    }
    public static string ToStringExt<K, V>(this Dictionary<K, List<V>> dic)
    {
        return "{" + string.Join(", ", dic.Select((kvp) => kvp.ToStringExt())) + "}";
    }

    // List
    public static string ToStringExt<T>(this List<T> list) => "[" + string.Join(", ", list) + "]";

    public static string ToStringExt<T>(this List<List<T>> listOfLists) => "[" + string.Join(", ", listOfLists.Select(l => l.ToStringExt())) + "]";

    public static string ToStringExt<K, V>(this List<Dictionary<K, V>> listOfDics) => "[" + string.Join(", ", listOfDics.Select(kvp => kvp.ToStringExt())) + "]";

    public static string ToStringExt<T>(this List<List<List<T>>> list3) => "[" + string.Join(", ", list3.Select(l => l.ToStringExt())) + "]";

}

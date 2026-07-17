using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ArrayExpansion
{
    public static Array AddToArrayEnd(object o, Array a){
        Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length + 1);
        a.CopyTo(b, 0);
        b.SetValue(o, a.Length);
        a = b;
        return a;
    }

    
    public static Array AddToArrayAtIndex(object o, Array a, int index){
        Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length + 1);
    for (int i = 0; i < index; i++)
    {
        b.SetValue(a.GetValue(i), i);
    }
    for (int i = index + 1; i < b.Length; i++)
    {
        b.SetValue(a.GetValue(i - 1), i);
    }

    b.SetValue(o, index);
    a = b;
    return a;
    }
}

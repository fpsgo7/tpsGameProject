using System.Collections;
using UnityEngine;

public class ArrayHelper
{
    // list 관련 추가 삭제 기능을 구현하여
    // 다른 스크립트 에서 불러와 간단하게 처리하기 위해 사용한다.
    public static T[] Add<T>(T n, T[] list)
    {
        ArrayList tmp = new ArrayList();
        foreach (T item in list) tmp.Add(item);
        tmp.Add(n);
        return tmp.ToArray(typeof(T)) as T[];
    }

    public static T[] Remove<T>(int index, T[] list)
    {
        ArrayList tmp = new ArrayList();
        foreach (T item in list) tmp.Add(item);
        tmp.RemoveAt(index);
        return tmp.ToArray(typeof(T)) as T[];
    }

    public static T[] Copy<T>(T[] source)
    {
        if (source == null) return null;
        ArrayList tmp = new ArrayList();
        foreach (T item in source) tmp.Add(item);
        return tmp.ToArray(typeof(T)) as T[];
    }
}

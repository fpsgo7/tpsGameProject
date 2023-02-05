using System.Collections;
using UnityEngine;

public class ArrayHelper
{
    // list 관련 추가 삭제 기능을 구현하여
    // 다른 스크립트 에서 불러와 간단하게 처리하기 위해 사용한다.
    public static T[] Add<T>(T newItem, T[] source)
    {
        ArrayList arrayList = new ArrayList();
        foreach (T sourceItem in source) arrayList.Add(sourceItem);//원래있던 아이템을 새로운 리스트에 추가한다.
        arrayList.Add(newItem);//추가할 아이템을 추가한다.
        return arrayList.ToArray(typeof(T)) as T[];
    }

    public static T[] Remove<T>(int index, T[] source)
    {
        ArrayList arrayList = new ArrayList();
        foreach (T sourceItem in source) arrayList.Add(sourceItem);
        arrayList.RemoveAt(index);
        return arrayList.ToArray(typeof(T)) as T[];
    }

    public static T[] Copy<T>(T[] source)
    {
        if (source == null) return null;
        ArrayList arrayList = new ArrayList();
        foreach (T sourceItem in source) arrayList.Add(sourceItem);
        return arrayList.ToArray(typeof(T)) as T[];
    }
}

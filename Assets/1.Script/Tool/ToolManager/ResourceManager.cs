using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;//UnityEngine.Object 를 UnityObject로 박싱
/// <summary>
/// Resources.Load 를 래핑하는 클래스
/// 리소스에서 오브젝트를 주소를 통해 찾아
/// 로드하여 게임오브젝트 형식으로 출력한다.
/// </summary>
public class ResourceManager
{
    // 유니티 오브젝트를 로드하는 함수
    public static UnityObject Load(string path)
    {
        //Resources UnityEngine 의 클래스이며 Load 함수 를 사용한다.
        return Resources.Load(path);
    }
    //로드한후 생성합니다.
    public static GameObject LoadAndInstantiate(string path)
    {
        UnityObject source = Load(path); // 위의 Load 함수의 유니티 오브젝트 를 넣음
        if (source == null)
        {
            return null;
        }
        //Load 를 통해 얻어온 오브젝트를 넣은해당 소스를 게임오브젝트 형테로 출력한다.
        return GameObject.Instantiate(source) as GameObject;
    }
}


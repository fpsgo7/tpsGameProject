using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 프리팹과 경로와 타입등의 속성 데이터를 가지고 있게 되며
/// 프리팹 사전로딩 기능을 갖고 있고 - 풀링을 위한 기능이기도합니다.
/// 이펙트 인스턴스 기능도 갖고 있으며 - 풀링과 연계해서 사용하기도 합니다.
/// </summary>
public class ObjectClip
{
    //추후 속성은 같지만 다른 이펙트 클립이 있어서 분별용 id
    public int id = 0;
    public ObjectType objectType = ObjectType.NORMAL;//따른 스크립트에서 enum 형변수에 접근해 오브젝트 타입 구분을 위해 사용
    public GameObject gameObject = null;// 오브젝트
    public string objectName = string.Empty;// 오브젝트 이름
    public string objectPath = string.Empty;// 오브젝트 경로
    public string objectFullPath = string.Empty;// 오브젝트 경로 + 오브젝트 이름
    // 생성자
    public ObjectClip() { }
    // 사전 로드 기능
    public void PreLoad()
    {

        this.objectFullPath = objectPath + objectName;
        // 이경로에 경로 대이터가 있어야하고 한번도 사전로딩 되지 않았을경우 실행한다.
        //한번 만 실행하기 위해 조건문 사용
        if (this.objectFullPath != string.Empty && this.gameObject == null)
        {
            //ResourceManager의 Load 함수를 사용 매계변수를 objectFullPaht 로 경로를
            //지정한후 해당 경로로 게임오브젝트를 로드한다.
            this.gameObject = ResourceManager.Load(objectFullPath) as GameObject;
        }
    }
    /// <summary>
    /// 오브젝트 풀링을 위해 게임 시작할때 미리 생성을 위해사용된다.
    /// </summary>
    public GameObject Instantiate(Vector3 Pos)
    {
        if (this.gameObject == null)
        {
            this.PreLoad(); //생성할 대상이 없을경우 PreLoad로 로드해온다.
        }
        if (this.gameObject != null)
        {
            //effectPrefb 게임오브젝트를 Pos 위치에 (identity)기본 각도로 생성한다.
            GameObject createObject = GameObject.Instantiate(this.gameObject, Pos, Quaternion.identity);
            return createObject;
        }
        return null;
    }
}

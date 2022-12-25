using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public static class BackEndKeySetting
{
    public static KeySettingData GetPlayerKeySetting()
    {
        //사용할 객체 생성
        KeySettingData keySettingData = new KeySettingData();
        //자신의 아이디에 맞는 정보를 가져옴
        BackendReturnObject bro = Backend.GameData.GetMyData("KeySettingInfo", new Where(), 100);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return keySettingData;
        }
        else
        {
            keySettingData.FIRE = (int)bro.FlattenRows()[0]["FIRE"];
            keySettingData.ZOOMIN = (int)bro.FlattenRows()[0]["ZOOMIN"];
            keySettingData.GRENADE = (int)bro.FlattenRows()[0]["GRENADE"];
            keySettingData.JUMP = (int)bro.FlattenRows()[0]["JUMP"];
            keySettingData.RELOAD = (int)bro.FlattenRows()[0]["RELOAD"];
            keySettingData.RESTOREHEALTH = (int)bro.FlattenRows()[0]["RESTOREHEALTH"];
            keySettingData.SCOPEZOOMIN = (int)bro.FlattenRows()[0]["SCOPEZOOMIN"];
            keySettingData.INVENTORY = (int)bro.FlattenRows()[0]["INVENTORY"];
            keySettingData.INTERACTION = (int)bro.FlattenRows()[0]["INTERACTION"];
            keySettingData.RUN = (int)bro.FlattenRows()[0]["RUN"];
            return keySettingData;
        }
    }

    // Insert 는 '생성' 작업에 주로 사용된다. 
    public static void InsertPlayerKeySetting(string id)
    {

        // Param은 뒤끝 서버와 통신을 할 떄 넘겨주는 파라미터 클래스 입니다. 
        Param param = new Param();
        //int 형에서 string 형으로 자동 변환 이 되지않아 문자열 형태로 교체후 사용할 것
        param.Add("ID", id);
        param.Add("FIRE", (int)KeyCode.Mouse0);
        param.Add("ZOOMIN", (int)KeyCode.Mouse1);
        param.Add("GRENADE", (int)KeyCode.G);
        param.Add("JUMP", (int)KeyCode.Space);
        param.Add("RELOAD", (int)KeyCode.R);
        param.Add("RESTOREHEALTH", (int)KeyCode.V);
        param.Add("SCOPEZOOMIN", (int)KeyCode.Tab);
        param.Add("INVENTORY", (int)KeyCode.M);
        param.Add("INTERACTION", (int)KeyCode.E);
        param.Add("RUN", (int)KeyCode.LeftShift);


        BackendReturnObject BRO = Backend.GameData.Insert("KeySettingInfo", param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    public static void SetPlayerKeySetting(string id,KeySettingData keySettingData)
    {
        Where where = new Where();
        where.Equal("ID", id);
        Param param = new Param();
        param.Add("FIRE", keySettingData.FIRE);
        param.Add("ZOOMIN", keySettingData.ZOOMIN);
        param.Add("GRENADE", keySettingData.GRENADE);
        param.Add("JUMP", keySettingData.JUMP);
        param.Add("RELOAD", keySettingData.RELOAD);
        param.Add("RESTOREHEALTH", keySettingData.RESTOREHEALTH);
        param.Add("SCOPEZOOMIN", keySettingData.SCOPEZOOMIN);
        param.Add("INVENTORY", keySettingData.INVENTORY);
        param.Add("INTERACTION", keySettingData.INTERACTION);
        param.Add("RUN", keySettingData.RUN);

        BackendReturnObject bro = Backend.GameData.Update("KeySettingInfo", where, param);

        if (bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log("실패" + bro);
            return;
        }
        else
        {
            Debug.Log("성공" + bro);
        }
    }
}

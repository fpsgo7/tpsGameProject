using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string id;
    public string name;
    public int score;
    public int weaponNum;
    public int equipmentNum;
    public float axisX;
    public float axisY;

    public PlayerInfo(string id, string name, int score, int weaponNum, int equipmentNum, float axisX, float axisY)
    {
        this.id = id;
        this.name = name;
        this.score = score;
        this.weaponNum = weaponNum;
        this.equipmentNum = equipmentNum;
        this.axisX = axisX;
        this.axisY = axisY;
    }

    public void SetPlayerInfo(string id, string name, int score, int weaponNum, int equipmentNum, float axisX, float axisY)
    {
        this.id = id;
        this.name = name;
        this.score = score;
        this.weaponNum = weaponNum;
        this.equipmentNum = equipmentNum;
        this.axisX = axisX;
        this.axisY = axisY;
    }
}

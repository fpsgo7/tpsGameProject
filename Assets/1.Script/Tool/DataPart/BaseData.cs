using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// data 의 기본 클래스
/// data형 클래스의 부모클래스가 된다.
/// 공통적인 데이터를 가지고 있게된다. 이름만 현제 갖고있다.
/// 데이터의 갯수와 이름의 목록 리스트를 얻을 수 있다.
/// </summary>
public class BaseData : ScriptableObject
{
    //유니티 에셋의 주소값으로 에셋에 접근하기위해 사용
    public const string dataDirectory = "/3.DataResources/Resources/Data/";
    public string[] dataNames = null;//이름들을 넣기위한 배열 변수
    //생성자
    public BaseData()
    {

    }
    // 가지고 있는 데이터의 곘수를 반환한다.
    //반환값으로 int를 받는다.
    public int GetDataCount()
    {
        int dataCount = 0;//반환값
        //이클래스의 names 가 널이 아닌경우
        //(로딩이 끝난경우도 파악할 수 잇다.)길이를 반환한다.
        if (this.dataNames != null)
        {
            dataCount = this.dataNames.Length;
        }
        return dataCount;
    }
    /// <summary>
    /// 툴에 출력하기 위한 이름 목록을 만들어주는 함수 
    /// showID의 true false에 따라 배열의 몇번째인지 보여줄지 말지 결정할 수 있다.
    /// </summary>
    public string[] GetNameList(bool isShowID, string filterWord = "")
    {
        // 리스트 생성 해당 리스트에 값을 넣은후 반환함
        string[] dataNameList = new string[0];
        //이름들의 널이면 밑의 내용을 실행하지 않음
        if (this.dataNames == null)
        {
            return dataNameList;
        }
        //리스트에 names의 길이를 미리 집어넣음
        dataNameList = new string[this.dataNames.Length];
        for (int i = 0; i < this.dataNames.Length; i++)
        {
            if (filterWord != "")
            {
                //ToLower 글자들을 소문자로 바꿈 했깔리기를 방지하기 위함
                //names 배열에서 추출한 글자가 fillterWord를 포함하지 않는 다면 true
                if (dataNames[i].ToLower().Contains(filterWord.ToLower()) == false)
                {
                    continue;
                }

            }
            if (isShowID)
            {// i.ToString() 으로 배열의 몇번째인지 알 수 있으며 해당값이 ID 처럼 사용된다.
                dataNameList[i] = i.ToString() + " : " + this.dataNames[i];
            }
            else
            {
                dataNameList[i] = this.dataNames[i];
            }
        }
        return dataNameList;
    }
    //virtual 은 상속받은 스크립트에서 완성한다.
    //데이터를 추가할 경우
    public virtual int AddData(string newName)
    {
        return GetDataCount();//데이터가 추가되므로 숫자를 추가해준다.
    }
    //데이터 상제
    public virtual void RemoveData(int index)
    {

    }
    //데이터 복사
    public virtual void Copy(int index)
    {

    }
}

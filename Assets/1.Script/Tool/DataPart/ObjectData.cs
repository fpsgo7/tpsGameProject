﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

/// <summary>
/// 이펙트 클립 리스트 와 이펙트 파일 이름과 경로를 가지고 있으며 
/// 파일을 읽고 쓰는 기능을 가지고 있다.
/// </summary>
public class ObjectData : BaseData//BaseData를 상속받아 스크립터블 오브젝트가된다
{
    //EffectClip을 통한 스크립트형 변수 객체 배열로 보면된다.
    public ObjectClip[] objectClips = new ObjectClip[0];

    private string xmlFilePath = "";//xml 파일 경로 dataDirectory의 값을 널어사용한다.
    private string xmlFileName = "objectData.xml";//xml 파일 내임
    private string dataPath = "Data/objectData";//오브젝트파일 데이터 경로
    //XML 구분자
    private const string OBJECT = "object";// 저장키
    private const string CLIP = "clip";// 저장키
    //생성자
    private ObjectData() { }

    //읽어오고 저장하고, 데이터를 삭제하고, 특정 클립을 얻어오고, 복사하는 기능
    //읽어오기
    public void LoadData()
    {
        this.xmlFilePath = Application.dataPath + dataDirectory;// xml 파일 경로
        //TextAsset 에 리소스메니저로 경로를 통해 받아온 정보를 텍스트형태로  넣는다.
        TextAsset asset = (TextAsset)ResourceManager.Load(dataPath);
        if (asset == null || asset.text == null)
        {
            this.AddData("NewObject");//데이터가 없을경우 데이터 추가
            return;
        }
        //using 문법 사용하여 IO 관련 예외처리를 간단하게한다.
        //asset 택스트를 스트링형으로 바꾼후 다시 xml 형식으로 변환한다.
        //빠르고, 캐시되지 않으며 앞으로만 이동 가능한 XML 데이터 액세스를 제공하는 판독기
        using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
        {
            int currentID = 0;
            while (reader.Read()) //XmlTextReader 가 읽는동안 
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "length":
                            //오브젝트 클립수를 가져온다.
                            //가져운값을 인트화 시킨후 해당 길이를 names 배열의 길이를
                            //적용한다.
                            int length = int.Parse(reader.ReadString());
                            this.dataNames = new string[length];
                            this.objectClips = new ObjectClip[length];
                            break;
                        case "id":
                            //아이디를 얻어온후 이펙트 클립 배열에 접근하여
                            //아이디를 넣는다.
                            currentID = int.Parse(reader.ReadString());
                            //객체 생성후 정보를 집어넣는다.
                            //현제 아이디 값을 배열 번째에 맞춰 넣는다.
                            this.objectClips[currentID] = new ObjectClip();
                            this.objectClips[currentID].id = currentID;
                            break;
                        case "name":
                            this.dataNames[currentID] = reader.ReadString();
                            break;
                        case "objectType":
                            this.objectClips[currentID].objectType = (ObjectType)
                                Enum.Parse(typeof(ObjectType), reader.ReadString());
                            break;
                        case "objectName":
                            this.objectClips[currentID].objectName = reader.ReadString();
                            break;
                        case "objectPath":
                            this.objectClips[currentID].objectPath = reader.ReadString();
                            break;
                        case "objectnecessary":
                            this.objectClips[currentID].objectnecessary = int.Parse(reader.ReadString());
                            break;
                        case "objectTime":
                            this.objectClips[currentID].objectTime = int.Parse(reader.ReadString());
                            break;
                    }
                }
            }
        }
    }
    //저장하기 새로 추가하거나 삭제한 결과를 xml 에 저장하여서 게임이 종료되어서도 저장되기 위해 사용
    public bool SaveData()
    {
        using (XmlTextWriter xml = new XmlTextWriter
            (xmlFilePath + xmlFileName, System.Text.Encoding.Unicode))
        {
            //OBJECT 키 는 전체 범위로 length 데이터의 개수값을 가지며
            //xml 에서 클립간 구분은 CLIP 키를 통해 구준한다.
            xml.WriteStartDocument();//Document 시작
            xml.WriteStartElement(OBJECT);//OBJECT 를 이용한 element를 시작한다.
            //xml 파일의 length 컬럼 에 GetDataCount()를 통해 얻어온 데이타의 길이값을 넣는다.
            xml.WriteElementString("length", GetDataCount().ToString());
            // 각 클립별로 맞춰 정보를 입력한다.
            for (int i = 0; i < this.dataNames.Length; i++)
            {
                ObjectClip clip = this.objectClips[i];//각번째에 맞은 이펙트 배열의 하나를 연결
                xml.WriteStartElement(CLIP);//CLIP을 통해 구분한다. 
                                            //아이템 클립에 각 변수에 값을 적용함
                xml.WriteElementString("id", i.ToString());
                xml.WriteElementString("name", this.dataNames[i]);
                xml.WriteElementString("objectType", clip.objectType.ToString());
                xml.WriteElementString("objectPath", clip.objectPath);
                xml.WriteElementString("objectName", clip.objectName);
                xml.WriteElementString("objectnecessary", clip.objectnecessary.ToString());
                xml.WriteElementString("objectTime", clip.objectTime.ToString());
                xml.WriteEndElement();//CLIP element 종료
            }
            xml.WriteEndElement();//OBJECT element 종료
            xml.WriteEndDocument();//Document 종료
            return true;
        }
    }

    //추가하기 추가한후 저장하지 않으면 추가한 값은 xml 파일에 저장되지 않는다.
    public override int AddData(string newName)
    {
        //추가하는 작업 하기
        if (this.dataNames == null)//처음 추가하는 경우
        {
            this.dataNames = new string[] { name };
            this.objectClips = new ObjectClip[] { new ObjectClip() };
        }
        else//리스트 객체가 이미 만들어진경우 Add를 활용하여 추가
        {
            //ArrayHelper은 유틸성 스크립트로 기본적인 기능을 구현하여 함수로 불러 사용하기위해 쓴다.
            this.dataNames = ArrayHelper.Add(name, this.dataNames);//this.names 라는 리스트에 name을 추가한다.
            this.objectClips = ArrayHelper.Add(new ObjectClip(), this.objectClips);
        }
        return GetDataCount();//추가하기로 증가한 길이를 전달한다.
    }
    //데이터 삭제 마찬가지로 저장하지 않으면 삭제한 값은 xml 파일에 적용되지 않는다.
    public override void RemoveData(int index)
    {
        this.dataNames = ArrayHelper.Remove(index, this.dataNames);
        if (this.dataNames.Length == 0)//데이터 삭제한후 길이가 0이된경우
        {
            this.dataNames = null;
        }
        this.objectClips = ArrayHelper.Remove(index, this.objectClips);
    }
    //데이터 복사
    public override void Copy(int index)
    {
        this.dataNames = ArrayHelper.Add(this.dataNames[index], this.dataNames);
        //GetCopy를 통해 얻은 정보를 Add 하여 추가한다.
        this.objectClips = ArrayHelper.Add(GetCopy(index), this.objectClips);
    }

    // 복사를 위한 정보 값 얻기
    public ObjectClip GetCopy(int index)
    {
        //index는 복사대상의 몇번째인지 알려주는 수로
        //id를 통해 찾듯이 번째로 찾아 복사한다.
        // 복사한 것을 리턴해준다.
        if (index < 0 || index >= this.objectClips.Length)
        {
            return null;
        }
        ObjectClip original = this.objectClips[index];
        ObjectClip clip = new ObjectClip();
        clip.objectFullPath = original.objectFullPath;
        clip.objectName = original.objectName;
        clip.objectType = original.objectType;
        clip.objectPath = original.objectPath;
        clip.id = this.objectClips.Length;
        return clip;
    }

    /// <summary>
    /// 원하는 인덱스를 프리로딩 해서 찾아준다.
    /// </summary>
    public ObjectClip GetClip(int index)
    {
        if (index < 0 || index >= this.objectClips.Length)
        {
            return null;
        }
        objectClips[index].PreLoad();
        return objectClips[index];
    }

}
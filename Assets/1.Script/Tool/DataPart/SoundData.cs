using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

/// <summary>
/// 사운드 클립을 배열로 소지 , 사운드 데이터를 저장하고 로드하고 , 
/// 프리로딩을 갖고 있다.
/// </summary>
public class SoundData : BaseData
{
    public SoundClip[] soundClips = new SoundClip[0];//사운드 클립 배열
    private string xmlFilePath = "";//xml 파일경로
    private string xmlFileName = "soundData.xml";// 사운드 클립관련 정보를 저장할 xml 파일이름
    private string dataPath = "Data/soundData";// 데이타 경로
    private static string SOUND = "sound";// xml 파일에서 구분을 위한 저장키
    private static string CLIP = "clip";// xml 파일에서 구분을 위한 저장키
    //생성자
    private SoundData() { }
    // 데이터 로드하기
    public void LoadData()
    {
        this.xmlFilePath = Application.dataPath + dataDirectory;// xml 파일을 불러오기위한 주소 지정
        //경로를 통하여 로드한 정보를 TExtAsset 변수에 넣기
        TextAsset asset = (TextAsset)Resources.Load(dataPath, typeof(TextAsset));
        //만약 아무 정보도 없을경우
        if (asset == null || asset.text == null)
        {
            this.AddData("NewSound");//기초데이타로 추가
            return;
        }
        // XmlTextReader을 이용하여 읽을 xml 파일을 text 형태로 가져와 넣는다.
        using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
        {
            int currentID = 0;
            while (reader.Read())// 파일을 읽는 동안 true
            {
                if (reader.IsStartElement())// 시작
                {
                    switch (reader.Name)// 리더의 이름 에따라 구분함
                    {
                        case "length":// 사운드 데이터 개수
                            int length = int.Parse(reader.ReadString());
                            this.names = new string[length];// 개수를 배열개수에 적용 배열 생성
                            this.soundClips = new SoundClip[length];
                            break;
                        case "id":// id 값
                            currentID = int.Parse(reader.ReadString());
                            soundClips[currentID] = new SoundClip();
                            soundClips[currentID].realId = currentID;
                            break;
                        case "name":// 이름
                            this.names[currentID] = reader.ReadString();
                            break;
                        case "clippath":// 클립 경로
                            soundClips[currentID].clipPath = reader.ReadString();
                            break;
                        case "clipname":// 클립 이름
                            soundClips[currentID].clipName = reader.ReadString();
                            break;
                        case "type":// 타입 
                            //타입을 가져와해당 배열에 넣어준다.
                            soundClips[currentID].playType = (SoundPlayType)
                                Enum.Parse(typeof(SoundPlayType), reader.ReadString());
                            break;
                    }
                }
            }
        }
        // 프리로딩 테스트
        //스트리밍 할때 버벅이지 않기위해 사용 사양 타면 삭제
        foreach (SoundClip clip in soundClips)
        {
            clip.PreLoad();
        }
    }
    // 데이터 저장하기
    public void SaveData()
    {
        //using을 활용하여 예외처리를 하며
        //XmlTextWriter  객체를 파일 경로와 이름으로 위치를 찾아 넣어주고
        //택스트 인코딩은 유니코드로 한다.
        using (XmlTextWriter xml = new XmlTextWriter(xmlFilePath + xmlFileName,
            System.Text.Encoding.Unicode))
        {

            xml.WriteStartDocument();// 다큐먼트 시작
            xml.WriteStartElement(SOUND);// Sound Element 시작
            xml.WriteElementString("length", GetDataCount().ToString());// 사운드 개수를 적용
            xml.WriteWhitespace("\n");// 줄바꾸기
            // 사온드 개수많큼 반복하며 클립 내용 넣기
            for (int i = 0; i < this.names.Length; i++)
            {
                SoundClip clip = this.soundClips[i];//soundClips 배열에서 알맞은 값 넣기
                xml.WriteStartElement(CLIP);//Clip 엘리먼트 시작
                xml.WriteElementString("id", i.ToString());//몇번째를 나타내는 아이디 입력
                xml.WriteElementString("name", this.names[i]);// 이름 임력
                xml.WriteElementString("clippath", clip.clipPath);// 클립경로
                xml.WriteElementString("clipname", clip.clipName);// 클립이름
                xml.WriteElementString("type", clip.playType.ToString());// 타입 넣기
                xml.WriteEndElement();// clip 앨리 먼트 종료
            }
            xml.WriteEndElement();//sound 앨리먼트 종료
            xml.WriteEndDocument();// 다큐먼트 종료
        }
    }
    // 데이터 추가 
    public override int AddData(string newName)
    {
        // 새로운 이름과 클립을 추가한다.
        if (this.names == null)
        {
            this.names = new string[] { newName };
            this.soundClips = new SoundClip[] { new SoundClip() };
        }
        else
        {
            this.names = ArrayHelper.Add(newName, names);
            this.soundClips = ArrayHelper.Add(new SoundClip(), soundClips);
        }
        return GetDataCount();// 개수가 봐껴 새로운 숫자를 반환한다.
    }
    // 데이터 삭제
    public override void RemoveData(int index)
    {
        // 클립스와 이름들에서 index로 찾은 대상을 제거
        this.names = ArrayHelper.Remove(index, this.names);
        if (this.names.Length == 0)
        {
            this.names = null;
        }
        this.soundClips = ArrayHelper.Remove(index, this.soundClips);
    }
    // 복사작업
    public SoundClip GetCopy(int index)
    {
        if (index < 0 || index >= soundClips.Length)
        {
            return null;
        }
        // 복사에 사용하기위한 변수 생성 복사할 대상의 정보를 저장할 변수 생성
        SoundClip clip = new SoundClip();
        SoundClip original = soundClips[index];
        // 값들을 넣음
        clip.realId = index;
        clip.clipPath = original.clipPath;
        clip.clipName = original.clipName;
        clip.playType = original.playType;
        clip.PreLoad();
        return clip;
    }
    // 실제 데이터 복사
    public override void Copy(int index)
    {
        this.names = ArrayHelper.Add(this.names[index], this.names);
        this.soundClips = ArrayHelper.Add(GetCopy(index), soundClips);
    }
}

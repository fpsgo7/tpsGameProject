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
        clip.maxVolume = original.maxVolume;
        clip.pitch = original.pitch;
        clip.dopplerLevel = original.dopplerLevel;
        clip.rolloffMode = original.rolloffMode;
        clip.minDistance = original.minDistance;
        clip.maxDistance = original.maxDistance;
        clip.spartialBlend = original.spartialBlend;
        clip.isLoop = original.isLoop;
        clip.checkTime = new float[original.checkTime.Length];
        clip.setTime = new float[original.setTime.Length];
        clip.playType = original.playType;
        for (int i = 0; i < clip.checkTime.Length; i++)
        {
            clip.checkTime[i] = original.checkTime[i];
            clip.setTime[i] = original.setTime[i];
        }
        clip.PreLoad();
        return clip;
    }
    // 실제 데이터 복사
    public override void Copy(int index)
    {
        this.names = ArrayHelper.Add(this.names[index], this.names);
        this.soundClips = ArrayHelper.Add(GetCopy(index), soundClips);
    }
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
                        case "clip":
                            break;
                        case "id":// id 값
                            currentID = int.Parse(reader.ReadString());
                            soundClips[currentID] = new SoundClip();
                            soundClips[currentID].realId = currentID;
                            break;
                        case "name":// 이름
                            this.names[currentID] = reader.ReadString();
                            break;
                        case "loops":// 루프
                            int count = int.Parse(reader.ReadString());
                            soundClips[currentID].checkTime = new float[count];
                            soundClips[currentID].setTime = new float[count];
                            break;
                        case "maxvol":// 최대 볼륨
                            soundClips[currentID].maxVolume = float.Parse(reader.ReadString());
                            break;
                        case "pitch":// 피치
                            soundClips[currentID].pitch = float.Parse(reader.ReadString());
                            break;
                        case "dolpplerlevel":// 도플러 레벨
                            soundClips[currentID].dopplerLevel = float.Parse(reader.ReadString());
                            break;
                        case "rolloffmode":// 롤오프 모드
                            soundClips[currentID].rolloffMode = (AudioRolloffMode)
                                Enum.Parse(typeof(AudioRolloffMode), reader.ReadString());
                            break;
                        case "mindistance":// 최소거리
                            soundClips[currentID].minDistance = float.Parse(reader.ReadString());
                            break;
                        case "maxdistance":// 최대 거리
                            soundClips[currentID].maxDistance = float.Parse(reader.ReadString());
                            break;
                        case "spartialBlend":// 스패셜 블랜드
                            soundClips[currentID].spartialBlend = float.Parse(reader.ReadString());
                            break;
                        case "loop":// 루프
                            soundClips[currentID].isLoop = true;
                            break;
                        case "clippath":// 클립 경로
                            soundClips[currentID].clipPath = reader.ReadString();
                            break;
                        case "clipname":// 클립 이름
                            soundClips[currentID].clipName = reader.ReadString();
                            break;
                        case "checktimecount":// 아이템 개수 루프에서 이미 기능을 처리하여 필요 x
                            break;
                        case "checktime":// 시간 체크
                            SetLoopTime(true, soundClips[currentID], reader.ReadString());
                            break;
                        case "settime":// 시간설정
                            SetLoopTime(false, soundClips[currentID], reader.ReadString());
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
    // 시간값을 슬레시로 쪼개 배열로 나누어 사용할수 있게한다.
    void SetLoopTime(bool isCheck, SoundClip clip, string timeString)
    {
        string[] time = timeString.Split('/');//timeString 이 /을 기준으로 나눠저서 배열에 하나씩 들어간다.
        for (int i = 0; i < time.Length; i++)
        {
            if (time[i] != string.Empty)
            {
                if (isCheck == true)
                {
                    clip.checkTime[i] = float.Parse(time[i]);
                }
                else
                {
                    clip.setTime[i] = float.Parse(time[i]);
                }
            }
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
                xml.WriteElementString("loops", clip.checkTime.Length.ToString());// 체크타임 입력
                xml.WriteElementString("maxvol", clip.maxVolume.ToString());// 최대 볼륨
                xml.WriteElementString("pitch", clip.pitch.ToString());// 피치
                xml.WriteElementString("dopplerlevel", clip.dopplerLevel.ToString());// 도플러 레벨
                xml.WriteElementString("rolloffmode", clip.rolloffMode.ToString());// 롤오프 모드 
                xml.WriteElementString("mindistance", clip.minDistance.ToString());// 최소 거리
                xml.WriteElementString("maxdistance", clip.maxDistance.ToString());// 최대 거리
                xml.WriteElementString("sparialBlend", clip.spartialBlend.ToString());// 스패셜 블랜드
                if (clip.isLoop == true)// 루프가 존재할 경우
                {
                    // 루프가 존재하기 때문에 루프 컬럼을 추가하며 true 값 wjrdyd
                    xml.WriteElementString("loop", "true");
                }
                xml.WriteElementString("clippath", clip.clipPath);// 클립경로
                xml.WriteElementString("clipname", clip.clipName);// 클립이름
                xml.WriteElementString("checktimecount", clip.checkTime.Length.ToString());//시간 체크 개수
                string str = "";//반복문에 쓰일 스트링형 변수 생성
                // 반복문을 통하여 배열에 글자를 넣으며 / / 을 통하여 구분한다.
                foreach (float t in clip.checkTime)
                {
                    str += t.ToString() + "/";
                }
                xml.WriteElementString("checktime", str);// 체크 타임에  만든 스트링값을 넣는다.
                xml.WriteElementString("settimecount", clip.setTime.Length.ToString());// 시간 설정 값 개수
                str = "";
                foreach (float t in clip.setTime)
                {
                    str += t.ToString() + "/";
                }
                xml.WriteElementString("settime", str);// 슬래시 로 구분된 설정 시간 값이 들어간다.
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
        if (this.name.Length == 0)
        {
            this.names = null;
        }
        this.soundClips = ArrayHelper.Remove(index, this.soundClips);
    }
}

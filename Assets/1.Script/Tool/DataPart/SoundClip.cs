using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 사운드 클립들을 담당하는 스크립트이다.
/// </summary>
public class SoundClip 
{
    //SoundPlayType은 GlobalDefine 에서 가져와 사용한다.
    public SoundPlayType playType = SoundPlayType.None;// None으로 초기화한다.
    public string clipName = string.Empty;// 클립 이름
    public string clipPath = string.Empty;//클랩 경로
    public float maxVolume = 1.0f;// 볼륨
    public bool isLoop = false;// 반복 유무
    public float[] checkTime = new float[0];// 시간체크
    public float[] setTime = new float[0];// 설정시간
    public int realId = 0;// 아이디
    //오디오 클립
    private AudioClip clip = null;// 오디오 클립
    public int currentLoop = 0; // 몇개가 반복중인지 값
    public float pitch = 1.0f;// 핏치 
    public float dopplerLevel = 1.0f;// 도플러 레벨
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;// 오디오 롤 오프 모드
    public float minDistance = 10000.0f;// 최소 거리
    public float maxDistance = 50000.0f;// 최대 거리
    public float spartialBlend = 1.0f;// 사운드 블랜드 타입

    public float fadeTime1 = 0.0f;//패이드 타임1
    public float fadeTime2 = 0.0f;// 페이드 타임2
   
    public bool isFadeIn = false;// 패이드인 중인가
    public bool isFadeOut = false;// 패이드아웃 중인가.


    // 오디오 클립을 가져오기
    // 오디오클립을 반환해주는 함수이다.
    public AudioClip GetClip()
    {
        if (this.clip == null)
        {
            PreLoad();// 클립이 없으면 PreLoad()로 클립을 얻어준다.
        }
        // 클립이 없고 클립 이름이 있을경우 해당 클립은 존제자체가 오류이므로
        // 문제가 있는걸 알려준다.
        if (this.clip == null && this.clipName != string.Empty)
        {
            Debug.LogWarning($"Can not load audio clip Resource {this.clipName}");
            return null;
        }
        return this.clip;// 로드한 클립을 반환
    }

    // 사운드 클립을 경로를 통하여 로드하는 기능이다.
    public void PreLoad()
    {
        if (this.clip == null)
        {
            string fullPath = this.clipPath + this.clipName;
            this.clip = ResourceManager.Load(fullPath) as AudioClip;
        }
    }
    // 루프 추가하기
    public void AddLoop()
    {
        // 리스트 관련 도움을 주는 클래스를 활용하여 내용을 추가한다.
        // 루프 반복에 필요한 시간을 넣는다.
        this.checkTime = ArrayHelper.Add(0.0f, this.checkTime);
        this.setTime = ArrayHelper.Add(0.0f, this.setTime);
    }
    // 루프 제거하기
    public void RemoveLoop(int index)
    {
        // 리스트 관련 도움을 주는 클래스를 활용하여 내용을 삭제한다.
        this.checkTime = ArrayHelper.Remove(index, this.checkTime);
        this.checkTime = ArrayHelper.Remove(index, this.setTime);
    }
}

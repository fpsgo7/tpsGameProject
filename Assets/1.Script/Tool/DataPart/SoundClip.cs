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
    public bool isLoop = false;// 반복 유무
    public int realId = 0;// 아이디
    //오디오 클립
    private AudioClip clip = null;// 오디오 클립
    public int currentLoop = 0; // 몇개가 반복중인지 값



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
}

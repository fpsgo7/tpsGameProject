using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundToolManager : MonoBehaviour
{
    private static SoundToolManager instance;

    public static SoundToolManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SoundToolManager>();

            return instance;
        }
    }

    // 사용할 문자열들을 미리 문자열 변수에 넣어 사용한다.
    // 상수 스트링 변수들
    public const string MasterGroupName = "Master";
    public const string EffectGroupName = "Effect";
    public const string BGMGroupName = "BGM";
    public const string UIGroupName = "UI";
    public const string MixerName = "AudioMixer";
    public const string ContainerName = "SoundContainer";//오디오 소스랑 오디오 네임 담음
    public const string FadeA = "FadeA";
    public const string FadeB = "FadeB";
    public const string UI = "UI";
    public const string EffectVolumeParam = "Volume_Effect";// 볼륨 조절하기
    public const string BGMVolumeParam = "Volume_BGM";
    public const string UIVolumeParam = "Volume_UI";
    // 음악 플레이 타입 
    public enum MusicPlayingType
    {
        // 패이드인 패이드 아웃에 쓸 enum 형 변수
        None = 0,
        SourceA = 1,
        SourceB = 2,
        AtoB = 3,
        BtoA = 4
    }

    public AudioMixer mixer = null;// 오디오 믹서
    public Transform audioRoot = null;// 위치
    public AudioSource fadeA_audio = null;// 오디오 소스중 fadeA 역할
    public AudioSource fadeB_audio = null;// 오디오 소스중 fadeB 역할
    public AudioSource[] effect_audios = null;// 이팩트 오디오들의 배열 로 동시에 송출되는 오디오를 제한함
    public AudioSource UI_audio = null;// 유아이 오디오

    public float[] effect_PlayStartTime = null;// 이팩트의 시작 시간
    private int EffectChennelCount = 5;// 이팩트 체널의 게수 5개로 제한
    private MusicPlayingType currentPlayingType = MusicPlayingType.None;// 이펙트의 플레이 타입 
    private bool isTicking = false;// 
    private SoundClip currentSound = null;// 현제사운드
    private SoundClip lastSound = null;// 마지막에 재생했던사운다.
    private float minVolume = -80.0f;// 최소 볼륨
    private float maxVolume = 0.0f;// 최대 볼륨

    // 특정 지점에 이팩트 사운드 내기
    public void PlayEffectSound(SoundClip clip, Vector3 position, float volume)
    {
        bool isPlaySuccess = false;
        for (int i = 0; i < this.EffectChennelCount; i++)
        {
            if (this.effect_audios[i].isPlaying == false)
            {
                // 특정지점에서 소리를 내야하기 때문에 다른 함수 사용
                PlayAudioSourceAtPoint(clip, position, volume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
            else if (this.effect_audios[i].clip == clip.GetClip())
            {
                this.effect_audios[i].Stop();
                PlayAudioSourceAtPoint(clip, position, volume);
                this.effect_PlayStartTime[i] = Time.realtimeSinceStartup;
                isPlaySuccess = true;
                break;
            }
        }
        if (isPlaySuccess == false)
        {
            PlayAudioSourceAtPoint(clip, position, volume);
        }
    }
    // 이팩트를 한번 실행하기
    public void PlayOneShotEffect(int index, Vector3 postion, float volume)
    {
        //if (index == (int)SoundList.None)
        //{
        //    return;
        //}

        SoundClip clip = DataToolManager.SoundData().GetCopy(index);// 사운드데이타에서 크립을 복사해온다
        if (clip == null)
        {
            return;
        }
        PlayEffectSound(clip, postion, volume);// 이팩트 사운드를 얻어온 값으로 재생한다.
    }
    //특정 지점에서 제생
    void PlayAudioSourceAtPoint(SoundClip clip, Vector3 position, float volume)
    {
        // 유니티 엔진의 AudioSource 의 지정된 함수에 매게변수 값을 넣어 재생한다.
        AudioSource.PlayClipAtPoint(clip.GetClip(), position, volume);
    }
}

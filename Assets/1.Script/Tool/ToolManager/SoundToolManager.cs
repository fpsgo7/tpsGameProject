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

    private void Start()
    {
        // 위의 사운드의 변수들을 리소스를 통해 체워줄 문장들을 실행한다.
        // if 문으로 비어있는 경우에만 얻어오는 문장을 실행한다.
        //if (this.mixer == null)
        //{
        //    this.mixer = Resources.Load(MixerName) as AudioMixer;
        //}
        if (this.audioRoot == null)
        {
            audioRoot = new GameObject(ContainerName).transform;
            audioRoot.SetParent(transform);// 소리가 날대상의 위치와 자식으로 들어간다.
            audioRoot.localPosition = Vector3.zero;
        }
        if (fadeA_audio == null)
        {
            GameObject fadeA = new GameObject(FadeA, typeof(AudioSource));/// 오브젝트 생성
            fadeA.transform.SetParent(audioRoot);// 목표 대상의 자식으로 들어간다.
            this.fadeA_audio = fadeA.GetComponent<AudioSource>();// 목표대상의 오디오 소스 가져오기
            this.fadeA_audio.playOnAwake = false;// 자동 재생 종료
        }
        if (fadeB_audio == null)
        {
            // fadeA와 작업이 같다.
            GameObject fadeB = new GameObject(FadeB, typeof(AudioSource));
            fadeB.transform.SetParent(audioRoot);
            fadeB_audio = fadeB.GetComponent<AudioSource>();
            fadeB_audio.playOnAwake = false;
        }
        if (UI_audio == null)
        {
            // fadeA 와작업이 같다.
            GameObject ui = new GameObject(UI, typeof(AudioSource));
            ui.transform.SetParent(audioRoot);
            UI_audio = ui.GetComponent<AudioSource>();
            UI_audio.playOnAwake = false;
        }
        if (this.effect_audios == null || this.effect_audios.Length == 0)
        {
            this.effect_PlayStartTime = new float[EffectChennelCount];// 이팩트 체널 카운트만큼 배열길이를 정한다.
            this.effect_audios = new AudioSource[EffectChennelCount];// 마찬가지로 배열길이를 정한다.
            for (int i = 0; i < EffectChennelCount; i++)
            {
                effect_PlayStartTime[i] = 0.0f;// 시작시간은 0으로 시작
                // 게임오브젝트 생성 이름은 Effect + 몇번째 숫자 그리고 타입은 AudioSource
                GameObject effect = new GameObject("Effect" + i.ToString(), typeof(AudioSource));
                effect.transform.SetParent(audioRoot);// 부모 대상 설정
                this.effect_audios[i] = effect.GetComponent<AudioSource>();// 오디오 소스 가져오기
                this.effect_audios[i].playOnAwake = false;// 자동재생 비활성화
            }
        }
        if (this.mixer != null)
        {
            // AudioSource 에서 outputAudioMixerGroup 파트로 가서 
            // 오디오 믹서변수의 FindMatchingGroups 함수를 실행하여 적용한다.
            this.fadeA_audio.outputAudioMixerGroup = mixer.FindMatchingGroups(BGMGroupName)[0];
            this.fadeB_audio.outputAudioMixerGroup = mixer.FindMatchingGroups(BGMGroupName)[0];
            this.UI_audio.outputAudioMixerGroup = mixer.FindMatchingGroups(UIGroupName)[0];
            for (int i = 0; i < this.effect_audios.Length; i++)
            {
                this.effect_audios[i].outputAudioMixerGroup = mixer.FindMatchingGroups(EffectGroupName)[0];
            }
        }

        VolumeInit(); // 볼륨 을 초기화 하는 함수이다.
    }
    void VolumeInit()//볼륨 초기화
    {
        // 믹서가 존재할경우
        if (this.mixer != null)
        {
            //모든 키값을 통하여 초기화한다.
            this.mixer.SetFloat(BGMVolumeParam, GetBGMVolume());
            this.mixer.SetFloat(EffectVolumeParam, GetEffectVolume());
            this.mixer.SetFloat(UIVolumeParam, GetUIVolume());
        }
    }

    // 1.볼륨 을 설정하기 위한 함수들
    // bgm 볼륨 설정
    public void SetBGMVolume(float currentRatio)
    {//슬라이어다보 조절하기위해 해당 식이용
        currentRatio = Mathf.Clamp01(currentRatio);//0과 1사이에서 값을 정해줌
        float volume = Mathf.Lerp(minVolume, maxVolume, currentRatio);// 볼륨값을 넣는다.
        this.mixer.SetFloat(BGMVolumeParam, volume);// 볼륨값을 mixer에 적용한다.
        PlayerPrefs.SetFloat(BGMVolumeParam, volume);// 로컬에도 값을 저장한다.
    }
    // bgm 볼륨값 받기
    public float GetBGMVolume()
    {
        if (PlayerPrefs.HasKey(BGMVolumeParam))// 게임상에 해당 키가 있을경우
        {
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(BGMVolumeParam));//키를 통해  해당값을 가져온다.
        }
        else
        {
            return maxVolume;// 없을경우 최대값으로 시작한다.
        }
    }
    // 이팩트 볼륨 설정
    public void SetEffectVolume(float currentRatio)
    {
        currentRatio = Mathf.Clamp01(currentRatio);
        float volume = Mathf.Lerp(minVolume, maxVolume, currentRatio);
        this.mixer.SetFloat(EffectVolumeParam, volume);
        PlayerPrefs.SetFloat(EffectVolumeParam, volume);
    }
    // 이팩트 볼륨 가져오기
    public float GetEffectVolume()
    {
        if (PlayerPrefs.HasKey(EffectVolumeParam))
        {
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(EffectVolumeParam));
        }
        else
        {
            return maxVolume;
        }
    }
    // ui volume 설정
    public void SetUIVolume(float currentRatio)
    {
        currentRatio = Mathf.Clamp01(currentRatio);
        float volume = Mathf.Lerp(minVolume, maxVolume, currentRatio);
        this.mixer.SetFloat(UIVolumeParam, volume);
        PlayerPrefs.SetFloat(UIVolumeParam, volume);
    }
    public float GetUIVolume()
    {
        if (PlayerPrefs.HasKey(UIVolumeParam))
        {
            return Mathf.Lerp(minVolume, maxVolume, PlayerPrefs.GetFloat(UIVolumeParam));
        }
        else
        {
            return maxVolume;
        }
    }

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
        if (index == (int)SoundList.None)
        {
            return;
        }

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

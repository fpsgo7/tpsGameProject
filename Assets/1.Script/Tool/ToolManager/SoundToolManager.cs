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
    public const string EffectGroupName = "Effect";
    public const string ContainerName = "SoundContainer";//오디오 소스랑 오디오 네임 담음
    public const string EffectVolumeParam = "Volume_Effect";// 볼륨 조절하기

    public Transform audioRoot = null;// 위치
    public AudioSource[] effect_audios = null;// 이팩트 오디오들의 배열 로 동시에 송출되는 오디오를 제한함

    public float[] effect_PlayStartTime = null;// 이팩트의 시작 시간
    private int EffectChennelCount = 5;// 이팩트 체널의 게수 5개로 제한

    private void Start()
    {
        //위의 사운드의 변수들을 리소스를 통해 체워줄 문장들을 실행한다.
        // if 문으로 비어있는 경우에만 얻어오는 문장을 실행한다.
        if (this.audioRoot == null)
        {
            audioRoot = new GameObject(ContainerName).transform;
            audioRoot.SetParent(transform);// 소리가 날대상의 위치와 자식으로 들어간다.
            audioRoot.localPosition = Vector3.zero;
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
    //특정 지점에서 제생
    void PlayAudioSourceAtPoint(SoundClip clip, Vector3 position, float volume)
    {
        // 유니티 엔진의 AudioSource 의 지정된 함수에 매게변수 값을 넣어 재생한다.
        AudioSource.PlayClipAtPoint(clip.GetClip(), position, volume);
    }
}

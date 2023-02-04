using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using UnityObject = UnityEngine.Object;
/// <summary>
/// 사운드의 툴을 담당함
/// </summary>
public class SoundTool : EditorWindow// 에디터 형태를 사용하므로 상속필요
{
    public int uiWidthLarge = 450;// 넓은길이
    public int uiWidthMiddle = 300;// 중간 길이
    public int uiWidthSmall = 200;// 짧은 길이

    private int selection = 0;//선택값
    private Vector2 SP1 = Vector2.zero;// 백터 값
    private Vector2 SP2 = Vector2.zero;// 백터값
    private AudioClip soundSource;// 오디오 클립
    private static SoundData soundData;// 스크립트 형 변수

    [MenuItem("Tools/Sound Tool")]// 유니티 메뉴아이템 상에서 Tools 에 SoundTool 선택가능하게함
    static void Init()
    {
        // 사운드데이터 스크립터블 오브젝트 생성후 정보를 로드하고,
        // 창을 띄운다.
        soundData = CreateInstance<SoundData>();
        soundData.LoadData();
        SoundTool window = GetWindow<SoundTool>(false, "Sound Tool");
        window.Show();

    }

    private void OnGUI()
    {
        if (soundData == null)
        {
            return;
        }
        EditorGUILayout.BeginVertical();// 수직 레이아웃
        {
            UnityObject source = soundSource;// 유니티 오브젝트 형태로 박싱
            // EditorHelper 을 통해서 에디터의 탑부분 생성
            EditorHelper.EditorToolTopLayer(soundData, ref selection, ref source, uiWidthMiddle);
            // 선택값으로 선택된 배열의 사운드 클립이 
            //해당 사운트 클립 변수로 박싱되서 사용된다.
            SoundClip sound = soundData.soundClips[selection];
            soundSource = (AudioClip)source;//박싱한 대상을  언박싱

            EditorGUILayout.BeginHorizontal();// 수평 레이아웃
            {
                // 리스트 레이어를 생성
                EditorHelper.EditorToolListLayer(ref SP1, soundData, ref selection,
                    ref source, uiWidthMiddle);


                soundSource = (AudioClip)source;

                EditorGUILayout.BeginVertical();
                {
                    // 스크롤 뷰 생성
                    this.SP2 = EditorGUILayout.BeginScrollView(this.SP2);
                    {
                        if (soundData.GetDataCount() > 0)
                        {
                            EditorGUILayout.BeginVertical();// 스크롤뷰 안에서 수직으로 레이아웃함
                            {
                                EditorGUILayout.Separator();// 빈칸띄우기
                                // 값들 보여주기
                                EditorGUILayout.LabelField("ID", selection.ToString(),
                                    GUILayout.Width(uiWidthLarge));
                                // 수정되면 해당 팝업과 필드에 입력된값이 들어간다.
                                // 한줄한줄 추가되며 들어갈 내용과 내용의 값 그리고 해당
                                // 줄의 길이 값이 들어간다.
                                soundData.names[selection] = EditorGUILayout.TextField("Name",
                                    soundData.names[selection], GUILayout.Width(uiWidthLarge));
                                sound.playType = (SoundPlayType)EditorGUILayout.EnumPopup("PlayType",
                                    sound.playType, GUILayout.Width(uiWidthLarge));
                                sound.maxVolume = EditorGUILayout.FloatField("Max Volume",
                                    sound.maxVolume, GUILayout.Width(uiWidthLarge));
                                sound.isLoop = EditorGUILayout.Toggle("LoopClip",
                                    sound.isLoop, GUILayout.Width(uiWidthLarge));
                                EditorGUILayout.Separator();// 빈칸 띄우기
                                // 사운드 소스가 없고 해당 소스 이름이 있을 경우 
                                if (this.soundSource == null && sound.clipName != string.Empty)
                                {
                                    // 사운드 소스에 사운드 경로를 통해 얻은 오디오 클립을 넣어준다.
                                    this.soundSource = Resources.Load(sound.clipPath + sound.clipName)
                                        as AudioClip;
                                }
                                // 오디오 클립을 넣을 수 있는 레이아웃 생성
                                this.soundSource = (AudioClip)EditorGUILayout.ObjectField("Audio Clip",
                                    this.soundSource, typeof(AudioClip), false, GUILayout.Width(uiWidthLarge));
                                // 사운드 소스 가 있을경우 
                                if (soundSource != null)
                                {
                                    //사운드 소스 즉 오디오 클립 관련 값들을 조정할 수 있는 
                                    // 레이아웃들 작성
                                    sound.clipPath = EditorHelper.GetPath(soundSource);// 경로
                                    sound.clipName = soundSource.name;// 이름
                                    sound.pitch = EditorGUILayout.Slider("Pitch", sound.pitch, -3.0f, 3.0f,
                                        GUILayout.Width(uiWidthLarge));// 피치를 조절할 슬라이더 레이아웃
                                    sound.dopplerLevel = EditorGUILayout.Slider("Doppler",
                                        sound.dopplerLevel, 0.0f, 5.0f, GUILayout.Width(uiWidthLarge));// 도플러 레벨을 조절할 슬라이더 레이아수
                                    sound.rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup(
                                        "volume Rolloff", sound.rolloffMode, GUILayout.Width(uiWidthLarge));// 롤오프 모드 팝업창
                                    sound.minDistance = EditorGUILayout.FloatField("min Distance", sound.minDistance,
                                        GUILayout.Width(uiWidthLarge));// 최소거리 입력란
                                    sound.maxDistance = EditorGUILayout.FloatField("MaxDistance",
                                        sound.maxDistance, GUILayout.Width(uiWidthLarge));// 최대거리 입력란
                                    sound.spartialBlend = EditorGUILayout.Slider("PanLevel",
                                        sound.spartialBlend, 0.0f, 1.0f, GUILayout.Width(uiWidthLarge));// 스패셜 블랜더 슬라이더

                                }
                                else
                                {
                                    // 소스가 없는경우 해당 변수를 비운다.
                                    sound.clipName = string.Empty;
                                    sound.clipPath = string.Empty;
                                }
                                EditorGUILayout.Separator();// 한줄 띄우기
                                // 애드루프를 적용하는 버튼으로
                                // 해당 버튼을 클립하면 밑의 문장이 실행되어
                                // 해당 클립에 애드루프가 실행된다.
                                if (GUILayout.Button("Add Loop", GUILayout.Width(uiWidthMiddle)))
                                {
                                    soundData.soundClips[selection].AddLoop();
                                }
                                // 내가 선택한 사운드 클립의 채크타임 수만큼 반복한다.
                                for (int i = 0; i < soundData.soundClips[selection].checkTime.Length; i++)
                                {
                                    EditorGUILayout.BeginVertical("box");// 박스형태로 수직 레이아웃 생성
                                    {

                                        GUILayout.Label("Loop Step " + i, EditorStyles.boldLabel);// 굵은 글씨로 적음
                                        // 해당 루프를 삭제하기 위한 버튼
                                        if (GUILayout.Button("Remove", GUILayout.Width(uiWidthMiddle)))
                                        {
                                            soundData.soundClips[selection].RemoveLoop(i);
                                            return;
                                        }
                                        // 체크 타임과 셋 타임 을 설정할 수 있는 레이아웃 생성
                                        sound.checkTime[i] = EditorGUILayout.FloatField("check Time",
                                            sound.checkTime[i], GUILayout.Width(uiWidthMiddle));
                                        sound.setTime[i] = EditorGUILayout.FloatField("Set Time",
                                            sound.setTime[i], GUILayout.Width(uiWidthMiddle));
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();
        //하단
        EditorGUILayout.BeginHorizontal();
        {
            // 사운드 크립들을 다시 로드하는 버튼
            if (GUILayout.Button("Reload"))
            {
                soundData = CreateInstance<SoundData>();
                soundData.LoadData();
                selection = 0;
                this.soundSource = null;
            }
            // 내가 변경한 내용을 파링ㄹ에 적용시켜 저장하는 기능
            // 이름 중복 체크 추가하기
            if (GUILayout.Button("Save"))
            {
                if (CreateEnumStructure())
                {
                    soundData.SaveData();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
               
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    // 리스트 를 담아두는 스크립트에 내용을 수정하기
    public bool CreateEnumStructure()
    {
        if (ImpossibleDataName())
        {
            Debug.Log("저장을 취소합니다.");
            return false;
        }
        else
        {
            string enumName = "SoundList";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < soundData.names.Length; i++)
            {
                if (!soundData.names[i].ToLower().Contains("none"))
                {
                    builder.AppendLine("     " + soundData.names[i] + " = " + i.ToString() + ",");
                }
            }
            EditorHelper.CreateEnumStructure(enumName, builder);
            return true;
        }
       
    }
    public bool ImpossibleDataName()
    {
        if (NamingRules.FirstTextisNum(soundData.names[soundData.names.Length - 1]))
            return true;
        if (NamingRules.NamingBlank(soundData.names[soundData.names.Length - 1]))
            return true;
        if (!NamingRules.NumKorEng(soundData.names[soundData.names.Length - 1]))
            return true;
        if (NamingRules.ReservedWord(soundData.names[soundData.names.Length - 1]))
            return true;
        return false;
    }
}

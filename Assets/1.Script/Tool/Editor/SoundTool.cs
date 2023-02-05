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
            EditorHelper.ToolTopLayer(soundData, ref selection, uiWidthMiddle);
            // 선택값으로 선택된 배열의 사운드 클립이 
            //해당 사운트 클립 변수로 박싱되서 사용된다.
            SoundClip sound = soundData.soundClips[selection];
            soundSource = (AudioClip)source;//박싱한 대상을  언박싱

            EditorGUILayout.BeginHorizontal();// 수평 레이아웃
            {
                // 리스트 레이어를 생성
                EditorHelper.ToolListLayer(ref SP1, soundData, ref selection,
                    uiWidthMiddle);


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
                                soundData.dataNames[selection] = EditorGUILayout.TextField("Name",
                                    soundData.dataNames[selection], GUILayout.Width(uiWidthLarge));
                                sound.playType = (SoundPlayType)EditorGUILayout.EnumPopup("PlayType",
                                    sound.playType, GUILayout.Width(uiWidthLarge));
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
                                }
                                else
                                {
                                    // 소스가 없는경우 해당 변수를 비운다.
                                    sound.clipName = string.Empty;
                                    sound.clipPath = string.Empty;
                                }
                                EditorGUILayout.Separator();// 한줄 띄우기
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
            for (int i = 0; i < soundData.dataNames.Length; i++)
            {
                if (!soundData.dataNames[i].ToLower().Contains("none"))
                {
                    builder.AppendLine("     " + soundData.dataNames[i] + " = " + i.ToString() + ",");
                }
            }
            EditorHelper.CreateEnumList(enumName, builder);
            return true;
        }
       
    }
    public bool ImpossibleDataName()
    {
        string dataName = soundData.dataNames[soundData.dataNames.Length - 1];
        if (NamingRules.IsFirstTextisNum(dataName))
            return true;
        if (NamingRules.IsNamingBlank(dataName))
            return true;
        if (!NamingRules.IsOnlyNumKorEng(dataName))
            return true;
        if (NamingRules.IsReservedWord(dataName))
            return true;
        return false;
    }
}

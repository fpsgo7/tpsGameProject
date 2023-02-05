using UnityEngine;
//unityEditor  용 스크립트에 쓰이는 using 이다.
using UnityEditor;//주의 해당 using을 사용할려면 Editor 폴더 아레에 넣어야한다.
using System.Text;
using UnityObject = UnityEngine.Object;
/// <summary>
/// 이펙트 클립을 수정하고 읽고 한다.
/// </summary>
public class EffectTool : EditorWindow//EditorWindow를 상속받아 에디터 창을 만들수 있게된다.
{
    //UI 그리는대 필요한 변수들
    public int uiWidthLarge = 300;// 유아이의 겉부분 가로 
    public int uiWidthMiddle = 200;// 유아이의 안부분 가로
    private int selection = 0;// 선택값
    private Vector2 SP1 = Vector2.zero;// 스크롤에 쓸 벡터1
    private Vector2 SP2 = Vector2.zero;// 스크롤에 쓸 벡터2
    //이펙트 툴용 클립
    private GameObject effectSource = null;
    //이펙트 데이터 싱글톤을 줄이기 위해 static 활용하여 
    //다른 스크립트에서도 접글 할 수 있게함
    private static EffectData effectData;// 클래스형 변수 
    //MenuItem이라는 경로를 만들어서 
    //유니티에서 해당 툴을 열수 있게함
    [MenuItem("Tools/Effect Tool")]//해당 버튼을 입력하면 실행
    static void Init()// 이함수가 호출됨으로써 EffectTool 클래스가 생성된다.
    {
        effectData = ScriptableObject.CreateInstance<EffectData>();//스크립터블 오브젝트 생성
        effectData.LoadData(); // 데이타 불러오기
        //전체적인 툴 띄우기
        //EditorWindow의 GetWindow 함수를 사용하여 창을 가져온다.
        EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");
        window.Show();// 이후 show로 해당 클래스가 생성된다 즉 툴만생성된다.
    }
    // 위의  window.Show();로 툴을 만들었다면 OnGUI로 하나씩 체워준다.
    private void OnGUI()
    {
        //effectData 가 아직 로딩이 되지 않아서 null 일경우 
        if (effectData == null)
        {
            return;
        }
        EditorGUILayout.BeginVertical();// 수직 레이아웃을 시작한다.
        {
            // 상단, add,remove, copy
            //effectSouce 게임오브젝트를 UnityEngine.Object 형식으로 바꿔준다.
            // 이유는 밑의 EditorToolTopLayer 의 매계변수로 UnityEngine.Object형식을 사용하며
            // UnityEngine.Object 으로 여러 이펙트 사운드 등 여러형식을 포괄하여 사용할 수 있기 때문이다.
            UnityObject source = effectSource;
            //EditorHelper 에 만들어두었던 툴 상단파트를 적용하기
            //매계변수 로 EffectData의 클래스형 변수와 선택값, 소스, 유아니 길이 를 보낸다.
            EditorHelper.EditorTopLayer(effectData, ref selection, ref source, this.uiWidthMiddle);
            effectSource = (GameObject)source;//이후 source를 GameObject화 시키기

            EditorGUILayout.BeginHorizontal();// 수평 레이아웃
            {
                //중간, 데이터 목록 레이아웃 가져오기
                //매계변수 SP1 백터값, 클래스형변수, 선택값, 소스, 유아이 길이
                EditorHelper.EditorToolListLayer(ref SP1, effectData, ref selection,
                    ref source, this.uiWidthLarge);
                effectSource = (GameObject)source;

                //설정 부분
                EditorGUILayout.BeginVertical();//수직
                {
                    //설정을 위한 스크롤 뷰를 만들면서 백터값을 적용한다.
                    SP2 = EditorGUILayout.BeginScrollView(this.SP2);
                    {
                        if (effectData.GetDataCount() > 0)
                        {// 데이터가 있을경우
                            EditorGUILayout.BeginVertical();//수직 레이아웃
                            {
                                EditorGUILayout.Separator();// 구분을 위해 한줄 띄운다.
                                // ID 라는 출력이름과 선택한대상의 아이디 즉 번째수 출력
                                //이후 길이 지정한다.
                                EditorGUILayout.LabelField("ID", selection.ToString(),
                                    GUILayout.Width(uiWidthLarge));
                                // 이팩트 이름들은 이름들에 선택번째 값 에 
                                // effectData.names[selection] = 을 넣음으로써
                                // 해당 택스트필드에 세로운 정보를 입력하면 OnGUI 함수기떄문에
                                // 새로운 값이 해당 배열에 들어간다.
                                //(택스트 필드의 제목에 쓸 이름 텍스트, 이팩트데이타의 이름 , 넓이)
                                effectData.names[selection] = EditorGUILayout.TextField(
                                    "이름.", effectData.names[selection],
                                    GUILayout.Width(uiWidthLarge * 1.5f));
                                //이 팩트 타입을 위한 파트로 EnumPopup을 써 선택하면
                                //enum 값들을 선택할 수 잇는 팝업창이 뜬다.
                                //,해당 팝업창에서 enum 값을 선택하면 
                                //effectData.effectClips[selection].effectType 에 값이 들어간다.
                                effectData.effectClips[selection].effectType =
                                    (EffectType)EditorGUILayout.EnumPopup("이팩트 타입.",
                                    effectData.effectClips[selection].effectType,
                                    GUILayout.Width(uiWidthLarge));
                                // 한칸 띄운다.
                                EditorGUILayout.Separator();
                                //이팩트 소스가 아직 로드되지 않았고, 이팩트 데이타 클래스에서 해당 이팩트
                                //클립의 이펙트 이름이 있을경우 로드한다.
                                if (effectSource == null && effectData.effectClips[selection].effectName !=
                                    string.Empty)
                                {
                                    effectData.effectClips[selection].PreLoad();//해당 이팩트 클립을 PreLoad() 한다.
                                    //PreLoad를 통해 이팩트 클립에 이팩트 프리팹을 로드가되고
                                    //이팩트 소스에 위를 통해 이팩트 경로를 활용하여 게임오브젝트를 얻어와
                                    //넣어준다.
                                    effectSource = Resources.Load(
                                        effectData.effectClips[selection].effectPath +
                                        effectData.effectClips[selection].effectName) as GameObject;
                                }
                                // 위의 동작을 완료하면 이펙트 소스에 오브젝트가 체워줘서 보여줄 수 있게된다.
                                effectSource = (GameObject)EditorGUILayout.ObjectField("이팩트",
                                    this.effectSource, typeof(GameObject), false, GUILayout.Width(uiWidthLarge * 1.5f));
                                if (effectSource != null)
                                {
                                    //새롭게 이펙트를 넣을경우 이펙트를 경로를 변화시킨다.
                                    effectData.effectClips[selection].effectPath =
                                        EditorHelper.GetPath(this.effectSource);
                                    effectData.effectClips[selection].effectName = effectSource.name;
                                    //Debug.Log(effectData.effectClips[selection].effectName);
                                }
                                else
                                {
                                    //이팩트 소스가 없는경우 경로와 이름에 빈값을 넣는다.
                                    effectData.effectClips[selection].effectPath = string.Empty;
                                    effectData.effectClips[selection].effectName = string.Empty;
                                    effectSource = null;
                                }
                                EditorGUILayout.Separator();// 한칸 띄우기
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
        // End 레이아웃 으로 시작한 레이아웃을 종료한다.
        EditorGUILayout.Separator();// 하단과 구분을 위하여 한칸 띄운다.
        //하단
        EditorGUILayout.BeginHorizontal();// 가로
        {

            if (GUILayout.Button("Reload Settings"))//데이터 로드
            {
                effectData = CreateInstance<EffectData>();// 다시 생성하여
                effectData.LoadData();// 다시 불러오기
                selection = 0;// 선택값 초기화
                this.effectSource = null;//이팩트 소스 초기화
            }
            if (GUILayout.Button("Save"))//데이터 저장
            {
                if(CreateEnumStucture())// 이팩트가 추가되어 이펙트 리스트에 내용을 추가한다.
                {
                    EffectTool.effectData.SaveData();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
        }
        EditorGUILayout.EndHorizontal();// 수직
    }
    //enum 생성하기 EffectList 라는 스크립트에 접근하여 EffectList 라는 enum 에 내용을 추가한다.
    public bool CreateEnumStucture()
    {
        if (ImpossibleDataName())
        {
            Debug.Log("저장을 취소합니다.");
            return false;
        }
        else
        {
            string enumName = "EffectList";
            StringBuilder builder = new StringBuilder();// 스트링빌더 변수 생성
            for (int i = 0; i < effectData.names.Length; i++)
            {
                if (effectData.names[i] != string.Empty)
                {
                    //해당 변수에 라인을 추가하는 형식으로 추가한 enum 과 원래 enum을 넣어준다.
                    builder.AppendLine("     " + effectData.names[i] + " = " + i + ",");
                }
            }
            EditorHelper.CreateEnumList(enumName, builder);//그리고 값들을 보내어  완료시킨다.
            return true;
        }
    }
    public bool ImpossibleDataName()
    {
        string dataName= effectData.names[effectData.names.Length - 1];
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

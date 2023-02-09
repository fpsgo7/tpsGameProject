using UnityEngine;
//unityEditor  용 스크립트에 쓰이는 using 이다.
using UnityEditor;//주의 해당 using을 사용할려면 Editor 폴더 아레에 넣어야한다.
using System.Text;
using UnityObject = UnityEngine.Object;
/// <summary>
/// 오브젝트 클립을 수정하고 읽고 한다.
/// </summary>
public class ObjectTool : EditorWindow//EditorWindow를 상속받아 에디터 창을 만들수 있게된다.
{
    //UI 그리는대 필요한 변수들
    public int uiWidth300 = 300;// 
    public int uiWidth200 = 200;// 유아이의 안부분 가로
    private int selection = 0;// 선택값
    private Vector2 listScroll = Vector2.zero;// 스크롤에 쓸 벡터1
    private Vector2 selectScroll = Vector2.zero;// 스크롤에 쓸 벡터2
    private GameObject objectSource = null;//이펙트 툴용 클립
    private static ObjectData objectData;// 클래스형 변수 
    //MenuItem이라는 경로를 만들어서 
    //유니티에서 해당 툴을 열수 있게함
    [MenuItem("Tools/Object Tool")]//해당 버튼을 입력하면 실행
    static void Init()// 이함수가 호출됨으로써 EffectTool 클래스가 생성된다.
    {
        objectData = ScriptableObject.CreateInstance<ObjectData>();//스크립터블 오브젝트 생성
        objectData.LoadData(); // 데이타 불러오기
        //전체적인 툴 띄우기
        //EditorWindow의 GetWindow 함수를 사용하여 창을 가져온다.
        ObjectTool window = GetWindow<ObjectTool>(false, "Object Tool");
        window.Show();// 이후 show로 해당 클래스가 생성된다 즉 툴만생성된다.
    }
    // 위의  window.Show();로 툴을 만들었다면 OnGUI로 하나씩 체워준다.
    private void OnGUI()
    {
        //objectData 가 아직 로딩이 되지 않아서 null 일경우 
        if (objectData == null)
        {
            return;
        }
        EditorGUILayout.BeginVertical();// 수직 레이아웃을 시작한다.
        {
            UnityObject source = objectSource;
            objectSource = (GameObject)source;//게임오브젝트로 해서 가져오기
            //EditorHelper 에 만들어두었던 툴 상단파트를 적용하기
            //매계변수 로 ObjectData의 클래스형 변수와 선택값, 소스, 유아니 길이 를 보낸다.
            EditorHelper.ToolTopLayer(objectData, ref selection, ref source, this.uiWidth200);
            EditorGUILayout.BeginHorizontal();// 수평 레이아웃
            {
                //중간, 데이터 목록 레이아웃 가져오기
                //매계변수 SP1 백터값, 클래스형변수, 선택값, 소스, 유아이 길이
                EditorHelper.ToolListLayer(ref listScroll, objectData, ref selection,
                    ref source, this.uiWidth300);
                objectSource = (GameObject)source;
                //설정 부분
                EditorGUILayout.BeginVertical();//수직
                {
                    //설정을 위한 스크롤 뷰를 만들면서 백터값을 적용한다.
                    selectScroll = EditorGUILayout.BeginScrollView(this.selectScroll);
                    {
                        if (objectData.GetDataCount() > 0)
                        {// 데이터가 있을경우
                            EditorGUILayout.BeginVertical();//수직 레이아웃
                            {
                                EditorGUILayout.Separator();// 구분을 위해 한줄 띄운다.
                                // ID 라는 출력이름과 선택한대상의 아이디 즉 번째수 출력
                                //이후 길이 지정한다.
                                EditorGUILayout.LabelField("ID", selection.ToString(),
                                    GUILayout.Width(uiWidth300));

                                // 이팩트 이름들은 이름들에 선택번째 값 에 
                                // effectData.names[selection] = 을 넣음으로써
                                // 해당 택스트필드에 세로운 정보를 입력하면 OnGUI 함수기떄문에
                                // 새로운 값이 해당 배열에 들어간다.
                                //(택스트 필드의 제목에 쓸 이름 텍스트, 이팩트데이타의 이름 , 넓이)
                                objectData.dataNames[selection] = EditorGUILayout.TextField(
                                    "이름.", objectData.dataNames[selection],
                                    GUILayout.Width(uiWidth300 * 1.5f));

                                //이 팩트 타입을 위한 파트로 EnumPopup을 써 선택하면
                                //enum 값들을 선택할 수 잇는 팝업창이 뜬다.
                                //,해당 팝업창에서 enum 값을 선택하면 
                                //objectData.objectClips[selection].objectType 에 값이 들어간다.
                                objectData.objectClips[selection].objectType =
                                    (ObjectType)EditorGUILayout.EnumPopup("오브젝트 타입.",
                                    objectData.objectClips[selection].objectType,
                                    GUILayout.Width(uiWidth300));

                                // 한칸 띄운다.
                                EditorGUILayout.Separator();
                                //오브젝트 소스가 아직 로드되지 않았고, 오브젝트 데이타 클래스에서 해당 오브젝트
                                //클립의 오브젝트 이름이 있을경우 로드한다.
                                if (objectSource == null && objectData.objectClips[selection].objectName !=
                                    string.Empty)
                                {
                                    objectData.objectClips[selection].PreLoad();//해당 오브젝트 클립을 PreLoad() 한다.
                                    //PreLoad를 통해 오브젝트 클립에 오브젝트 프리팹을 로드가되고
                                    //오브젝트 소스에 위를 통해 오브젝트 경로를 활용하여 게임오브젝트를 얻어와
                                    //넣어준다.
                                    objectSource = Resources.Load(
                                        objectData.objectClips[selection].objectPath +
                                        objectData.objectClips[selection].objectName) as GameObject;
                                }
                                // 위의 동작을 완료하면 오브젝트 소스에 오브젝트가 체워줘서 보여줄 수 있게된다.
                                objectSource = (GameObject)EditorGUILayout.ObjectField("오브젝트",
                                    this.objectSource, typeof(GameObject), false, GUILayout.Width(uiWidth300 * 1.5f));
                                if (objectSource != null)
                                {
                                    //새롭게 오브젝트를 넣을경우 오브젝트 경로를 변화시킨다.
                                    objectData.objectClips[selection].objectPath =
                                        EditorHelper.GetPath(this.objectSource);
                                    objectData.objectClips[selection].objectName = objectSource.name;
                                    //Debug.Log(effectData.effectClips[selection].effectName);
                                }
                                else
                                {
                                    //오브젝트 소스가 없는경우 경로와 이름에 빈값을 넣는다.
                                    objectData.objectClips[selection].objectPath = string.Empty;
                                    objectData.objectClips[selection].objectName = string.Empty;
                                    objectSource = null;
                                }

                                // 한칸 띄우기
                                EditorGUILayout.Separator();
                                // 오브젝트 풀링에 필요한 오브젝트 수 미리지정하기위한 필드
                                objectData.objectClips[selection].objectnecessary = EditorGUILayout.IntField("필요오브젝트 수",
                                        objectData.objectClips[selection].objectnecessary, GUILayout.Width(uiWidth300 * 1.5f));
                                objectData.objectClips[selection].objectTime = EditorGUILayout.IntField("풀링 시간",
                                      objectData.objectClips[selection].objectTime, GUILayout.Width(uiWidth300 * 1.5f));
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

            if (GUILayout.Button("Reload"))//데이터 로드
            {
                objectData = CreateInstance<ObjectData>();// 다시 생성하여
                objectData.LoadData();// 다시 불러오기
                selection = 0;// 선택값 초기화
                this.objectSource = null;//오브젝트 소스 초기화
            }
            if (GUILayout.Button("Save"))//데이터 저장
            {
                if(CreateEnumStucture())// 오브젝트가 추가되어 오브젝트 리스트에 내용을 추가한다.
                {
                    ObjectTool.objectData.SaveData();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
        }
        EditorGUILayout.EndHorizontal();// 수직
    }
    //enum 생성하기 ObjectList 라는 스크립트에 접근하여 ObjectList 라는 enum 에 내용을 추가한다.
    public bool CreateEnumStucture()
    {
        if (ImpossibleDataName())// 저장할 이름이 사용불가능할 경우
        {
            Debug.Log("저장을 취소합니다.");
            return false;
        }
        else
        {
            string enumName = "ObjectList";
            StringBuilder builder = new StringBuilder();// 스트링빌더 변수 생성
            for (int i = 0; i < objectData.dataNames.Length; i++)
            {
                if (objectData.dataNames[i] != string.Empty)
                {
                    //해당 변수에 라인을 추가하는 형식으로 추가한 enum 과 원래 enum을 넣어준다.
                    builder.AppendLine("     " + objectData.dataNames[i] + " = " + i + ",");
                }
            }
            EditorHelper.CreateEnumList(enumName, builder);//그리고 값들을 보내어  완료시킨다.
            return true;
        }
    }
    public bool ImpossibleDataName()
    {
        string dataName= objectData.dataNames[objectData.dataNames.Length - 1];
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

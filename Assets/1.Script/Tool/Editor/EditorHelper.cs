using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityObject = UnityEngine.Object;//
/// <summary>
/// 유니티 툴 에디터의 기능들을 돕기위해 사용되는 
/// 스크립트이다.
/// </summary>
public class EditorHelper
{

	/// <summary>
	/// 경로 계산 함수로 경로를 구해준다.
	/// </summary>
	/// <param name="p_clip"></param>
	/// <returns></returns>
	public static string GetPath(UnityEngine.Object p_clip)
	{
		string retString = string.Empty;
		// p_clip의 애샛 경로를 구한다.
		retString = AssetDatabase.GetAssetPath(p_clip);
		// /을 기준으로 분리하여 배열에 저장한다 
		string[] path_node = retString.Split('/'); //Assets/9.ResourcesData/Resources/Sound/BGM.wav
		bool findResource = false;
		// 반복문 길이에 -1을 해줘 끝부분의 애샛 이름을 제외하여 경로만 가져오게한다.
		for (int i = 0; i < path_node.Length - 1; i++)
		{
			if (findResource == false)
			{
				if (path_node[i] == "Resources")
				{
					findResource = true;
					retString = string.Empty;
				}
			}
			else
			{
				retString += path_node[i] + "/";
			}

		}

		return retString;
	}

	/// <summary>
	/// Data 리스트를 enum structure로 뽑아주는 함수.숫자가아닌 이름으로 검색한다.
	/// enum 생성하기
	/// 숫자가아닌 리스트에 enum 형으로 써서 중간에 껴서 순서가 엉망이 되는 것을 
	/// 방지한다.
	/// </summary>
	public static void CreateEnumStructure(string enumName, StringBuilder data)
	{
		// 해당 택스트 글자를 토대로  툴이사용할 enum  리스트 스크립트를 생성한다.
		string templateFilePath = "Assets/14.DataResources/Data/EnumTemplate.txt";
		string entittyTemplate = File.ReadAllText(templateFilePath);
		// Replace 함수를 사용하여 entittyTemplate의 검색문자로 검색된
		// 문자를 찾아 치환 문자로 교체한다. 
		// Replace( 검색 문자 , 치환문자);
		entittyTemplate = entittyTemplate.Replace("$DATA$", data.ToString());
		entittyTemplate = entittyTemplate.Replace("$ENUM$", enumName);
		string folderPath = "Assets/1.Scripts/Tool/DataPart/";// 위의 내용을 통해만든 스크립트 저장위치
		if (Directory.Exists(folderPath) == false)
		{
			Directory.CreateDirectory(folderPath);
		}

		string FilePath = folderPath + enumName + ".cs";
		if (File.Exists(FilePath))
		{
			File.Delete(FilePath);
		}
		File.WriteAllText(FilePath, entittyTemplate);
	}
	//에디터의 탑 레이어를 다룸 그래서 자동으로 추가할때마다 레이어에 배치하게 도와준다.
	// (데이터 변수, ref int 선택값, ref UnityEngine.Object 유니티 의 오브젝트, int 유아이 가로 길이 )
	public static void EditorToolTopLayer(BaseData data, ref int selection,
		ref UnityObject source, int uiWidth)
	{
		// Horizontal 로 시작한다.
		EditorGUILayout.BeginHorizontal();//Horizontal 이기때문에 (수평)가로로 된다.
		{//구분을 위하여 괄호를 활용한다.
			//ADD 버튼이 눌리면 data(BaseData 또는 그 자식 클래스)의 AddData 함수가 실행된다.
			if (GUILayout.Button("ADD", GUILayout.Width(uiWidth)))
			{
				data.AddData("New Data");
				selection = data.GetDataCount() - 1;// 최종 리스트를 선택
				source = null;
			}
			//COPY 버튼이 눌리면 선택된 대상을 복사한다.
			if (GUILayout.Button("Copy", GUILayout.Width(uiWidth)))
			{//selection은 선택한 대상을 찾기위한 번호값이다.
				data.Copy(selection);
				source = null;//해당 변수를 초기화
				selection = data.GetDataCount() - 1;//작업이 완료 되면최종 리스트를 선택하게한다.
			}
			// 삭제버튼
			if (data.GetDataCount() > 1)//데이터 개수가 2개이상일경우 삭제 버튼 활성화
			{
				if (GUILayout.Button("Remove", GUILayout.Width(uiWidth)))
				{
					source = null;
					data.RemoveData(selection);
				}
			}
			if (selection > data.GetDataCount() - 1)//만약 선택값이 최종값보다 큰경우
			{
				selection = data.GetDataCount() - 1;// 최종값을 선택하게함
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	//레이아웃의 리스트 부분으로  수직 형태로 한다.
	// 스크롤을 사용하기위해 스크롤 포지션 위치값을 위한 변수 생성한다.
	public static void EditorToolListLayer(ref Vector2 ScrollPosion, BaseData data,
		ref int selection, ref UnityObject source, int uiWidth)
	{
		//Vertical 형태로 레이아웃 시작
		EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));// 수직 형태로 적용
		{
			EditorGUILayout.Separator();//레이아웃에서 한칸 띄우기
			EditorGUILayout.BeginVertical("box");//수직안에 수직 하나더 "box" 스타일로생성
			{
				//스크롤 뷰를 생성함과 매계변수로 스크롤 뷰의 위치값을 지정해준다.
				ScrollPosion = EditorGUILayout.BeginScrollView(ScrollPosion);
				{
					if (data.GetDataCount() > 0)//데이터가 존재한다면
					{
						//선택을 바꿨는지 확인하기 위해 사용
						int lastSelection = selection;//마지막으로 선택한값을 넣는다.
													  //한줄짜리 그리드를 만들 것이다.
													  //(선택값, 데이터리스트, 개수)
						selection = GUILayout.SelectionGrid(selection,
							data.GetNameList(true), 1);
						if (lastSelection != selection)//선택이 봐꼇을경우
						{
							source = null;
						}
					}
				}
				EditorGUILayout.EndScrollView();
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();//레이아웃 종료
	}
}

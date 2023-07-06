using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class CoroutineTrackerWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<CoroutineInfo> runningCoroutines;

    private bool coroutinesFound = false;
    private bool refreshButtonClicked = false;
    private class CoroutineInfo
    {
        public string scriptName;
        public string methodName;
        public MonoBehaviour scriptInstance;
        public int lineNumber;
    }

    [MenuItem("Window/Coroutine Viewer")]
    public static void ShowWindow()
    {
        GetWindow<CoroutineTrackerWindow>("Coroutine Viewer");
    }

    private void OnEnable()
    {
        runningCoroutines = new List<CoroutineInfo>();
        EditorApplication.update += UpdateRunningCoroutines;

    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateRunningCoroutines;
    }
    /// <summary>
    /// 에디터 창에서 실행되는 GUI를 처리하는 메서드입니다.
    /// 실행 중인 코루틴 목록을 스크롤 뷰로 표시하고, 각 코루틴을 선택하면 해당 스크립트를 열고 코루틴이 위치한 라인으로 이동합니다
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("Running Coroutines", EditorStyles.boldLabel);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        if (GUILayout.Button("Refresh"))
        {
            refreshButtonClicked = true;
            coroutinesFound = false;
        }

        foreach (CoroutineInfo coroutineInfo in runningCoroutines)
        {
            string coroutineName = $"{coroutineInfo.scriptName} - {coroutineInfo.methodName}";

            if (GUILayout.Button(coroutineName))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = coroutineInfo.scriptInstance;

                string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(coroutineInfo.scriptInstance));
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(scriptPath, coroutineInfo.lineNumber);
            }
        }

        GUILayout.EndScrollView();
    }
    /// <summary>
    /// 실행 중인 코루틴을 업데이트하여 runningCoroutines 리스트를 갱신합니다.
    /// </summary>
    private void UpdateRunningCoroutines()
    {
        if (!coroutinesFound || refreshButtonClicked)
        {
            runningCoroutines.Clear();

            MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == "StartCoroutine")
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerator))
                        {
                            bool isObsolete = false;
                            object[] attributes = method.GetCustomAttributes(true);
                            foreach (object attribute in attributes)
                            {
                                if (attribute is System.ObsoleteAttribute)
                                {
                                    isObsolete = true;
                                    break;
                                }
                            }

                            if (!isObsolete)
                            {
                                string scriptName = monoBehaviour.GetType().Name;
                                string[] lines = File.ReadAllLines(GetScriptFilePath(monoBehaviour));
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (lines[i].Contains(method.Name))
                                    {
                                        string coroutineName = FindCoroutineName(lines[i]);
                                        if (!string.IsNullOrEmpty(coroutineName))
                                        {
                                            CoroutineInfo coroutineInfo = new CoroutineInfo();
                                            coroutineInfo.scriptName = scriptName;
                                            coroutineInfo.methodName = coroutineName;
                                            coroutineInfo.scriptInstance = monoBehaviour;
                                            coroutineInfo.lineNumber = i + 1;
                                            runningCoroutines.Add(coroutineInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            coroutinesFound = true;
            refreshButtonClicked = false;
        }
    }


    /// <summary>
    /// 주어진 코드 라인에서 코루틴 이름을 찾습니다.
    /// </summary>
    /// <param name="line">분석할 코드 라인</param>
    /// <returns>찾은 코루틴 이름</returns>
    private string FindCoroutineName(string line)
    {
        int startIndex = line.IndexOf("(");
        int endIndex = line.IndexOf(")");
        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        {
            string coroutineName = line.Substring(startIndex + 1, endIndex - startIndex).Trim();

            
            if (line.Contains("//")) //주석 처리된 부분인지 확인
            {
                //주석 처리된 부분은 처리하지 않고 null 반환
                return null;
            }

            return coroutineName; //코루틴 이름 반환
        }
        return null; //이름 못찾는다면 null 반환
    }


    /// <summary>
    /// 주어진 MonoBehaviour의 스크립트 파일 경로를 가져옵니다.
    /// </summary>
    /// <param name="monoBehaviour">대상 MonoBehaviour</param>
    /// <returns>스크립트 파일 경로</returns>
    private string GetScriptFilePath(MonoBehaviour monoBehaviour)
    {
        MonoScript monoScript = MonoScript.FromMonoBehaviour(monoBehaviour); //MonoBehaviour에 연결된 MonoScript 인스턴스를 가져오기
        string scriptFilePath = AssetDatabase.GetAssetPath(monoScript); //MonoScript에 대한 스크립트 파일 경로를 가져오기
        return scriptFilePath; //스크립트 파일경로 반환하기
    }
}
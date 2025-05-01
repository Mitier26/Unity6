using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(HintSystem))]
public class HintSystemEditor : Editor
{
    private HintData hintData;
    private string hintDataPath;
    private Vector2 scrollPosition;
    private bool showHints = true;
    
    private void OnEnable()
    {
        LoadHintData();
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Hint System Editor", EditorStyles.boldLabel);
        
        if (hintData == null)
        {
            if (GUILayout.Button("Create Hint Data"))
            {
                CreateDefaultHintData();
            }
            return;
        }
        
        // 기본 힌트 텍스트 편집
        EditorGUILayout.LabelField("Default Hint Text (When all dialogues are played):");
        hintData.defaultHint = EditorGUILayout.TextArea(hintData.defaultHint, GUILayout.Height(60));
        
        EditorGUILayout.Space(5);
        
        // 힌트 목록 표시 토글
        showHints = EditorGUILayout.Foldout(showHints, "Dialogue Hints", true);
        
        if (showHints)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            for (int i = 0; i < hintData.hints.Count; i++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                // 힌트 헤더
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Hint {i + 1}: {hintData.hints[i].dialogueName}", EditorStyles.boldLabel);
                
                // 힌트 삭제 버튼
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    hintData.hints.RemoveAt(i);
                    SaveHintData();
                    break;
                }
                EditorGUILayout.EndHorizontal();
                
                // 대화 이름
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Dialogue Name:", GUILayout.Width(120));
                hintData.hints[i].dialogueName = EditorGUILayout.TextField(hintData.hints[i].dialogueName);
                EditorGUILayout.EndHorizontal();
                
                // 우선순위
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Priority:", GUILayout.Width(120));
                hintData.hints[i].priority = EditorGUILayout.IntField(hintData.hints[i].priority);
                EditorGUILayout.EndHorizontal();
                
                // 힌트 텍스트
                EditorGUILayout.LabelField("Hint Text:");
                hintData.hints[i].hintText = EditorGUILayout.TextArea(hintData.hints[i].hintText, GUILayout.Height(60));
                
                // 재생 여부
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Was Played:", GUILayout.Width(120));
                hintData.hints[i].wasPlayed = EditorGUILayout.Toggle(hintData.hints[i].wasPlayed);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.Space(10);
        
        // 새 힌트 추가 버튼
        if (GUILayout.Button("Add New Hint"))
        {
            AddNewHint();
        }
        
        EditorGUILayout.Space(5);
        
        // 저장 버튼
        if (GUILayout.Button("Save Changes"))
        {
            SaveHintData();
            EditorUtility.DisplayDialog("Hint System", "Hint data saved successfully!", "OK");
        }
        
        // 리셋 버튼
        if (GUILayout.Button("Reset All Played Status"))
        {
            ResetAllPlayedStatus();
        }
    }
    
    private void LoadHintData()
    {
        hintDataPath = Path.Combine(Application.streamingAssetsPath, "HintData.json");
        
        try
        {
            if (File.Exists(hintDataPath))
            {
                string json = File.ReadAllText(hintDataPath);
                hintData = JsonUtility.FromJson<HintData>(json);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load hint data: " + e.Message);
        }
    }
    
    private void CreateDefaultHintData()
    {
        hintData = new HintData
        {
            defaultHint = "더 힌트가 없습니다. 지금까지의 단서를 다시 확인해보세요.",
            hints = new List<DialogueHint>()
        };
        
        SaveHintData();
    }
    
    private void SaveHintData()
    {
        try
        {
            // 디렉토리가 없으면 생성
            string directory = Path.GetDirectoryName(hintDataPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            string json = JsonUtility.ToJson(hintData, true);
            File.WriteAllText(hintDataPath, json);
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save hint data: " + e.Message);
            EditorUtility.DisplayDialog("Error", "Failed to save hint data: " + e.Message, "OK");
        }
    }
    
    private void AddNewHint()
    {
        hintData.hints.Add(new DialogueHint
        {
            dialogueName = "New_Dialogue",
            priority = hintData.hints.Count + 1,
            hintText = "힌트 텍스트를 입력하세요.",
            wasPlayed = false
        });
        
        SaveHintData();
    }
    
    private void ResetAllPlayedStatus()
    {
        for (int i = 0; i < hintData.hints.Count; i++)
        {
            hintData.hints[i].wasPlayed = false;
        }
        
        SaveHintData();
        EditorUtility.DisplayDialog("Hint System", "All dialogue played status has been reset.", "OK");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using PixelCrushers.DialogueSystem;
using System;
using System.Linq;

[System.Serializable]
public class HintData
{
    public string defaultHint;
    public List<DialogueHint> hints;
}

[System.Serializable]
public class DialogueHint
{
    public string dialogueName;
    public int priority;
    public string hintText;
    public bool wasPlayed;
}

public class HintSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private float idleTimeForHint = 5f;
    
    private HintData hintData;
    private float idleTimer = 0f;
    private bool isHintShowing = false;
    private string hintDataPath;
    
    void Start()
    {
        hintDataPath = Path.Combine(Application.streamingAssetsPath, "HintData.json");
        LoadHintData();
        
        // 대화 시스템에 이벤트 리스너 등록
        DialogueManager.instance.conversationStarted += OnConversationStarted;
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }
    
    void Update()
    {
        // 플레이어 입력이 있으면 타이머 리셋
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            ResetIdleTimer();
            HideHint();
        }
        else
        {
            // 입력이 없으면 타이머 증가
            idleTimer += Time.deltaTime;
            
            // 일정 시간 이상 지나면 힌트 표시
            if (idleTimer >= idleTimeForHint && !isHintShowing)
            {
                ShowHint();
            }
        }
    }
    
    private void LoadHintData()
    {
        try
        {
            if (File.Exists(hintDataPath))
            {
                string json = File.ReadAllText(hintDataPath);
                hintData = JsonUtility.FromJson<HintData>(json);
            }
            else
            {
                Debug.LogWarning("HintData.json file not found. Creating default hint data.");
                CreateDefaultHintData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load hint data: " + e.Message);
            CreateDefaultHintData();
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
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save hint data: " + e.Message);
        }
    }
    
    private void OnConversationStarted(Transform actor)
    {
        // 현재 시작된 대화의 제목 가져오기
        string currentDialogue = DialogueManager.lastConversationStarted;
        
        // 해당 대화에 대한 힌트가 있는지 확인하고, 있다면 wasPlayed를 true로 설정
        DialogueHint hint = hintData.hints.Find(h => h.dialogueName == currentDialogue);
        if (hint != null)
        {
            hint.wasPlayed = true;
            SaveHintData();
        }
    }
    
    private void OnConversationEnded(Transform actor)
    {
        // 대화가 끝나면 타이머 리셋
        ResetIdleTimer();
    }
    
    private void ShowHint()
    {
        if (hintText != null)
        {
            // 재생되지 않은 힌트 중 우선순위가 가장 높은 힌트 찾기
            var unplayedHints = hintData.hints.Where(h => !h.wasPlayed).ToList();
            
            if (unplayedHints.Count > 0)
            {
                // 이름 순으로 정렬된 우선순위에 따라 힌트 선택
                var sortedHints = unplayedHints.OrderBy(h => h.priority).ToList();
                DialogueHint nextHint = sortedHints[0];
                
                hintText.text = nextHint.hintText;
            }
            else
            {
                // 모든 힌트가 재생되었다면 기본 힌트 표시
                hintText.text = hintData.defaultHint;
            }
            
            hintText.gameObject.SetActive(true);
            isHintShowing = true;
        }
    }
    
    private void HideHint()
    {
        if (hintText != null && isHintShowing)
        {
            hintText.gameObject.SetActive(false);
            isHintShowing = false;
        }
    }
    
    private void ResetIdleTimer()
    {
        idleTimer = 0f;
    }
    
    private void OnDestroy()
    {
        // 이벤트 리스너 제거
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStarted;
            DialogueManager.instance.conversationEnded -= OnConversationEnded;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    public PuzzleFlowSO puzzleFlow;                         // 퍼즐 흐름 데이터
    private int currentStepIndex = 0;

    [Header("UI")]
    public GameObject bottomDialoguePanel;                  // 대화 UI
    public TextMeshProUGUI bottomDialogueText;              // 하단 대사 텍스트

    public GameObject leftTopMessagePanel;                  // 왼쪽 상단 메시지 UI
    public TextMeshProUGUI leftTopMessageText;              // 왼쪽 상단 메시지 텍스트

    public GameObject nextDialogueUI;
    
    private bool isWaitingForNextDialogue = false;

    public GameObject crosshair;

    [Header("연출용")]
    public PlayableDirector director;

    public PuzzleStepSO CurrentStep => puzzleFlow.steps[currentStepIndex];

    private bool isCutsceneActive = false;
    public bool IsCutsceneActive => isCutsceneActive;

    [Header("초기 상태")]
    public bool startWithCutscene = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (startWithCutscene)
        {
            BeginCutscene();
        }
    }

    public void AdvanceStep()
    {
        if (currentStepIndex < puzzleFlow.steps.Count - 1)
        {
            currentStepIndex++;
            TriggerStep(CurrentStep);
        }
        else
        {
            Debug.Log("퍼즐 완료!");
        }
    }

    private void TriggerStep(PuzzleStepSO step)
    {
        Debug.Log("현재 스텝: " + step.stepName);

        // 하단 대사 출력 (복수 지원)
        // if (step.bottomDialogues != null && step.bottomDialogues.Count > 0)
        //     ShowBottomDialogues();

        // 상단 메시지 출력
        if (!string.IsNullOrEmpty(step.topLeftMessageText))
            ShowLeftTopMessage(step.topLeftMessageText);

        // 타임라인 연출
        if (step.timeline != null && director != null)
        {
            BeginCutscene();
            director.Play(step.timeline);

            if (!step.waitForTimelineEnd)
            {
                StartCoroutine(AutoAdvanceAfter(step.autoAdvanceDelay));
            }
        }
        else if (step.triggerType == StepTriggerType.Auto)
        {
            StartCoroutine(AutoAdvanceAfter(step.autoAdvanceDelay));
        }
    }


    public void BeginCutscene() => SetCutsceneState(true);
    public void EndCutscene() => SetCutsceneState(false);

    public void SetCutsceneState(bool active)
    {
        isCutsceneActive = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
        crosshair.SetActive(!active);
    }

    public void ShowBottomDialogues()
    {
        StartCoroutine(ShowDialogueSequence(CurrentStep.bottomDialogues));
    }

    private IEnumerator ShowDialogueSequence(List<string> messages)
    {
        bottomDialoguePanel.SetActive(true);
        foreach (var message in messages)
        {
            bottomDialogueText.text = string.Empty;
            nextDialogueUI.SetActive(true);
            yield return StartCoroutine(TypingText(bottomDialogueText, message));
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            nextDialogueUI.SetActive(false);
        }
        bottomDialoguePanel.SetActive(false);
        ResumeTimelineAfterDialogue();
    }

    public void ShowLeftTopMessage(string message)
    {
        leftTopMessagePanel.SetActive(true);
        leftTopMessageText.text = message;
    }

    public void HideBottomDialogue()
    {
        bottomDialoguePanel.SetActive(false);
        bottomDialogueText.text = string.Empty;
    }

    IEnumerator TypingText(TextMeshProUGUI text, string message, float delay = 0.05f)
    {
        text.text = string.Empty;
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator AutoAdvanceAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndCutscene(); 
        AdvanceStep();
    }

    public void OnTimelineEnded()
    {
        EndCutscene();
        AdvanceStep();
    }
    
    private void ResumeTimelineAfterDialogue()
    {
        if (director != null)
        {
            director.Play();
            isWaitingForNextDialogue = false;
            nextDialogueUI.SetActive(false);
        }
    }

    
    public void PauseTimelineForDialogue()
    {
        if (director != null)
        {
            director.Pause();
            isWaitingForNextDialogue = true;
            nextDialogueUI.SetActive(true);
            
            ShowBottomDialogues();
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PanelManager Instance { get; private set; }
    
    // 현재 활성화된 패널 추적
    private GameObject activePanel;
    
    // 모든 패널 토글 컨트롤러 목록 관리
    private List<PanelToggleController> registeredControllers = new List<PanelToggleController>();

    // ESC 키로 열리는 패널 (주로 옵션 패널)
    private PanelToggleController escActivatedController;
    
    // 플레이어 동작을 막을 변수
    private bool isUiOpened = false;
    public bool IsUiOpened => isUiOpened;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // ESC 키 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 열린 패널이 있으면 닫기
            if (activePanel != null)
            {
                foreach (var controller in registeredControllers)
                {
                    if (controller.targetPanel == activePanel && controller.useEscToClose)
                    {
                        controller.ClosePanel();
                        return;
                    }
                }
            }
            // 열린 패널이 없으면 ESC로 활성화되는 패널 열기
            else if (escActivatedController != null)
            {
                escActivatedController.OpenPanel();
            }
        }

        // 각 컨트롤러의 활성화 키 처리
        foreach (var controller in registeredControllers)
        {
            if (Input.GetKeyDown(controller.activationKey))
            {
                // 이미 해당 패널이 활성화되어 있으면 닫기
                if (controller.targetPanel == activePanel)
                {
                    controller.ClosePanel();
                }
                // 다른 패널이 활성화되어 있으면 무시
                else if (activePanel != null)
                {
                    // 다른 패널이 열려있을 때는 아무 동작 없음
                }
                // 활성화된 패널이 없으면 새 패널 열기
                else
                {
                    controller.OpenPanel();
                }
            }
        }
    }

    // 컨트롤러 등록
    public void RegisterController(PanelToggleController controller)
    {
        if (!registeredControllers.Contains(controller))
        {
            registeredControllers.Add(controller);
            
            // ESC로 활성화되는 패널 등록
            if (controller.isEscActivatedPanel)
            {
                escActivatedController = controller;
            }
        }
    }

    // 컨트롤러 등록 해제
    public void UnregisterController(PanelToggleController controller)
    {
        if (registeredControllers.Contains(controller))
        {
            registeredControllers.Remove(controller);
            
            // ESC로 활성화되는 패널 등록 해제
            if (controller == escActivatedController)
            {
                escActivatedController = null;
            }
        }
    }

    // 패널 활성화
    public void SetActivePanel(GameObject panel, PanelToggleController controller)
    {
        activePanel = panel;
        isUiOpened = true;
    }
    
    // 활성 패널 지우기
    public void ClearActivePanel(GameObject panel)
    {
        // 현재 활성 패널과 일치하는 경우에만 지우기
        if (activePanel == panel)
        {
            activePanel = null;
            isUiOpened = false;
        }
    }

    // 현재 활성화된 패널이 있는지 확인
    public bool IsAnyPanelActive()
    {
        return activePanel != null && activePanel.activeSelf;
    }

    // 모든 패널 닫기
    public void CloseAllPanels()
    {
        foreach (var controller in registeredControllers)
        {
            if (controller.targetPanel != null && controller.targetPanel.activeSelf)
            {
                controller.targetPanel.SetActive(false);
            }
        }
        activePanel = null;
    }
}
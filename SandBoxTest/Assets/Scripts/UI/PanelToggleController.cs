using UnityEngine;
using UnityEngine.UI;

public class PanelToggleController : MonoBehaviour
{
    [Header("Panel Settings")]
    [Tooltip("패널 게임오브젝트를 여기에 넣으세요")]
    public GameObject targetPanel;

    [Header("Key Settings")]
    [Tooltip("패널을 열기 위한 키코드를 지정하세요")]
    public KeyCode activationKey = KeyCode.P;
    
    [Tooltip("ESC 키 외에 패널을 닫기 위한 추가 키 (선택사항)")]
    public KeyCode alternativeCloseKey = KeyCode.None;

    [Header("Options")]
    [Tooltip("ESC 키를 사용하여 패널을 닫을지 여부")]
    public bool useEscToClose = true;
    
    [Tooltip("이 패널이 ESC 키로 열리는 패널인지 여부 (옵션 패널 등)")]
    public bool isEscActivatedPanel = false;

    private void Start()
    {
        // 패널이 연결되어 있는지 확인
        if (targetPanel == null)
        {
            Debug.LogError("Panel is not assigned to PanelToggleController on " + gameObject.name);
            return;
        }

        // 시작 시 패널을 비활성화
        targetPanel.SetActive(false);
        
        // PanelManager가 존재하지 않으면 생성
        if (PanelManager.Instance == null)
        {
            GameObject managerObj = new GameObject("PanelManager");
            managerObj.AddComponent<PanelManager>();
        }
        
        // 패널 매니저에 이 컨트롤러 등록
        PanelManager.Instance.RegisterController(this);
    }
    
    private void OnDestroy()
    {
        // 파괴될 때 패널 매니저에서 등록 해제
        if (PanelManager.Instance != null)
        {
            PanelManager.Instance.UnregisterController(this);
        }
    }

    // 패널을 열 수 있는지 확인
    public bool CanOpenPanel()
    {
        // 다른 패널이 열려있으면 열 수 없음
        return !PanelManager.Instance.IsAnyPanelActive();
    }

    // 패널 열기
    public void OpenPanel()
    {
        if (CanOpenPanel())
        {
            targetPanel.SetActive(true);
            PanelManager.Instance.SetActivePanel(targetPanel, this);
            
        }
    }

    // 패널 닫기
    public void ClosePanel()
    {
        if (targetPanel.activeSelf)
        {
            targetPanel.SetActive(false);
            PanelManager.Instance.ClearActivePanel(targetPanel);
        }
    }

    // 패널 토글
    public void TogglePanel()
    {
        if (targetPanel.activeSelf)
        {
            ClosePanel();
        }
        else if (CanOpenPanel())
        {
            OpenPanel();
        }
    }
}
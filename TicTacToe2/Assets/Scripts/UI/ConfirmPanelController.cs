using UnityEngine;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private string message;

    /// <summary>
    /// 확인 버튼 클릭시 호출되는 함수
    /// </summary>
    public void OnClickConfirmButton()
    {
        GameManager.Instance.InitGame();
        Hide();
    }

    /// <summary>
    /// x 버튼 클릭시 호출되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}

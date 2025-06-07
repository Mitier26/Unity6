using UnityEngine;

public class SettingsPanelController : PanelController
{
    /// <summary>
    /// SFX On/Off 시 호출되는 함수
    /// </summary>
    /// <param name="value">On/Off</param>
    public void OnSFXToggleValueChanged(bool value)
    {
        
    }

    /// <summary>
    /// BGM On/Off 시 호출되는 함수
    /// </summary>
    /// <param name="value">On/Off</param>
    public void OnBGMToggleValueChanged(bool value)
    {
        
    }

    /// <summary>
    /// X 버튼 클리시 호출되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}

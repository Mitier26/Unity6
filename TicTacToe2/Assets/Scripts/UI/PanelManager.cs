using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelController startPanelController;
    [SerializeField] private PanelController confirmPanelController;
    [SerializeField] private PanelController settingsPanelController;

    public enum PanelType {StartPanel, ConfirmPanel, SettingsPanel}
    
    private PanelController _currentPanelController;

    public void ShowPanel(PanelType panelType)
    {
        switch (panelType)
        {
            case PanelType.StartPanel:
                ShowPanelController(startPanelController);
                break;
            case PanelType.ConfirmPanel:
                ShowPanelController(confirmPanelController);
                break;
            case PanelType.SettingsPanel:
                ShowPanelController(settingsPanelController);
                break;
        }
    }

    private void ShowPanelController(PanelController panelController)
    {
        if (_currentPanelController != null)
        {
            _currentPanelController.Hide();
        }
        
        panelController.Show(() =>
        {
            _currentPanelController = null;
        });
        _currentPanelController = panelController;
    }
}
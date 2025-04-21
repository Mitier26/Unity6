using TMPro;
using UnityEngine;

public class InteractionObjectUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionPanel.SetActive(false); // 시작 시 숨김!
    }

    public void Show(string message)
    {
        interactionText.text = message;
        interactionPanel.SetActive(true);
    }

    public void Hide()
    {
        interactionPanel.SetActive(false);
    }
}
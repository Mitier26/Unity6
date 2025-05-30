using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public TextMeshProUGUI mLabel;
    public Button mButton;
    public Main mMain;

    public void Fill()
    {
        mButton.interactable = false;

        mLabel.text = mMain.GetTurnCharacter();
        
        mMain.Switch();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.UI;

public class CrosshairUIController : MonoBehaviour
{
    [Header("UI References")]
    public Image crosshairImage;

    [Header("Sprites")]
    public Sprite closedEyeSprite;
    public Sprite openEyeSprite;

    public void SetState(bool isOpen)
    {
        if (isOpen && crosshairImage.sprite != openEyeSprite)
            crosshairImage.sprite = openEyeSprite;
        else if (!isOpen && crosshairImage.sprite != closedEyeSprite)
            crosshairImage.sprite = closedEyeSprite;
    }
}



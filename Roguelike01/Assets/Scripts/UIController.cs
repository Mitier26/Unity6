using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    public Slider HealthSlider;
    public TextMeshProUGUI HealthText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = currentHealth;
        
        HealthText.text = $"{currentHealth} / {maxHealth}";
    }
}

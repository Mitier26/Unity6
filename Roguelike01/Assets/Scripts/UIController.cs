using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;

    public GameObject deathScreen;

    public Image fadeImage;
    public float fadeSpeed;
    private bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu;

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

    private void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;
    }

    private void Update()
    {
        if (fadeOutBlack)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b,
                Mathf.MoveTowards(fadeImage.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeImage.color.a == 0)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b,
                Mathf.MoveTowards(fadeImage.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeImage.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnPause();
    }
}

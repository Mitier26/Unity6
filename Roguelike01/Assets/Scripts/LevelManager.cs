using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

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

    void Start()
    {
        Time.timeScale = 1f;

        UIController.instance.coinText.text = currentCoins.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayLevelWin();

        PlayerController.instance.canMove = false;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        // 다음 레벨로 이동
        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnPause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;
        
        UIController.instance.coinText.text = currentCoins.ToString();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            currentCoins = 0;
        }
        
        UIController.instance.coinText.text = currentCoins.ToString();
    }
    
}

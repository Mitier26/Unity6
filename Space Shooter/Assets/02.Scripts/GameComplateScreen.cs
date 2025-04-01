using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplateScreen : MonoBehaviour
{
    public float timeBetweenTexts;

    public bool canExit;

    public string mainmenuName = "MainMenu";

    public TextMeshProUGUI message, score, pressKey;

    private void Start()
    {
        StartCoroutine(ShowTextCo());
    }

    private void Update()
    {
        if (canExit && Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainmenuName);
        }
    }

    public IEnumerator ShowTextCo()
    {
        yield return new WaitForSeconds(timeBetweenTexts);
        message.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenTexts);
        score.text = "Final Score: " + PlayerPrefs.GetInt("CurrentScore");
        score.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenTexts);
        pressKey.gameObject.SetActive(true);
        canExit = true;
    }

}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [FormerlySerializedAs("heroMenu")] public GameObject hero_Menu;
    public TextMeshProUGUI starScoreText;

    public Image music_Img;
    public Sprite music_Off, music_On;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void HeroMenu()
    {
        hero_Menu.SetActive(true);
        starScoreText.text = "" + GameManager.instance.starScore;
    }

    public void HomeButton()
    {
        hero_Menu.SetActive(false);
    }

    public void MusicButton()
    {
        if (GameManager.instance.playSound)
        {
            music_Img.sprite = music_Off;
            GameManager.instance.playSound = false;
        }
        else
        {
            music_Img.sprite = music_On;
            GameManager.instance.playSound = true;
        }
    }

    


}

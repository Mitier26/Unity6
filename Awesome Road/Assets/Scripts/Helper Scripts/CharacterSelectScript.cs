using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScript : MonoBehaviour
{
    public GameObject[] available_Heros;

    private int currentIndex;

    public TextMeshProUGUI selectedText;
    public GameObject starIcon;
    public Image selectBtn_Image;
    public Sprite button_Green, button_Blue;

    private bool[] heroes;

    public TextMeshProUGUI starScoreText;

    private void Start()
    {
        InitializeCharacters();
    }

    void InitializeCharacters()
    {
        currentIndex = 0;
        
        for(int i = 0; i < available_Heros.Length; i++)
        {
            available_Heros[i].SetActive(false);
        }
        
        available_Heros[currentIndex].SetActive(true);
        
        
    }

    public void NextHero()
    {
        available_Heros[currentIndex].SetActive(false);

        if (currentIndex + 1 == available_Heros.Length)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        
        available_Heros[currentIndex].SetActive(true);
    }
    
    public void PreviousHero()
    {
        available_Heros[currentIndex].SetActive(false);

        if (currentIndex - 1 < 0)
        {
            currentIndex = available_Heros.Length - 1;
        }
        else
        {
            currentIndex--;
        }
        
        available_Heros[currentIndex].SetActive(true);
    }

}

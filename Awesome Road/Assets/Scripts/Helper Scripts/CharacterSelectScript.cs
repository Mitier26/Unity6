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
        currentIndex = GameManager.instance.selected_Index;
        
        for(int i = 0; i < available_Heros.Length; i++)
        {
            available_Heros[i].SetActive(false);
        }
        
        available_Heros[currentIndex].SetActive(true);
        
        heroes = GameManager.instance.heroes;
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
        
        CheckIfCharacterIsUnlocked();
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
        
        CheckIfCharacterIsUnlocked();
    }
    
    void CheckIfCharacterIsUnlocked()
    {
        if (heroes[currentIndex])
        {
            starIcon.SetActive(false);
            if (currentIndex == GameManager.instance.selected_Index)
            {
                selectBtn_Image.sprite = button_Green;
                selectedText.text = "Selected";
            }
            else
            {
                selectBtn_Image.sprite = button_Blue;
                selectedText.text = "Select?";
            }
        }
        else
        {
            selectBtn_Image.sprite = button_Blue;
            starIcon.SetActive(true);
            selectedText.text = "1000";
        }
    }

    public void SelectHero()
    {
        if (!heroes[currentIndex])
        {
            if (currentIndex != GameManager.instance.selected_Index)
            {
                if (GameManager.instance.starScore >= 1000)
                {
                    GameManager.instance.starScore -= 1000;
                
                    selectBtn_Image.sprite = button_Green;
                    selectedText.text= "Selected";
                    starIcon.SetActive(false);
                    heroes[currentIndex] = true;
                
                    starScoreText.text = "" + GameManager.instance.starScore;

                    GameManager.instance.selected_Index = currentIndex;
                    GameManager.instance.heroes = heroes;
                
                    GameManager.instance.SaveGameData();
                }
            }
            
        }
        else
        {
            selectBtn_Image.sprite = button_Green;
            selectedText.text= "Selected";
            GameManager.instance.selected_Index = currentIndex;
            GameManager.instance.SaveGameData();
        }
    }

}

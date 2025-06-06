using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameData gameData;
    
    [HideInInspector]
    public int starScore, score_Count, selected_Index;

    [HideInInspector] 
    public bool[] heroes;

    [HideInInspector] 
    public bool playSound = true;
    
    private string data_path = "GameData.dat";

    private void Awake()
    {
        MakeInstance();
        
        InitializeGameData();
    }

    private void Start()
    {
        print(Application.persistentDataPath + data_path);

    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void InitializeGameData()
    {
        LoadGameData();

        if (gameData == null)
        {
            starScore = 0;
            score_Count = 0;
            selected_Index = 0;
            heroes = new bool[9];
            heroes[0] = true;
            
            for(int i = 1; i < heroes.Length; i++)
            {
                heroes[i] = false;
            }
            
            gameData = new GameData();
            
            gameData.StarScore = starScore;
            gameData.ScoreCount = score_Count;
            gameData.Heroes = heroes;
            gameData.SelectedIndex = selected_Index;
            
            SaveGameData();
        }
    }

    public void SaveGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + data_path);

            if (gameData != null)
            {
                gameData.Heroes = heroes;
                gameData.StarScore = starScore;
                gameData.ScoreCount = score_Count;
                gameData.SelectedIndex = selected_Index;

                bf.Serialize(file, gameData);

            }
        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void LoadGameData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            file = File.Open(Application.persistentDataPath + data_path, FileMode.Open);

            gameData = (GameData)bf.Deserialize(file);

            if (gameData != null)
            {
                starScore = gameData.StarScore;
                score_Count = gameData.ScoreCount;
                heroes = gameData.Heroes;
                selected_Index = gameData.SelectedIndex;
            }

        }catch (Exception e)
        {
            
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
}

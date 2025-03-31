using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    public AudioSource levelMusic, boosMusic, victoryMusic, gameOverMusic;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        levelMusic.Play();
    }

    private void StopMusic()
    {
        levelMusic.Stop();
        boosMusic.Stop();
        victoryMusic.Stop();
        gameOverMusic.Stop();
    }

    public void PlayBoos()
    {
        StopMusic();
        boosMusic.Play();
    }

    public void PlayVictory()
    {
        StopMusic();
        victoryMusic.Play();
    }

    public void PlayGameOver()
    {
        StopMusic();
        gameOverMusic.Play();
    }
}

using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource move_Audio_Source, jump_Audio_Source, powerUp_Die_Audio_Source, background_Audio_Source;

    public AudioClip power_Up_Clip, die_Clip, coin_Clip, game_Over_Clip; 
    
    
    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        if(GameManager.instance.playSound)
        {
            background_Audio_Source.Play();
        }
        else
        {
            background_Audio_Source.Stop();
        }
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMoveLineSound()
    {
        move_Audio_Source.Play();
    }

    public void PlayJumpSound()
    {
        jump_Audio_Source.Play();
    }

    public void PlayDeadSound()
    {
        powerUp_Die_Audio_Source.clip = die_Clip;
        powerUp_Die_Audio_Source.Play();
    }

    public void PlayPowerUpSound()
    {
        powerUp_Die_Audio_Source.clip = power_Up_Clip;
        powerUp_Die_Audio_Source.Play();
    }
    
    public void PlayCoinSound()
    {
        powerUp_Die_Audio_Source.clip = coin_Clip;
        powerUp_Die_Audio_Source.Play();
    }

    public void PlayGameOverClip()
    {
        background_Audio_Source.Stop();
        background_Audio_Source.clip = game_Over_Clip;
        background_Audio_Source.loop = false;
        background_Audio_Source.Play();
    }
    
    
}// end of class SoundManager

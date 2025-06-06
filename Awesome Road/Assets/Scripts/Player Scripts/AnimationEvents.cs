using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Animator anim;

    private string walk_Animation = "PlayerWalk";
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void PlayerWalkAnimation()
    {
        anim.Play(walk_Animation);

        if (PlayerController.instance.player_Jumped)
        {
            PlayerController.instance.player_Jumped = false;
        }
    }

    void AnimationEnded()
    {
        gameObject.SetActive(false);
    }

    void PausePanelClose()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}

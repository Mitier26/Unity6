using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Animator anim;

    private string jump_Animation = "PlayerJump";
    private string change_Line_Animation = "ChangeLine";

    public GameObject
        player,
        shadow;

    public Vector3
        firstPosOfPlayer,
        second_PosOfPlayer;

    [HideInInspector] public bool player_Died;

    [HideInInspector] public bool player_Jumped;
    void Awake()
    {
        MakeInstance();
        
        anim = player.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleChangeLine();
        HandleJump();
    }


    void MakeInstance(){
        if(instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void HandleChangeLine()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = second_PosOfPlayer;
        }else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = firstPosOfPlayer;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown((KeyCode.Space)))
        {
            if (!player_Jumped)
            {
                anim.Play(jump_Animation);
                player_Jumped = true;
                
            }
        }
    }
}

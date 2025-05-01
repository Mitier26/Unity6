using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StartingMonologue : MonoBehaviour
{
    public string conversationName = "Opening_Monologue";
    public bool playOnStart = true;
    private bool hasPlayed = false;

    void Start()
    {
        if (playOnStart)
        {
            PlayMonologue();
        }
    }

    public void PlayMonologue()
    {
        if (!hasPlayed)
        {
            DialogueManager.StartConversation(conversationName);
            hasPlayed = true;
        }
    }
}
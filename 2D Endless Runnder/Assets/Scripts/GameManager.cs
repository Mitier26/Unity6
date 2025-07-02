using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coins;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

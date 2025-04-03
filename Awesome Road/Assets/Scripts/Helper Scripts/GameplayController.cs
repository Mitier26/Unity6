using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;
    private Camera mainCam;

    public float moveSpeed, distance_Factor = 1f;
    private float distance_Move;
    private bool gameJustStarted;
    
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        mainCam = Camera.main;
        gameJustStarted = true;
    }
    
    void Update()
    {
        MoveCamera();
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void MoveCamera()
    {
        if (gameJustStarted)
        {
            if (!PlayerController.instance.player_Died)
            {
                if (moveSpeed < 12.0f)
                {
                    moveSpeed += Time.deltaTime * 5.0f;
                }
                else
                {
                    moveSpeed = 12f;
                    gameJustStarted = false;
                }
            }
        }

        if (!PlayerController.instance.player_Died)
        {
            mainCam.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            UpdateDistance();
        }
    }

    void UpdateDistance()
    {
        distance_Move += Time.deltaTime * distance_Factor;
        float round = Mathf.Round(distance_Move);
        
        
        
        if(round >= 30.0f && round < 60.0f)
        {
            moveSpeed = 14f;
        }
        else if (round >= 60f)
        {
            moveSpeed = 16f;
        }
    }
}

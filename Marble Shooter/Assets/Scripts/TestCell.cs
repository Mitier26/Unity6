using UnityEngine;

public class CellController : MonoBehaviour
{
    public Material player1Material;
    public Material player2Material;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void HitByBullet(int bulletLayer)
    {
        if (bulletLayer == LayerMask.NameToLayer("Player1") && gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player1");
            tag = "Player1";
            sr.material = player1Material;
        }
        else if (bulletLayer == LayerMask.NameToLayer("Player2") && gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player2");
            tag = "Player2";
            sr.material = player2Material;
        }
    }
}
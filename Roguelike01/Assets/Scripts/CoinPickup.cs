using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    public float waitToBeCollected = 0.5f;

    private void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySfx(5);
        }
    }
}

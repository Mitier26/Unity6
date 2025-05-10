using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Destroy(gameObject);

                // Instantiate broken pieces
                int pieceToDrop = Random.Range(1, maxPieces);

                for (int i = 0; i < pieceToDrop; i++)
                {
                    int randomPiece = Random.Range(0, brokenPieces.Length);
                
                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }
                
                // Drop item
                if (shouldDropItem)
                {
                    float randomValue = Random.Range(0f, 100f);
                    if (randomValue <= itemDropPercent)
                    {
                        int randomItem = Random.Range(0, itemsToDrop.Length);
                        Instantiate(itemsToDrop[randomItem], transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Destroy(gameObject);

                int pieceToDrop = Random.Range(1, maxPieces);

                for (int i = 0; i < pieceToDrop; i++)
                {
                    int randomPiece = Random.Range(0, brokenPieces.Length);
                
                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }
            }
        }
    }
}

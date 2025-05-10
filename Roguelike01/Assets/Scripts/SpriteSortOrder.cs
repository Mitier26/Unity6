using System;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
    }
}

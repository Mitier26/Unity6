using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrokenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDirection;
    
    public float deceleration = 5f;
    
    public float lifeTime = 3f;
    
    public SpriteRenderer spriteRenderer;
    public float fadeSpeed = 2.5f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime;
        
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
        
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime));
            
            if(spriteRenderer.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

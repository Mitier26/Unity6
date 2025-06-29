using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float parallaxEffect;
    private float length;
    private float xPosition;

    void Start()
    {
        cam = Camera.main;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        
        if(distanceMoved > xPosition + length)
        {
            xPosition += length;
        }
    }
}

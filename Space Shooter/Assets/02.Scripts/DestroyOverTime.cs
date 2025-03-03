using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime = 2.0f;

    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}

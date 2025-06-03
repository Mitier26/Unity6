using System;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}

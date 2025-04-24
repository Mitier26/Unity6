using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public Renderer targetRenderer;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();
    }

    public void EnableHighlight()
    {
        if (targetRenderer != null)
            targetRenderer.material.SetFloat("_HighlightOn", 1f);
    }

    public void DisableHighlight()
    {
        if (targetRenderer != null)
            targetRenderer.material.SetFloat("_HighlightOn", 0f);
    }
}

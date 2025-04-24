using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Renderer outlineRenderer;
    private MaterialPropertyBlock block;

    private bool forcedByHint = false;

    void Awake()
    {
        if (outlineRenderer == null)
            outlineRenderer = GetComponentInChildren<Renderer>();

        block = new MaterialPropertyBlock();
    }

    public void EnableOutline(Color color)
    {
        outlineRenderer.GetPropertyBlock(block);
        block.SetFloat("_EnableOutline", 1f);
        block.SetColor("_OutlineColor", color);
        outlineRenderer.SetPropertyBlock(block);
    }

    public void DisableOutline()
    {
        if (forcedByHint) return; // 힌트가 강제로 켠 상태면 끄지 않음
        outlineRenderer.GetPropertyBlock(block);
        block.SetFloat("_EnableOutline", 0f);
        outlineRenderer.SetPropertyBlock(block);
    }

    public void ForceEnableByHint(Color color)
    {
        forcedByHint = true;
        EnableOutline(color);
    }

    public void ClearHintForce()
    {
        forcedByHint = false;
        DisableOutline(); // 필요 시 끔
    }
}

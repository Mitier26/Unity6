using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Renderer outlineRenderer;
    public OutlineInfo info;

    private void Start()
    {
        if (info == null)
            info = GetComponent<OutlineInfo>();

        if (outlineRenderer == null)
            outlineRenderer = GetComponentInChildren<Renderer>();
    }

    public void EnableOutline()
    {
        if (outlineRenderer == null || info == null) return;

        Material[] mats = outlineRenderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].HasProperty("_EnableOutline"))
            {
                mats[i].SetFloat("_EnableOutline", 1f);
                mats[i].SetColor("_OutlineColor", info.GetOutlineColor());
            }
        }

        outlineRenderer.materials = mats;
    }

    public void DisableOutline()
    {
        if (outlineRenderer == null) return;

        Material[] mats = outlineRenderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].HasProperty("_EnableOutline"))
            {
                mats[i].SetFloat("_EnableOutline", 0f);
            }
        }

        outlineRenderer.materials = mats;
    }
}

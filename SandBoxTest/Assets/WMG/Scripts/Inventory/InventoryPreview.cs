using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPreview : MonoBehaviour
{
    public static InventoryPreview Instance { get; private set; }

    public Transform previewAnchor;
    private GameObject currentPreview;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowItem(GameObject prefab)
    {
        if (currentPreview != null)
            Destroy(currentPreview);

        currentPreview = Instantiate(prefab, previewAnchor);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.identity;
        currentPreview.transform.localScale = Vector3.one; // 초기화
    }

    public void Clear()
    {
        if (currentPreview != null)
            Destroy(currentPreview);
    }

    private void Update()
    {
        if (currentPreview != null && Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * 50f;
            float rotY = Input.GetAxis("Mouse Y") * 50f;
            currentPreview.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentPreview.transform.Rotate(Vector3.right, rotY, Space.World);
        }

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0 && currentPreview != null)
        {
            currentPreview.transform.localScale += Vector3.one * scroll * 0.1f;
        }
    }
}


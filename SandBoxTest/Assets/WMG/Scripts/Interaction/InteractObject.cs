using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        ObjectInspector.Instance.EnterInspectMode(gameObject);
    }
}

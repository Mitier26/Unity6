using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType {
    DescriptionOnlyObject,
    ActionObject,
    CollectibleObject,
    InventoryObject,
}

public interface IInteractable {
    void Interact();
}


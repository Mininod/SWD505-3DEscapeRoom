using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public string tooltipText;

    public enum objectType
    {
        None,
        Box,
        Cylinder,
        TestPickup
    }

    public objectType myType;
    public bool collectable;

}

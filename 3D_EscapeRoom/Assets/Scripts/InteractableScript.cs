using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public objectType myType;               //objet type includes all pickups and interactables

    public string hoverTooltipText;         //text to show when hovered over

    public bool canBeClicked;               //if it can be clicked
    public bool collectable;                //if it can be collected

    public string clickTooltipText;                 //if it can be clicked/collected text will show up
    public string clickTooltipTextSuccess;          //if it can be clicked, this will show up if there is a success

    private bool hasBeenTriggered = false;

    public bool getTriggerStatus()
    {
        return hasBeenTriggered;
    }

    public void triggerInteractable()
    {
        hasBeenTriggered = true;
    }
}

//[CustomEditor(typeof(InteractableScript))]
//public class InteractableScriptEditor : AssemblyIsEditorAssembly
//{
//    override public void OnInspectorGUI()
//    {
//        var myScript = target as InteractableScript;


//    }
//}


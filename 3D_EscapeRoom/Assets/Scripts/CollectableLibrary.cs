using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableLibrary : MonoBehaviour
{
    public List<objectType> collectableNames;
    public List<GameObject> collectables;

    public Dictionary<objectType, GameObject> collectableLibrary = new Dictionary<objectType, GameObject>();

    void Start()
    {
        for (int i = 0; i < collectables.Count; ++i)
        {
            collectableLibrary.Add(collectableNames[i], collectables[i]);
        }
    }

    public GameObject getGameObject(objectType type)
    {
        return collectableLibrary[type];
    }
}

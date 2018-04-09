using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveControlScript : MonoBehaviour
{
    public int valveNumber;     //the actual valve number, not the array number

    private PipePuzzleControlScript controller;

    void Start()
    {
        controller = GameObject.Find("PipePuzzleControl").GetComponent<PipePuzzleControlScript>();
    }

    void Update()
    {

    }

    public void toggleValve()
    {
        //animation & sound here
        controller.toggleValve(valveNumber);
    }
}

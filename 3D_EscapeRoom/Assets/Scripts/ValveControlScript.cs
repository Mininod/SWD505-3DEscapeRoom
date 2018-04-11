using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveControlScript : MonoBehaviour
{
    public int gateNumber;     //the actual gate number the handle controls, not the array number

    public bool canControlSecondgate = false;
    public int secondGateNumber = 0;

    private PipePuzzleControlScript controller;

    void Start()
    {
        controller = GameObject.Find("PipePuzzleControl").GetComponent<PipePuzzleControlScript>();
    }

    void Update()
    {

    }

    public void toggleGate()
    {
        //animation & sound here

        if (gateNumber != 0)     //if there is an assigned gate, else nothing happens
        {
            controller.toggleValve(gateNumber);
            if (canControlSecondgate) controller.toggleValve(secondGateNumber);
        }
    }
}

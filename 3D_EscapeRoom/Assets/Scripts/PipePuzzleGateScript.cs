using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleGateScript : MonoBehaviour
{
    private bool gateOpen = false;

    public void toggleGate()
    {
        if(gateOpen)
        {
            //close
            transform.Translate(new Vector3(0, -2, 0));
            gateOpen = false;
        }
        else
        {
            //open
            transform.Translate(new Vector3(0, 2, 0));
            gateOpen = true;
        }
    }
}

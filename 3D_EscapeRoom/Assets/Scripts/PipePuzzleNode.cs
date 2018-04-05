using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleNode : MonoBehaviour
{
    public GameObject[] nodesToDrawTo;
    public bool startActive;
   
    private LineRenderer myLineRenderer;
    private bool line1IsSet = false;
    private bool line2IsSet = false;
    private bool isActive = false;

    void Start ()
    {
        if (startActive) isActive = true;

        myLineRenderer = GetComponent<LineRenderer>();              //get reference to the line renderer  
        myLineRenderer.positionCount = 4;
        //1st line from 0 - 1
        myLineRenderer.SetPosition(0, transform.position);          
        myLineRenderer.SetPosition(1, transform.position);
        //second line from 2 - 3
        myLineRenderer.SetPosition(2, transform.position);
        myLineRenderer.SetPosition(3, transform.position);
    }
	
	void Update ()
    {
        //Code for line 1
        Vector3 direction = nodesToDrawTo[0].transform.position - transform.position;
        var hits = Physics.RaycastAll(transform.position, direction);
        Debug.DrawRay(transform.position, direction);

        if(hits[0].transform.gameObject == nodesToDrawTo[0] && !line1IsSet && isActive)            //if the first item hit is the node - draw the line, must also be active to draw line
        {
            Debug.Log("Line Draw");       
            myLineRenderer.SetPosition(1, nodesToDrawTo[0].transform.position);
            nodesToDrawTo[0].GetComponent<PipePuzzleNode>().setActive();        //activate the next node

            line1IsSet = true;
        }
        else if(hits[0].transform.gameObject != nodesToDrawTo[0] && line1IsSet || !isActive)         //clear line if not active at any point
        {
            Debug.Log("Obstruction");
            myLineRenderer.SetPosition(1, transform.position);
            nodesToDrawTo[0].GetComponent<PipePuzzleNode>().setInactive();        //deactivate the next node

            line1IsSet = false;
        }  
        
        //Code for line 2
        if(nodesToDrawTo.Length > 1)       //if there is a second node (there shouldnt ever be a third)
        {
            Debug.Log("2 Targets");
            Vector3 direction2 = nodesToDrawTo[1].transform.position - transform.position;
            var hits2 = Physics.RaycastAll(transform.position, direction2);
            Debug.DrawRay(transform.position, direction2);

            if (hits2[0].transform.gameObject == nodesToDrawTo[1] && !line2IsSet && isActive)            //if the first item hit is the node - draw the line, must also be active to draw line
            {
                Debug.Log("Line Draw");
                myLineRenderer.SetPosition(3, nodesToDrawTo[1].transform.position);
                nodesToDrawTo[0].GetComponent<PipePuzzleNode>().setActive();        //activate the next node

                line2IsSet = true;
            }
            else if (hits2[0].transform.gameObject != nodesToDrawTo[1] && line2IsSet || !isActive)         //clear line if not active at any point
            {
                Debug.Log("Obstruction");
                myLineRenderer.SetPosition(3, transform.position);
                nodesToDrawTo[1].GetComponent<PipePuzzleNode>().setInactive();        //deactivate the next node

                line2IsSet = false;
            }
        }
	}

    public void setActive()
    {
        isActive = true;
    }

    public void setInactive()
    {
        isActive = false;
    }
}

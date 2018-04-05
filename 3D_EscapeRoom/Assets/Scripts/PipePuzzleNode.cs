using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleNode : MonoBehaviour
{
    public GameObject nodeToDrawTo = null;

    private LineRenderer myLineRenderer;
    private bool lineIsSet = false;

    void Start ()
    {
        myLineRenderer = GetComponent<LineRenderer>();              //get reference to the line renderer     
        myLineRenderer.SetPosition(0, transform.position);          //position 0 is this node
        myLineRenderer.SetPosition(1, transform.position);
    }
	
	void Update ()
    {
        Vector3 direction = nodeToDrawTo.transform.position - transform.position;
        var hits = Physics.RaycastAll(transform.position, direction);
        Debug.DrawRay(transform.position, direction);

        if(hits[0].transform.gameObject == nodeToDrawTo && !lineIsSet)            //if the first item hit is the node - draw the line
        {
            Debug.Log("Line Draw");       
            myLineRenderer.SetPosition(1, nodeToDrawTo.transform.position);

            lineIsSet = true;
        }
        else if(hits[0].transform.gameObject != nodeToDrawTo && lineIsSet)
        {
            Debug.Log("Obstruction");
            myLineRenderer.SetPosition(1, transform.position);
            lineIsSet = false;
        }

        
	}
}

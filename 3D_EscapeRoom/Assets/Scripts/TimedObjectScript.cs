using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectScript : MonoBehaviour
{
    public float maxLifetime;
    private float lifetime = 0;
    
	void Update ()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= maxLifetime) Destroy(gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOutputScript : MonoBehaviour
{
    public GameObject spawn;
    public float ouputTime;
    public float spawnRange;
    private float timer = 0;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        timer += Time.deltaTime;

        Vector3 spawnPos = new Vector3(
            transform.position.x + Random.Range(-spawnRange, spawnRange),
            transform.position.y + Random.Range(-spawnRange, spawnRange), 
            transform.position.z);

		if(timer >= ouputTime)
        {
            Instantiate(spawn, spawnPos, Quaternion.identity);
            timer = 0;
        }
	}
}

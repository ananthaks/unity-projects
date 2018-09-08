using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject obstacles;
    [SerializeField] Vector3 spawnPoint;

	// Use this for initialization
	void Start ()
    {
        SpawnWaves();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void SpawnWaves()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(-spawnPoint.y, spawnPoint.y), spawnPoint.z);
        Quaternion spawnRotation = Quaternion.identity;

        Instantiate(obstacles, spawnPosition, spawnRotation);
    }

}

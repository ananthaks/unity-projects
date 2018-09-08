using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject obstacles;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] int obstacleCount;
    [SerializeField] float startWaitTime;
    [SerializeField] float spawnWaitTime;
    [SerializeField] float waveSpawnTime;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnWaves());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWaitTime);
        while(true)
        {
            for (int count = 0; count < obstacleCount; count++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnPoint.x, spawnPoint.x), Random.Range(-spawnPoint.y, spawnPoint.y), spawnPoint.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(obstacles, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWaitTime);
            }
            yield return new WaitForSeconds(waveSpawnTime);
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] GameObject enemyExplosion;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        print("Bolt::OnTriggerEnter");
        Debug.Log(other.name);
        Debug.Log(other.tag);
        if (other.tag == "Obstacle")
        {
            print("OnTriggerEnter :: Asteroid");
            // TODO: Asteroid Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            // TODO: Make Player Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            // TODO: Make Enemy Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

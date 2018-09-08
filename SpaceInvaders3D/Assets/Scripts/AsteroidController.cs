using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] float tumble = 1;

    private Rigidbody m_rigidBody;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.angularVelocity = Random.insideUnitSphere * tumble;
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // TODO:
            //Instantiate(asteroidExplosion, transform.position, transform.rotation);
            //Instantiate(playerExplosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.tag == "PlayerBolt")
        {
            //Instantiate(asteroidExplosion, transform.position, transform.rotation);
            print("Asteroid::Destroy");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            print("Asteroid::Destroy");
        }
    }
	
	
}

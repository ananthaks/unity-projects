using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] GameObject enemyExplosion;

    [SerializeField] float explosionLifeTime;

    private GameController gameController;

    // Use this for initialization
    void Start ()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            // TODO: Asteroid Explosion
            gameController.OnAsteroidHit(other);
            if (asteroidExplosion != null)
            {
                GameObject explosionObject = Instantiate(asteroidExplosion, transform.position, transform.rotation) as GameObject;
                Destroy(explosionObject, explosionLifeTime);
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            // TODO: Make Player Explosion
            gameController.OnPlayerHit(other);

            if (playerExplosion != null)
            {
                GameObject explosionObject = Instantiate(playerExplosion, transform.position, transform.rotation) as GameObject;
                Destroy(explosionObject, explosionLifeTime);
            }

            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            // TODO: Make Enemy Explosion
            gameController.OnEnemytHit(other);

            if(enemyExplosion != null)
            {
                GameObject explosionObject = Instantiate(enemyExplosion, transform.position, transform.rotation) as GameObject;
                Destroy(explosionObject, explosionLifeTime);
            }

            Destroy(gameObject);
        }
        else if (other.tag == "Bolt")
        {
            // TODO: Make Bolt with Bolt Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "GunShip")
        {
            gameController.OnGunShipHit(other);
            Destroy(gameObject);
        }
    }
}

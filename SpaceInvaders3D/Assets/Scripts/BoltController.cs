using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] GameObject enemyExplosion;

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

            gameController.AddPoints(1);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            // TODO: Make Player Explosion
            gameController.OnPlayerDestroyed();

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            // TODO: Make Enemy Explosion
            gameController.AddPoints(2);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Bolt")
        {
            // TODO: Make Bolt with Bolt Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

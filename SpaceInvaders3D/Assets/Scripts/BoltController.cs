using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    private GameController gameController;
    private bool isActive;

    // Use this for initialization
    void Start ()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        isActive = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(!isActive)
        {
            return;
        }

        if (other.tag == "Obstacle")
        {
            gameController.OnAsteroidHit(other);
            OnHit();
        }
        else if (other.tag == "Player")
        {
            gameController.OnPlayerHit(other);
            OnHit();
        }
        else if (other.tag == "Enemy")
        {
            gameController.OnEnemytHit(other);
            OnHit();
        }
        else if (other.tag == "Bolt")
        {
            Destroy(other.gameObject);
            OnHit();
        }
        else if (other.tag == "GunShip")
        {
            gameController.OnGunShipHit(other);
            OnHit();
        }
    }

    private void OnHit()
    {
        VolumetricLines.VolumetricLineBehavior behavior = GetComponentInChildren<VolumetricLines.VolumetricLineBehavior>();
        if(behavior != null)
        {
            behavior.LineColor = Color.black;
        }
        isActive = false;

    }

}

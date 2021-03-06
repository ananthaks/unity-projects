﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] GameObject playerExplosion;
    [SerializeField] GameObject asteroidExplosion;
    [SerializeField] float tumble = 1;

    private Rigidbody m_rigidBody;
    private GameController gameController;

    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.angularVelocity = Random.insideUnitSphere * tumble;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // TODO:
            //Instantiate(asteroidExplosion, transform.position, transform.rotation);
            //Instantiate(playerExplosion, transform.position, transform.rotation);

            gameController.OnPlayerDestroyed();

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
	
	
}

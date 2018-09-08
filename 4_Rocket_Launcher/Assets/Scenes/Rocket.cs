using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private AudioSource m_audioSource;

    [SerializeField] float Main_Thrust = 80f;
    [SerializeField] float RCS_Thrust = 50f;

    //--------------------------------------
    // Start
    //
    // Initialization
    //--------------------------------------
    private void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
    }

    //--------------------------------------
    // Update
    //
    // Called once per frame
    //--------------------------------------
    private void Update ()
    {
        HandleEvent();
	}

    //--------------------------------------
    // HandleEvent
    //
    // Handle external interactions on rocket
    //--------------------------------------
    private void HandleEvent()
    {
        Thrust();
        Rotate();
    }

    //--------------------------------------
    // OnCollisionEnter
    //--------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            default:
                print("Error: Collided with unknown object");
                break;
        }
    }

    //--------------------------------------
    // Thrust
    //--------------------------------------
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // Thrust
        {
            m_rigidBody.AddRelativeForce(Vector3.up * Main_Thrust * Time.deltaTime);
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
        else
        {
            m_audioSource.Stop();
        }
    }

    //--------------------------------------
    // Rotate
    //--------------------------------------
    private void Rotate()
    {
        // Take manual control of the rotation
        m_rigidBody.freezeRotation = true;

        Vector3 torque = Vector3.forward * (RCS_Thrust * Time.deltaTime);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // Rotate Left
        {
            transform.Rotate(torque);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Rotate Right
        {
            transform.Rotate(-torque);
        }

        // Resume physics control of rotation
        m_rigidBody.freezeRotation = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private AudioSource m_audioSource;

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
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // Thrust
        {
            m_rigidBody.AddRelativeForce(Vector3.up);
            if(!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
        else
        {
            m_audioSource.Stop();
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // Rotate Left
        {
            m_rigidBody.AddRelativeTorque(Vector3.forward);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Rotate Right
        {
            m_rigidBody.AddRelativeTorque(Vector3.back);
        }
    }
}

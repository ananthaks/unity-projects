using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    private bool mThrustActivated;
    private bool mRightTurn;
    private bool mLeftTurn;

    private Rigidbody mRigidBody;
    private AudioSource mAudioSource;

	// Use this for initialization
	void Start () {
        mThrustActivated = false;
        mRightTurn = false;
        mLeftTurn = false;

        mRigidBody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();


        if(mThrustActivated)
        {
            mRigidBody.AddRelativeForce(Vector3.up);
        }

        if(mLeftTurn)
        {
            mRigidBody.AddRelativeTorque(Vector3.forward * Time.deltaTime);
            //transform.Rotate(Vector3.forward * Time.deltaTime);
        }

        if (mRightTurn)
        {
            mRigidBody.AddRelativeTorque(-Vector3.forward * Time.deltaTime);
            //transform.Rotate(-Vector3.forward * Time.deltaTime);
        }

    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.W))
        {
            mThrustActivated = true;
        } 
        else
        {
            mThrustActivated = false;
        }

        if(Input.GetKey(KeyCode.A))
        {
            mRightTurn = false;
            mLeftTurn = true;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            mRightTurn = true;
            mLeftTurn = false;
        }
        else
        {
            mRightTurn = false;
            mLeftTurn = false;
        }
    }
}
/* Written by Kaz Crowe */
/* HealthPickupController.cs */
using UnityEngine;
using System.Collections;

namespace SimpleHealthBar_SpaceshipExample
{
	public class HealthPickupController : MonoBehaviour
	{
		// Reference Variables //
		Rigidbody myRigidbody;
		public ParticleSystem particles;
		public SpriteRenderer mySprite;

		// Controller Booleans //
		bool canDestroy = false;
		bool canPickup = true;
		

		public void Setup ( Vector3 force )
		{
			// Assign the rigidbody component attached to this game object.
			myRigidbody = GetComponent<Rigidbody>();

			// Add the force and torque to the rigidbody.
			myRigidbody.AddForce( force );
			
			StartCoroutine( DelayInitialDestruction( 1.0f ) );
		}

		IEnumerator DelayInitialDestruction ( float delayTime )
		{
			// Wait for the designated time.
			yield return new WaitForSeconds( delayTime );

			// Allow this asteroid to be destoryed.
			canDestroy = true;
		}
	
		void Update ()
		{
			
		}

        public void Kill()
        {
            Debug.Log("OnDestory");
            mySprite.enabled = false;
            if(myRigidbody != null)
            {
                myRigidbody.isKinematic = true;
            }
            particles.Stop();
        }

		void OnTriggerEnter ( Collider theCollider )
		{

		}
	}
}
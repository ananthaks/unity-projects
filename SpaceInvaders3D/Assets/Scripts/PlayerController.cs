using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin = -5.3f;
    public float xMax = 5.3f;
    public float yMin = -2.0f;
    public float yMax = 12.0f;
}

public class PlayerController : MonoBehaviour
{

    private Rigidbody m_rigidBody;
    private AudioSource m_audioSource;

    [SerializeField] GameObject MainWeaponBolt;
    [SerializeField] Transform shotspawn;

    // Editable Fields
    [SerializeField] float speed = 10.0f;
    [SerializeField] float tilt = 4.0f;
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] Boundary boundary;
    
    // Private members
    private float nextFire = 0.0f;

    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void FireWeapons()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(MainWeaponBolt, shotspawn.position, shotspawn.rotation);

            if(!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        FireWeapons();
    }

    void FixedUpdate()
    {
 
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        m_rigidBody.velocity = movement * speed;

        // Constrain to boundary
        m_rigidBody.position = new Vector3
            (
            Mathf.Clamp(m_rigidBody.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(m_rigidBody.position.y, boundary.yMin, boundary.yMax),
            0 
            );


        m_rigidBody.rotation = Quaternion.Euler(m_rigidBody.velocity.y * -tilt, 0.0f, m_rigidBody.velocity.x * -tilt);
    }
}

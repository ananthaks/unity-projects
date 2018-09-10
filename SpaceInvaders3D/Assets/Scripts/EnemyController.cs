using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody m_rigidBody;
    private AudioSource m_audioSource;

    [SerializeField] GameObject MainWeaponBolt;
    [SerializeField] Transform shotspawn;

    // Editable Fields
    [SerializeField] float fireRate = 0.25f;

    // Private members
    private float nextFire = 0.0f;

    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void FireWeapons()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(MainWeaponBolt, shotspawn.position, shotspawn.rotation);

            if (!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // TODO: Make Explosion
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        FireWeapons();
    }

    void FixedUpdate()
    {

       
    }
}

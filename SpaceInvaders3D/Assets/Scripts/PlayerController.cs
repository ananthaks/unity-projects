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

public enum AxisMovement
{
    AxisY,
    AxisZ
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

    [SerializeField] AxisMovement m_axisMovement;
    [SerializeField] int numHitsForceField;
    private int m_currentHitsForceField;
    
    // Private members
    private float nextFire = 0.0f;

    private GameController m_gameController;

    private GameObject m_forceField;


    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            m_gameController = gameControllerObject.GetComponent<GameController>();
        }
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

        if (Input.GetButton("Fire2") && m_gameController != null)
        {
            m_gameController.FireAlternateGuns();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            
                m_gameController.OnPlayerHit(gameObject.GetComponent<Collider>());
                m_gameController.OnEnemytHit(other);
                Destroy(other.gameObject, 2.5f);
            
        }
        if(other.tag == "ForceField")
        {
            m_forceField = GameObject.FindWithTag("ForceField");
            m_currentHitsForceField = numHitsForceField;
            m_gameController.OnPlayerPickUp(PickUpState.PickUpActive);
        }

        if (other.tag == "Health")
        {
            m_gameController.OnPlayerPickUp(PickUpState.PickUpActive);
            m_gameController.OnPlayerHealth();
            Debug.Log(other.tag);
            SimpleHealthBar_SpaceshipExample.HealthPickupController hCon = other.gameObject.GetComponentInChildren<SimpleHealthBar_SpaceshipExample.HealthPickupController>();
            hCon.Kill();
            Destroy(other);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        FireWeapons();
    }

    public bool OnPlayerHit()
    {
        if(m_currentHitsForceField == 0)
        {
            return true;
        }

        m_currentHitsForceField--;

        if(m_currentHitsForceField == 0)
        {
            m_gameController.OnPlayerPickUp(PickUpState.NoActivePickUp);
            Destroy(m_forceField);
        }
        return false;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float moveForwardZ = (m_axisMovement == AxisMovement.AxisZ) ? moveVertical : 0;
        float moveTopY = (m_axisMovement == AxisMovement.AxisY) ? moveVertical : 0;

        Vector3 movement = new Vector3(moveHorizontal, moveTopY, moveForwardZ);
        m_rigidBody.velocity = movement * speed;

        float posY = (m_axisMovement == AxisMovement.AxisY) ? Mathf.Clamp(m_rigidBody.position.y, boundary.yMin, boundary.yMax) : 0;
        float posZ = (m_axisMovement == AxisMovement.AxisZ) ? Mathf.Clamp(m_rigidBody.position.z, boundary.yMin, boundary.yMax) : 0;

        // Constrain to boundary
        m_rigidBody.position = new Vector3
            (
            Mathf.Clamp(m_rigidBody.position.x, boundary.xMin, boundary.xMax),
            posY,
            posZ
            );

        if(m_forceField != null)
        {
            m_forceField.transform.position = m_rigidBody.position;
        }

        m_rigidBody.rotation = Quaternion.Euler(m_rigidBody.velocity.y * -tilt, 0.0f, m_rigidBody.velocity.x * -tilt);
    }
}

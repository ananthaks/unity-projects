using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    //private AudioSource m_audioSource;

    [SerializeField] GameObject MainWeaponBolt;
    [SerializeField] Transform shotspawn;

    // Editable Fields
    [SerializeField] float fireRate = 0.25f;

    // Private members
    private float nextFire = 0.0f;

    private bool m_isFiringEnabled = true;

    private GameController gameController;

    private int m_indexX;
    private int m_indexZ;

    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        //m_audioSource = GetComponent<AudioSource>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    public void EnableDisableFire(bool enable)
    {
        m_isFiringEnabled = enable;
    }

    public void SetIndex(int indexX, int indexZ)
    {
        m_indexX = indexX;
        m_indexZ = indexZ;
    }

    public int GetIndexX()
    {
        return m_indexX;
    }

    public int GetIndexZ()
    {
        return m_indexZ;
    }

    void FireWeapons()
    {
        if (m_isFiringEnabled && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(MainWeaponBolt, shotspawn.position, shotspawn.rotation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // TODO: Make Explosion
            Debug.Log("Collision with Player");
            gameController.OnPlayerDestroyed();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "GunShip")
        {
            GunshipController gunShipObjectController = other.gameObject.GetComponent<GunshipController>();
            gunShipObjectController.OnShotHit();
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

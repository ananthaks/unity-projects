using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin = -5.3f;
    public float xMax = 5.3f;
    public float zMin = -2.0f;
    public float zMax = 12.0f;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    [SerializeField] float speed = 10.0f;
    [SerializeField] float tilt = 4.0f;
    [SerializeField] Boundary boundary;

    public GameObject shot;
    public Transform shotspawn;

    public float fireRate = 0.5f;
    private float nextFire = 0.0f;
    
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotspawn.position, shotspawn.rotation);
        }
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        m_rigidBody.velocity = movement * speed;

        // COntrain to boundary
        m_rigidBody.position = new Vector3
            (
            Mathf.Clamp(m_rigidBody.position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(m_rigidBody.position.z, boundary.zMin, boundary.zMax)
            );


        m_rigidBody.rotation = Quaternion.Euler(0.0f, 0.0f, m_rigidBody.velocity.x * -tilt);


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    private Rigidbody m_rigidBody;

    [SerializeField] float speed;

    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

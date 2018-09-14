using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    StraightMotion,
    OsscilateInX,
    OsscilateInY,
    OsscilateInZ,
};

public class MoveController : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    [SerializeField] MoveType m_moveType;
    [SerializeField] float speedInX;
    [SerializeField] float speedInY;
    [SerializeField] float speedInZ;

    [SerializeField] float oscilationScale = 0.5f;

    private float timer;

    float oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }

    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.velocity = transform.forward * speedInZ;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_moveType == MoveType.OsscilateInX)
        {
            timer += Time.deltaTime;
            m_rigidBody.velocity = new Vector3(
                oscillate(timer, speedInX, oscilationScale),
                m_rigidBody.velocity.y,
                m_rigidBody.velocity.z
            );
        }
    }
}

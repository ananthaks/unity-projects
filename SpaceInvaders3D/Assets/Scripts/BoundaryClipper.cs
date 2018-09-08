using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryClipper : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit(Collider other)
    {
        print("BoundaryClipper::Destroy");
        Destroy(other.gameObject);
    }

}

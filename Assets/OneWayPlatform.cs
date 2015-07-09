using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {

    public BoxCollider parentcollider;
    int cool = 2;

    void Start()
    {
        parentcollider = transform.parent.GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(other);
            Physics.IgnoreCollision(parentcollider, other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit!");
        if (other.tag == "Player")
        {
            Physics.IgnoreCollision(parentcollider, other, false);
        }
    }
}

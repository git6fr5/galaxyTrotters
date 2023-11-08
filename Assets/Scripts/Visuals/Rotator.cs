/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour {

    public Vector3 axis;
    public float speed;

    void FixedUpdate() {
        transform.Rotate(speed * axis * Time.fixedDeltaTime, Space.Self);
    }

}


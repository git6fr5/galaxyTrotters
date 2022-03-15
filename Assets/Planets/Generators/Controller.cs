using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Rigidbody body;
    public float speed;
    public float acceleration;

    Vector3 velocity;
    Vector3 momentum;


    void Start() {
        this.body = GetComponent<Rigidbody>();
    }

    void Update() {

        this.transform.up = transform.position.normalized;

        Vector3 rightMove = Input.GetAxisRaw("Horizontal") * transform.right.normalized;
        Vector3 forwardMove = Input.GetAxisRaw("Vertical") * transform.forward.normalized;
        Vector3 upMomentum = Vector3.zero; // Vector3.Dot(this.body.velocity, transform.up) * transform.up;

        Vector3 targetVelocity = (rightMove + forwardMove).normalized * speed;
        this.body.velocity += (targetVelocity - this.body.velocity) * acceleration * Time.deltaTime + upMomentum;
        this.body.velocity *= 0.995f;

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.body.velocity += transform.up * 22.5f;
        }


    }
}

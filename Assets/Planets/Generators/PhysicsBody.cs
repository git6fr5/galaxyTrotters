using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PhysicsBody : MonoBehaviour
{

    public float mass;

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public float resistance;

    void Start() {
        position = transform.position;
    }

    void Update() {
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f) * Time.deltaTime;
    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;

        position = transform.position;

        position += velocity * deltaTime;
        velocity += acceleration * deltaTime;
        acceleration *= resistance;

        transform.position = position;
    }

    public void AddForce(Vector3 force) {
        acceleration += (force / mass);
    }


}

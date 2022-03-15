using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour {

    public static float G = 6.6738e-5f;

    // Switches.
    public bool resetRotation = false;

    // Rotation.
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float rotationSpeed;

    public float surfaceRadius;
    public float gravitationalRadius;

    public List<Rigidbody> bodies = new List<Rigidbody>();

    Rigidbody body;

    void Start() {
        this.body = GetComponent<Rigidbody>();
    }

    void Update() {

        if (resetRotation) {
            ResetRotation();
            resetRotation = false;
        }

        UpdateBodies();
    }

    private void UpdateBodies() {
        bodies = new List<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, gravitationalRadius);
        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody body = colliders[i].GetComponent<Rigidbody>();
            if (body != null && body != this.body) {
                bodies.Add(body);
            }
        }

        print(bodies.Count);

    }

    void FixedUpdate() {

        float deltaTime = Time.fixedDeltaTime;
        Rotate(deltaTime);

        Attract();

    }

    private void Attract() {

        
        for (int i = 0; i < bodies.Count; i++) {

            Vector3 force = (this.body.position - bodies[i].position) * G * this.body.mass * bodies[i].mass / (this.body.position - bodies[i].position).sqrMagnitude;
            bodies[i].AddForce(force);

            print(force.magnitude / bodies[i].mass);

        }

    }


    private void Rotate(float deltaTime) {
        transform.eulerAngles += rotationAxis * rotationSpeed * deltaTime;
    }

    private void ResetRotation() {
        transform.eulerAngles = Vector3.zero;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);
        Gizmos.DrawWireSphere(transform.position, gravitationalRadius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour {

    public static float G = 6.6738e-5f;

    /* --- Fields --- */
    #region Fields

    // Components.
    [HideInInspector] public Rigidbody body;

    // Radius.
    [SerializeField] private float surfaceRadius;
    [SerializeField] private float gravitationalRadius;

    // Effected Controllers.
    [SerializeField] private List<Rigidbody> bodies = new List<Rigidbody>();

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start() {
        Init();
    }

    void Update() {
        GetBodies();
        AttractBodies();
    }

    #endregion

    /* --- Initialization --- */
    #region Initializtion

    private void Init() {
        // Caching.
        body = GetComponent<Rigidbody>();
    }

    #endregion

    /* --- Attraction --- */
    #region Attraction

    private void GetBodies() {
        bodies = new List<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, gravitationalRadius);
        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody body = colliders[i].GetComponent<Rigidbody>();
            Vector3 displacement = transform.position - colliders[i].transform.position;
            if (body != null && body != this.body) {
                bodies.Add(body);
            }
        }
    }

    private void AttractBodies() {
        for (int i = 0; i < bodies.Count; i++) {
            Vector3 displacement = transform.position - bodies[i].transform.position;
            Vector3 force = G * (body.mass * bodies[i].mass) * displacement.normalized / displacement.sqrMagnitude;
            
            bodies[i].AddForce(force);
            bodies[i].transform.up = -displacement;
        }
    }

    #endregion

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);
        Gizmos.DrawWireSphere(transform.position, gravitationalRadius);
    }

}

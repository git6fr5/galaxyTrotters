/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The destination the station is trying to get to.
/// </summary>
public class Station : MonoBehaviour {

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] private float m_Radius = 1.5f; // The radius of effect that this laser affects.

    //* --- Unity --- */
    // Runs once before the first frame.
    void Start() {

    }

    // Runs once every frame.
    void Update() {

    }

    /* --- Interfaces --- */
    public bool Check(Vector2 position) {
        return (position - (Vector2)transform.position).sqrMagnitude <= m_Radius * m_Radius;
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Draw the shuttle path.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

}

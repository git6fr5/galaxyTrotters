using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] public int MaxScore = 4; // The maximum score that can be gotten from this destination.
    [SerializeField] public float MinRadius = 1f; // The minimum radius which maximises the score.
    [SerializeField] public float MaxRadius = 4f; // The maximum radius which minimises the score.

    //* --- Unity --- */
    // Runs once before the first frame.
    void Start() {

    }

    // Runs once every frame.
    void Update() {

    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, MinRadius);
        Gizmos.DrawWireSphere(transform.position, MaxRadius);
    }

}

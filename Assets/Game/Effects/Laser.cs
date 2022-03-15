/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Focuses the shuttle in a specific direction.
/// </summary>
public class Laser : MonoBehaviour, IEffect {

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] private float m_Radius = 3f; // The radius of effect that this laser affects.
    [SerializeField] private Vector2 m_Direction; // The direction that this laser points towards.

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

    public void Apply(ref List<Vector2> positions) {
        Vector2 endPosition = (Vector2)transform.position + (m_Radius + Shuttle.StepDistance) * m_Direction.normalized;
        //int steps = (int)Mathf.Floor((endPosition - positions[positions.Count - 1]).magnitude / Shuttle.StepDistance);
        //Vector2 gradient = (endPosition - positions[positions.Count - 1]) / steps;
        //for (int i = 0; i < steps; i++) {
        //    positions.Add(positions[positions.Count - 1] + gradient);
        //}
        positions.Add(endPosition);
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Draw the shuttle path.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

}

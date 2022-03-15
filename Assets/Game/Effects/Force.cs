/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a force to slingshot the shuttle.
/// </summary>
public class Force : MonoBehaviour, IEffect {

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] private float m_Radius = 3f; // The radius of effect that this force affects.
    [SerializeField] private float m_Rotation = 90f; // The radius of effect that this force affects.

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

    // What a gorgeous block of unreadable goo.
    public void Apply(ref List<Vector2> positions) {
        // Get the initial parameters.
        Vector2 velocity = positions[positions.Count - 1] - positions[positions.Count - 2];
        Vector2 displacement = (Vector2)transform.position - positions[positions.Count - 1];
        // Figure out the direction we're rotating in.
        float angleA = Vector2.SignedAngle(velocity, displacement);
        float angleB = Vector2.SignedAngle(displacement, velocity);
        angleA = angleA < 0f ? 360f + angleA : angleA;
        angleB = angleB < 0f ? 360f + angleB : angleB;
        float rotationDirection = angleA < angleB ? 1f : -1f;
        // Calculate the final velocity.
        Vector2 targetVelocity = Quaternion.Euler(0f, 0f, rotationDirection * m_Rotation) * velocity;
        // Path until we reach the concentric threshold.
        bool initBelow90 = Vector2.Angle(velocity, displacement) < 90f;
        bool crossedThreshold = false;
        while (!crossedThreshold) {
            positions.Add(positions[positions.Count - 1] + velocity.normalized * Shuttle.StepDistance);
            displacement = (Vector2)transform.position - positions[positions.Count - 1];
            crossedThreshold = (Vector2.Angle(velocity, displacement) < 90f) != initBelow90;
        }
        // Path around the circle until we reach the target velocity.
        float circumfrence = (m_Rotation * Mathf.PI / 180f)  * displacement.magnitude;
        int steps = (int)Mathf.Floor(circumfrence / Shuttle.StepDistance);
        for (int i = 0; i < steps; i++) {
            float angle = i * m_Rotation / (float)steps;
            Vector2 nextPositionOnCircle = transform.position + Quaternion.Euler(0f, 0f, rotationDirection * angle - 180f) * displacement;
            positions.Add(nextPositionOnCircle);
        }
        Vector2 endCircle = transform.position + Quaternion.Euler(0f, 0f, rotationDirection * m_Rotation - 180f) * displacement;
        positions.Add(endCircle);
        // Path out of the area of effect with the target velocity.
        while (Check(positions[positions.Count - 1])) {
            positions.Add(positions[positions.Count - 1] + targetVelocity.normalized * Shuttle.StepDistance);
        }
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Draw the shuttle path.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

}

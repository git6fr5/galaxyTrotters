/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleports the shuttle between ports.
/// </summary>
public class Teleporter : MonoBehaviour, IEffect {

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] private float m_PortRadius = 1.5f; // The radius of effect of a port.
    [SerializeField] private Transform[] ports; // The ports that can be teleported between.

    //* --- Unity --- */
    // Runs once before the first frame.
    void Start() {

    }

    // Runs once every frame.
    void Update() {

    }

    /* --- Interfaces --- */
    public bool Check(Vector2 position) {
        for (int i = 0; i < ports.Length; i++) {
            if ((position - (Vector2)ports[i].position).sqrMagnitude <= m_PortRadius * m_PortRadius) {
                return true;
            }
        }
        return false;
    }

    public void Apply(ref List<Vector2> positions) {
        // Make sure there are at least two ports.
        if (ports.Length <= 1) { return; }
        // Find the starting port.
        int portIndex = 0;
        bool faultyCheck = true;
        for (int i = 0; i < ports.Length; i++) {
            if ((positions[positions.Count - 1] - (Vector2)ports[i].position).sqrMagnitude <= m_PortRadius * m_PortRadius) {
                portIndex = i;
                faultyCheck = false;
                break;
            }
        }
        // Make sure we actually found a port.
        if (faultyCheck) { return; }
        // Get the teleportation point.
        Vector2 displacement = positions[positions.Count - 1] - (Vector2)ports[portIndex].position;
        Vector2 velocity = positions[positions.Count - 1] - positions[positions.Count - 2];
        Vector2 positionA = (Vector2)ports[(portIndex + 1) % ports.Length].position - displacement;
        Vector2 positionB = positionA + Shuttle.StepDistance * velocity.normalized;
        positions.Add(positionA);
        positions.Add(positionB);
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Draw the shuttle path.
        Gizmos.color = Color.blue;
        if (ports != null) {
            for (int i = 0; i < ports.Length; i++) {
                Gizmos.DrawWireSphere(ports[i].position, m_PortRadius);
            }
        }
    }

}

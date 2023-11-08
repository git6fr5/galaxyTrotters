/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    /// <summary>
    /// Teleports the shuttle between ports.
    /// </summary>
    public class Teleporter : GalaxyObject, IEffect {

        #region Fields.

        /* --- Member Variables --- */
        [Space(2), Header("Teleporter Variables")]

        // The radius of effect of a port.
        [SerializeField] 
        private float m_PortRadius = 1.5f; 

        // The maximum distance a port can be from the center.
        [SerializeField] 
        private float m_PortMaxDistance = 1.5f;

        // The ports that can be teleported between.
        [SerializeField] 
        private Transform[] m_Ports; 

        #endregion

        #region Effect Interface

        // Checks whether a position is within a port of this teleporter.
        public bool Check(Vector2 position) {
            for (int i = 0; i < m_Ports.Length; i++) {
                if ((position - (Vector2)m_Ports[i].position).sqrMagnitude <= m_PortRadius * m_PortRadius) {
                    return true;
                }
            }
            return false;
        }

        public void Apply(ref List<Vector2> positions, float stepDistance) {
            // Make sure there are at least two ports.
            if (m_Ports.Length <= 1) { return; }
            // Find the starting port.
            int portIndex = 0;
            bool faultyCheck = true;
            for (int i = 0; i < m_Ports.Length; i++) {
                // Find the next port and make sure the shuttle is not already inside it.
                if ((positions[positions.Count - 1] - (Vector2)m_Ports[i].position).sqrMagnitude <= m_PortRadius * m_PortRadius) {
                    portIndex = i;
                    faultyCheck = false;
                    break;
                }
            }
            // Make sure we actually found a port.
            if (faultyCheck) { return; }
            // Get the teleportation point.
            Vector2 displacement = positions[positions.Count - 1] - (Vector2)m_Ports[portIndex].position;
            Vector2 velocity = positions[positions.Count - 1] - positions[positions.Count - 2];
            Vector2 positionA = (Vector2)m_Ports[(portIndex + 1) % m_Ports.Length].position - displacement;
            Vector2 positionB = positionA + stepDistance * velocity.normalized;
            positions.Add(positionA);
            positions.Add(positionB);
        }

        #endregion

        #region Methods.

        // Runs once every fixed interval.
        void FixedUpdate() {
            for (int i = 0; i < m_Ports.Length; i++) {
                // Make sure that the ports are within the max distance.
                if (((Vector2)m_Ports[i].localPosition).sqrMagnitude > m_PortMaxDistance * m_PortMaxDistance) {
                    m_Ports[i].localPosition = m_PortMaxDistance * (Vector3)((Vector2)m_Ports[i].localPosition).normalized;
                }
            }
        }

        protected override void Draw() {
            base.Draw();
        } 

        protected override void Debug() {
            // Draw the ports.
            if (m_Ports != null) {
                for (int i = 0; i < m_Ports.Length; i++) {
                    // Draw the port.
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(m_Ports[i].position, m_PortRadius);
                    // Draw the line to the center.
                    Gizmos.color = Color.blue + Color.yellow;
                    Gizmos.DrawLine(transform.position, m_Ports[i].position);
                }
            }
            
        }

        #endregion

    }

}

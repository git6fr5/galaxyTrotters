/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    /// <summary>
    /// Focuses the shuttle in a specific direction.
    /// </summary>
    public class Laser : GalaxyObject, IEffect {

        #region Fields.

        /* --- Member Variables --- */
        [Space(2), Header("Laser Variables")]
        
        // The radius of effect that this laser affects.
        [SerializeField]
        private float m_Radius = 1f; 
        
        // The radius of effect that this laser affects.
        [SerializeField] 
        private float m_Length = 4f; 
        public float ActualLength => m_Radius + m_Length;

        #endregion

        #region Effect Interface

        // Checks whether a position is within this laser origin.
        public bool Check(Vector2 position) {
            return (position - (Vector2)transform.position).sqrMagnitude <= m_Radius * m_Radius;
        }

        public void Apply(ref List<Vector2> positions, float stepDistance) {
            // Get the end position of the laser and spit the shuttle out there.
            Vector2 endPosition = (Vector2)transform.position + (ActualLength + stepDistance) * Direction;
            positions.Add(endPosition);
        }

        #endregion

        #region Methods.

        protected override void Debug() {
            // Draw the enter radius.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
            
            // Draw the cone.
            Gizmos.color = Color.yellow;
            Vector3 up = Quaternion.Euler(0f, 0f, 90f) * Direction;
            Vector3 down = Quaternion.Euler(0f, 0f, -90f) * Direction;
            Gizmos.DrawLine(transform.position + m_Radius * up, transform.position + ActualLength * (Vector3)Direction);
            Gizmos.DrawLine(transform.position + m_Radius * down, transform.position + ActualLength * (Vector3)Direction);
        }

        #endregion

    }

}

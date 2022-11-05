/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    /// <summary>
    /// The station the shuttle is trying to get to.
    /// </summary>
    public class Station : GalaxyObject, IEffect {

        /* --- Member Variables --- */

        // The radius of this station.
        [SerializeField] private float m_Radius = 1.5f;

        // Checks whether a position is within this station.
        public bool Check(Vector2 position) {
            return (position - (Vector2)transform.position).sqrMagnitude <= m_Radius * m_Radius;
        }

        
        public void Apply(ref List<Vector2> positions, float stepDistance) {
            // Do nothing for now.
        }

        protected override void Debug() {
            // Draw station.
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }

    }

}

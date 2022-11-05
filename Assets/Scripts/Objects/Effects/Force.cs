/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    /// <summary>
    /// Applies a force to slingshot the shuttle.
    /// </summary>
    public class Force : GalaxyObject, IEffect {

        #region Fields.

        /* --- Member Variables --- */
        [Space(2), Header("Force Variables")]

        // The radius of effect that this force affects.
        [SerializeField] 
        private float m_Radius = 1.5f; 
        
        // The amount of rotation this force applies.
        [SerializeField] 
        private float m_Rotation = 45f; 

        #endregion

        #region Effect Interface

        // Checks whether a position is within the area of effect of this force.
        public bool Check(Vector2 position) {
            return (position - (Vector2)transform.position).sqrMagnitude <= m_Radius * m_Radius;
        }

        // What a gorgeous block of unreadable goo.
        public void Apply(ref List<Vector2> positions, float stepDistance) {
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
                positions.Add(positions[positions.Count - 1] + velocity.normalized * stepDistance);
                displacement = (Vector2)transform.position - positions[positions.Count - 1];
                crossedThreshold = (Vector2.Angle(velocity, displacement) < 90f) != initBelow90;
            }
            // Path around the circle until we reach the target velocity.
            float circumfrence = (m_Rotation * Mathf.PI / 180f)  * displacement.magnitude;
            int steps = (int)Mathf.Floor(circumfrence / stepDistance);
            for (int i = 0; i < steps; i++) {
                float angle = i * m_Rotation / (float)steps;
                Vector2 nextPositionOnCircle = transform.position + Quaternion.Euler(0f, 0f, rotationDirection * angle - 180f) * displacement;
                positions.Add(nextPositionOnCircle);
            }
            Vector2 endCircle = transform.position + Quaternion.Euler(0f, 0f, rotationDirection * m_Rotation - 180f) * displacement;
            positions.Add(endCircle);
            // Path out of the area of effect with the target velocity.
            while (Check(positions[positions.Count - 1])) {
                positions.Add(positions[positions.Count - 1] + targetVelocity.normalized * stepDistance);
            }
        }

        #endregion

        #region Methods.

        protected override void Debug() {
            // Draw the radius within which this force exerts an effect.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }

        #endregion

    }

}

/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy;
using Galaxy.UI;
using GalaxyObject = Galaxy.Objects.GalaxyObject;

namespace Galaxy.UI {

    ///<summary>
    /// Points in a direction.
    ///<summary>
    public class DirectionIndicator : MonoBehaviour {

        #region Fields.

        /* --- Static Variables --- */

        // The period by which this animates.
        public static float Period = 1.5f;

        // The radius this arrow stays at.
        public static float Radius = 0.75f;

        // The amplitude that this oscillates with.
        public static float Amplitude = 0.1f;
        
        /* --- Member Components --- */

        // The object that this arrow tracks.
        [SerializeField, ReadOnly]
        private GalaxyObject m_Object = null;

        #endregion

        #region Methods.

        void Start() {
            m_Object = transform.parent.GetComponent<GalaxyObject>();
        }

        void Update() {
            transform.localScale = (Mathf.Sin(Period * Game.CurrentTime) * Amplitude + 1f - Amplitude) * new Vector3(1f, 1f, 1f);
            PointTowards(m_Object.Direction, Radius);
        }

        public void PointTowards(Vector2 target, float offset) {
            float angle = Vector2.SignedAngle(Vector2.right, target.normalized);
            transform.localPosition = offset * target.normalized;
            transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

        #endregion

    }

}
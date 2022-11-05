using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Galaxy.UI;

namespace Galaxy.UI {

    /// <summary>
    /// One of the destinations that a tourist on the shuttle would want to see.
    /// </summary>
    public class Destination : GalaxyUI {

        #region Data Structures

        public enum DestinationType {
            Planet
        }

        #endregion
        
        #region Fields
        
        /* --- Member Variables --- */
        [Space(2), Header("Destination Variables")]
        
        // The type of destination this is.
        [SerializeField] 
        public DestinationType Type = DestinationType.Planet; 
        
        // The radius between which minimises/maximises the score.
        [SerializeField] 
        private GalaxyUI.Range m_Radius = new Range(1f, 3f);
        public GalaxyUI.Range Radius => m_Radius;

        // The mini which maximises the score.
        [SerializeField] 
        private float m_Score = 16f;
        public GalaxyUI.Range Score => new Range(0f, m_Score);

        #endregion

        #region Methods.

        // Runs once before the first frame.
        void Start() {
            Valuename = "Score";
            Value = 0;
            MaxValue = m_Score;
        }
        
        /* --- Debug --- */
        void OnDrawGizmos() {
            // Draw the force position.
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, m_Radius.Min);
            Gizmos.DrawWireSphere(transform.position, m_Radius.Max);
        }

        #endregion

    }

}

/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Galaxy.UI;

/* --- Definitions --- */
using Tourist = Galaxy.UI.Labels.Tourist;

namespace Galaxy.UI.Labels {

    /// <summary>
    /// 
    /// </summary>
    public class ScoreTracker : GalaxyLabel {

        #region Fields.

        /* --- Member Variables --- */
        
        [HideInInspector]
        private Tourist[] m_AllTourists;

        [SerializeField, ReadOnly] 
        private int m_TotalScore = 0;

        [SerializeField, ReadOnly] 
        private int m_MaxScore = 0;

        [SerializeField] 
        private Text m_Textbox = null;

        #endregion

        // Runs once before the first frame.
        void Start() {
            m_AllTourists = (Tourist[])GameObject.FindObjectsOfType(typeof(Tourist));
        }

        // Runs once every frame.
        void Update() {
            
            foreach (Tourist tourist in m_AllTourists) {
                if (tourist.Active) {
                    m_TotalScore += (int)tourist.Value;
                    m_MaxScore += (int)tourist.MaxValue;
                }
            }

            m_Textbox.text = "SCORE: " + m_TotalScore.ToString() + "/" + m_MaxScore.ToString();

        }

    }

}
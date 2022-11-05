/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Galaxy.UI;

namespace Galaxy.UI {

    ///<summary>
    ///
    ///<summary>
    public class GalaxyUI : MonoBehaviour {

        #region Data Structures.

        [System.Serializable]
        public class Range {

            public float Min;
            public float Max;

            public Range(float min, float max) {
                Min = min;
                Max = max;
            }

            public float Evaluate(float x) {
                if (x >= 1f) {
                    return Max;
                }
                else if (x <= 0f) {
                    return Min;
                }
                return (Max - Min) * x + Min;
            }

            public float Ratio(float v) {
                if (v >= Max) {
                    return 1f;
                }
                else if (v <= Min) {
                    return 0f;
                }
                return (v - Min) / (Max -Min);
            }

        }

        #endregion

        #region Fields.

        /* --- Member Variables --- */

        // The textbox for the name of this component.
        [SerializeField] 
        private Text m_NameBox;
        
        // The name of this component.
        [SerializeField]
        private string m_Name = "Unnamed";
        public string Name {
            set {
                m_Name = value;
                UpdateName(m_NameBox, m_Name);
            }
        }

        // The textbox for the value of this component.
        [SerializeField] 
        private Text m_ValueBox;

        // The name of the value that is relevant to this component.
        private string m_Valuename = "Value";
        public string Valuename { 
            set { 
                m_Valuename = value;
                UpdateValue(m_ValueBox, m_Valuename, m_Value, m_MaxValue);
            }
        }

        // The maximum value of this component. (-1 for N/A)
        private float m_MaxValue = -1f;
        public float MaxValue { 
            set { 
                m_MaxValue = value;
                UpdateValue(m_ValueBox, m_Valuename, m_Value, m_MaxValue);
            }
        }

        // The current value of this component.
        private float m_Value = 0f;
        public float Value { 
            set { 
                m_Value = value;
                UpdateValue(m_ValueBox, m_Valuename, m_Value, m_MaxValue);
            }
        }

        #endregion

        #region Methods.

        void Awake() {
            Name = m_Name;
        }

        // Updates the name field.
        public void UpdateName(Text textbox, string text) {
            textbox.text = text;
        }

        // Updates the value field.
        public void UpdateValue(Text valuebox, string valuename, int value, int maxValue = -1) {
            if (maxValue == -1) {
                valuebox.text = valuename + ": " + value.ToString();
                return;
            }
            valuebox.text = valuename + ": " + value.ToString() + "/" + maxValue.ToString();
        }

        // Updates the value field when floats are passed through.
        public void UpdateValue(Text valuebox, string valuename, float value, float maxValue = -1f) {
            int v = (int)Mathf.Floor(value);
            int mv = (int)Mathf.Ceil(maxValue);
            UpdateValue(valuebox, valuename, v, mv);
        }

        #endregion

    }
}
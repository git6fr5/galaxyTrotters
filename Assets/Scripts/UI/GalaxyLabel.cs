/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Galaxy.UI;

namespace Galaxy.UI.Labels {

    ///<summary>
    ///
    ///<summary>
    public class GalaxyLabel : MonoBehaviour {

        #region Fields.

        /* --- Static Variables --- */

        // To assign the name box.
        private static string DefaultNameboxName = "Name";

        // To assign the name box.
        private static string DefaultValueboxName = "Value";

        /* --- Member Components --- */

        // The canvas where this is labeled.
        [SerializeField]
        private Canvas m_Canvas = null;

        // The textbox for the name of this component.
        private Text m_NameBox = null;

        // The textbox for the value of this component.
        private Text m_ValueBox = null;

        /* --- Member Variables --- */
        
        // The name of this component.
        [SerializeField, ReadOnly]
        private string m_Name = "Unnamed";
        public string Name {
            set {
                m_Name = value;
                UpdateName(m_NameBox, m_Name);
            }
        }

        
        // The name of the value that is relevant to this component.
        [SerializeField, ReadOnly]
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
            get {
                return m_MaxValue;
            }
            set { 
                m_MaxValue = value;
                UpdateValue(m_ValueBox, m_Valuename, m_Value, m_MaxValue);
            }
        }

        // The current value of this component.
        private float m_Value = 0f;
        public float Value { 
            get {
                return m_Value;
            }
            set { 
                m_Value = value;
                UpdateValue(m_ValueBox, m_Valuename, m_Value, m_MaxValue);
            }
        }

        #endregion

        #region Methods.

        void Awake() {
            AssignTextbox(m_Canvas, ref m_NameBox, DefaultNameboxName);
            AssignTextbox(m_Canvas, ref m_ValueBox, DefaultValueboxName);
            Name = gameObject.name;
        }

        // Assigns the textboxes from the children.
        private void AssignTextbox(Canvas canvas, ref Text assignment, string name) {
            foreach (Transform child in canvas.transform) {
                Text textbox = child.GetComponent<Text>();
                if (textbox != null && textbox.name == name) {
                    assignment = child.GetComponent<Text>();
                    return;
                }
            }
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
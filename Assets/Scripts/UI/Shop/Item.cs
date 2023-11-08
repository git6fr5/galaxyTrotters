/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.UI.Labels;
using GalaxyObject = Galaxy.Objects.GalaxyObject;

namespace Galaxy.UI.Labels {

    /// <summary>
    /// An item that can be purchased from the shop.
    /// </summary>
    public class Item : GalaxyLabel {

        #region Fields.

        /* --- Static Variables --- */

        // The area that this can be selected in.
        public static float DefaultSelectionRadius = 0.5f; 

        /* --- Member Variables --- */
        [Space(2), Header("Item Variables")]

        // The object that can be purchased.
        [SerializeField] 
        private GalaxyObject m_Object;
        public GalaxyObject ItemObject {
            get { 
                return m_Object; 
            }
            set {
                m_Object = value;
                m_Object.transform.parent = transform;
                m_Object.transform.localPosition = Vector3.zero;
                m_Object.gameObject.SetActive(false);
                CreateFrame(m_Object, transform);
            }
        } 

        // The cost of purchasing this object.
        [SerializeField] 
        private float m_Cost;
        public float Cost {
            get { 
                return m_Cost;
            }
            set {
                m_Cost = value;
                Value = m_Cost;
            }
        }

        // The position of this object.
        public Vector2 Position => (Vector2)transform.position;

        // If this object is being held.
        [SerializeField, ReadOnly] 
        private bool m_Held = false;
        public bool Held => m_Held;

        // If this object is being hovered over.
        [SerializeField, ReadOnly] 
        private bool m_Hover = false;
        public bool Hover => m_Hover;

        // The last cached object created from this item.
        [SerializeField, ReadOnly] 
        private GalaxyObject m_CachedObject;

        #endregion

        #region Methods.

        // Runs once before the first frame.
        void Start() {
            Valuename = "Cost";
            Value = m_Cost;
            MaxValue = -1f;
        }

        void Update() {

            // Whether this object is being hovered over
            m_Hover = (Game.MousePosition - Position).sqrMagnitude < (DefaultSelectionRadius * DefaultSelectionRadius);

            // Create new objects on click.
            if (m_Hover && Game.LPress && !Game.AltPressed) {
                m_CachedObject = CreateNewGalaxyObject();
                m_CachedObject.ForceSelect(true);
                m_Held = true;
            }

            // A little buffer to make sure they're placed somewhere.
            if (Game.LRelease && m_CachedObject != null) {
                if ((m_CachedObject.Position - Position).magnitude < Shop.ButtonRadius) {
                    Destroy(m_CachedObject.gameObject);
                }
            }

            if ((Game.MousePosition - Position).magnitude > Shop.ButtonRadius) {
                m_Held = false;
            }

        }

        // Creates a new object.
        public GalaxyObject CreateNewGalaxyObject() {
            GameObject newObject = Instantiate(m_Object.gameObject, transform.position, Quaternion.identity, Game.Instance.transform);
            newObject.SetActive(true);
            return newObject.GetComponent<GalaxyObject>();
        }

        public static void CreateFrame(GalaxyObject galaxyObject, Transform parent) {
            Sprite picture = galaxyObject.GetComponent<SpriteRenderer>()?.sprite;
            if (picture == null) { return; }
            SpriteRenderer frame = new GameObject("Frame", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            frame.sprite = picture;
            frame.sortingLayerName = "UI";
            frame.transform.parent = parent;
            frame.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            frame.transform.localPosition = Vector3.zero;
            // frame.sortingOrder = 1000;
        }

        // Draw gizmos to help with debugging.
        void OnDrawGizmos() {
            // Use different colors and sizes to indicate
            // the different states.
            float size = DefaultSelectionRadius;
            if (m_Held) {
                Gizmos.color = Color.red;
                size *= 0.9f;
            }
            else if (m_Hover) {
                Gizmos.color = Color.yellow;
                size *= 1.05f;
            }
            else {
                Gizmos.color = Color.white;
            }
            Gizmos.DrawWireSphere(transform.position, size);
        }

        #endregion

    }

}

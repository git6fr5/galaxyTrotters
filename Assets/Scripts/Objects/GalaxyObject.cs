/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    ///<summary>
    /// The base for any object in this game.
    ///<summary>
    public class GalaxyObject : MonoBehaviour {

        #region Fields

        /* --- Static Variables --- */
        
        // The default canvas size for sprites
        public static int DefaultCanvasSize = 512;

        // The increment precision this rotates with.
        public static int DefaultRotationPrecision = 90;
        public static float DeltaAngle => 360f / (float)DefaultRotationPrecision;

        // The delay between processing inputs.
        public static float DefaultInputDelay = 0.025f; 

        // The area that this can be selected in.
        public static float DefaultSelectionRadius = 0.5f; 

        /* --- Member Components --- */

        // The sprite renderer attached to this object.
        public SpriteRenderer m_SpriteRenderer => GetComponent<SpriteRenderer>();

        /* --- Member Settings --- */

        // Whether this object can be deleted.
        [SerializeField]
        private bool m_Deletable = false;

        // Whether this object can be moved.
        [SerializeField]
        private bool m_Moveable = false;

        // Whether this object can be rotated.
        [SerializeField]
        private bool m_Rotatable = false;

        /* --- Member Variables --- */

        // The position of this object.
        public Vector2 Position => (Vector2)transform.position;
        
        // The direction this object is facing.
        [SerializeField, ReadOnly]
        private float m_Angle = 0f;
        public float Angle => m_Angle;
        public Vector2 Direction => Quaternion.Euler(0f, 0f, m_Angle) * Vector2.right;

        // If this object is being held.
        [SerializeField, ReadOnly] 
        private bool m_Held = false;
        public bool Held => m_Held;

        // If this object has been selected.
        [SerializeField, ReadOnly] 
        private bool m_Selected = false; 
        public bool Selected => m_Selected; 
        
        // If this object is being hovered over.
        [SerializeField, ReadOnly] 
        private bool m_Hover = false;
        public bool Hover => m_Hover;

        [SerializeField, ReadOnly] 
        private float m_RotationTicks = 0f;

        #endregion

        #region Methods

        // Runs once per frame.
        private void Update() {

            // Whether this object is being hovered over
            m_Hover = (Game.MousePosition - Position).sqrMagnitude < (DefaultSelectionRadius * DefaultSelectionRadius);

            // Toggle selection/begin holding.
            if (m_Hover && Game.LPress && !Game.AltPressed) {
                m_Selected = true;
                m_Held = true;
            }
            else if (Game.LPress) {
                m_Selected = false;
            }

            // Release hold.
            if (Game.LRelease) {
                m_Held = false;
            }

            // Destroy the object.
            if (m_Deletable && Game.RPress) {
                Destroy(gameObject);
            }

            // Move the object.
            if (m_Moveable && m_Held) { 
                Move(Game.MousePosition); 
            }

            // Rotate the object.
            if (m_Rotatable && m_Selected) {
                Rotate(Game.Scroll, Time.deltaTime);
            }

            // Draws the sprite to the screen.
            Draw();

        }

        // Moves the object to the given position.
        void Move(Vector2 position) {
            transform.position = (Vector3)position;
        }

        // Rotates the object in the direction of the rotation.
        void Rotate(float rotation, float dt) {
            if (rotation == 0f) {
                m_RotationTicks = 0f;
                return;
            }

            // Increment the ticks.
            m_RotationTicks -= dt;

            // Rotate the object by the delta angle.
            if (m_RotationTicks <= 0f) {
                m_Angle += (rotation * DeltaAngle);
                m_RotationTicks = DefaultInputDelay;
            }

        }

        // Forcefully selects the object.
        public void ForceSelect(bool select, bool alsoHold = true) {
            m_Selected = select;
            m_Held = select && alsoHold;
        }

        // Draw the sprite.
        protected virtual void Draw() {
            // Guards against drawing without a renderer.
            if (m_SpriteRenderer == null) { return; }

            // Use different colors and sizes to indicate
            // the different states.
            float scale = 1f;
            float outlineWidth = 0.025f;
            Color outlineColor = Color.white;
            if (m_Held) {
                outlineColor = Color.red;
                outlineWidth = 0.1f;
                scale = 0.9f;
            }
            else if (m_Selected) {
                outlineColor = new Color(0.5f, 0.5f, 1f);
                outlineWidth = 0.05f;
                scale = 1.025f;
            }
            else if (m_Hover) {
                outlineColor = Color.yellow;
                outlineWidth = 0.04f;
                scale = 1.05f;
            }
            m_SpriteRenderer.material.SetColor("_OutlineColor", outlineColor);
            m_SpriteRenderer.material.SetFloat("_OutlineWidth", outlineWidth);
            transform.localScale = new Vector3(1f, 1f, 1f) * scale;
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
            else if (m_Selected) {
                Gizmos.color = Color.green;
            }
            else if (m_Hover) {
                Gizmos.color = Color.yellow;
                size *= 1.05f;
            }
            else {
                Gizmos.color = Color.white;
            }
            Gizmos.DrawWireSphere(transform.position, size);
            Debug();
        }

        protected virtual void Debug() {
            // Do nothing.
        }

        #endregion

    }

}
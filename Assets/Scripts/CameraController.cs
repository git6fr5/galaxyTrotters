/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy;

// TODO: Clean.
namespace Galaxy {

    ///<summary>
    ///
    ///<summary>
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour {

        #region Fields

        /* --- Member Components --- */

        [SerializeField] 
        private Camera m_MainCamera => GetComponent<Camera>();

        /* --- Member Variables --- */

        [SerializeField, ReadOnly] 
        private bool m_Moving = false;
        private Vector3 m_CachedPosition = new Vector3(0f, 0f, 0f);
        private Vector2 m_CachedMousePosition = new Vector2(0f, 0f);

        #endregion

        #region Methods

        void Update() {
            if (!Game.AltPressed) { 
                m_Moving = false;
                return; 
            }

            if (Game.LPress) {
                m_Moving = true;
                m_CachedMousePosition = Game.MousePosition;
                m_CachedPosition = transform.position;
            }
            else if (Game.LRelease) {
                m_Moving = false;
            }

            if (m_Moving) {
                Vector2 offset = Game.MousePosition - m_CachedMousePosition;
                transform.position = m_CachedPosition - (Vector3)offset;
                m_CachedMousePosition = Game.MousePosition;
                m_CachedPosition = transform.position;
            }

            float zoomSpeed = 5f * m_MainCamera.orthographicSize;
            float minSize = 4f;
            float maxSize = 100f;
            m_MainCamera.orthographicSize += Game.Scroll * zoomSpeed *  Time.deltaTime;
            if (m_MainCamera.orthographicSize < minSize) {
                m_MainCamera.orthographicSize = minSize;
            }
            else if (m_MainCamera.orthographicSize > maxSize) {
                m_MainCamera.orthographicSize = maxSize;
            }

        }

        #endregion


    }

}
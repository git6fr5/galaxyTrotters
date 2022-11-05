/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Galaxy;

namespace Galaxy {

    /// <summary>
    /// 
    /// </summary>
    public class Game : MonoBehaviour {

        #region Fields.

        /* --- Singleton Objects --- */

        // The singleton.
        public static Game Instance;

        // The main camera
        [SerializeField] 
        private Camera m_Camera;
        public static Camera MainCamera => Instance.m_Camera;

        // The pixel perfect camera attached to the main camera.
        [SerializeField]
        private PixelPerfectCamera m_PixelPerfectCamera;
        public static PixelPerfectCamera MainPixelPerfectCamera => Instance.m_PixelPerfectCamera;

        /* --- Static Variables --- */

        // Position.
        public static Vector2 MousePosition => (Vector2)MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

        // Left click.
        public static bool LPress => UnityEngine.Input.GetMouseButtonDown(0);
        public static bool LRelease => UnityEngine.Input.GetMouseButtonUp(0);
                
        // Right click.
        public static bool RPress => UnityEngine.Input.GetMouseButtonDown(1);
        public static bool RRelease => UnityEngine.Input.GetMouseButtonDown(1);

        // Scroll wheel.
        public static float Scroll => UnityEngine.Input.mouseScrollDelta.y;

        #endregion Fields.

        #region Methods.

        void Start() {
            Instance = this;
        }

        // Gets an object in the scene with the given component
        public static object Get<T>() {
            return GameObject.FindObjectOfType(typeof(T));
        }
        
        // Gets all the objects in the scene with the given component.
        public static object[] GetAll<T>() {
            return GameObject.FindObjectsOfType(typeof(T));
        }

        // Draws debug information.
        void OnDrawGizmos() {
            // Draw the camera bounds.
            if (m_PixelPerfectCamera != null) {
                Gizmos.color = Color.green;
                float width = (float)m_PixelPerfectCamera.refResolutionX / (float)m_PixelPerfectCamera.assetsPPU;
                float height = (float)m_PixelPerfectCamera.refResolutionY / (float)m_PixelPerfectCamera.assetsPPU;
                Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
            }

        }

        #endregion

    }

}

    

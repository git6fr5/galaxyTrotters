/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Galaxy;
using Galaxy.Objects;

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

        /* --- Static Variables --- */

        // Position.
        public static Vector2 MousePosition => (Vector2)MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

        // Left click.
        public static bool LPress => UnityEngine.Input.GetMouseButtonDown(0);
        public static bool LRelease => UnityEngine.Input.GetMouseButtonUp(0);
                
        // Right click.
        public static bool RPress => UnityEngine.Input.GetMouseButtonDown(1);
        public static bool RRelease => UnityEngine.Input.GetMouseButtonDown(1);

        // Holding alt.
        public static bool AltPressed => Instance.m_AltPressed;
        private bool m_AltPressed = false;

        // Scroll wheel.
        public static float Scroll => UnityEngine.Input.mouseScrollDelta.y;

        // Tracks thet time since the game has started.
        [SerializeField] 
        private float m_Time = 0;
        public static float CurrentTime => Instance.m_Time;

        // Tracks thet frames since the game has started.
        [SerializeField] 
        private float m_Frames = 0;
        private float m_ThousandFrames = 0;
        public static float Frames => 1000 * Instance.m_ThousandFrames + Instance.m_Frames;

        [SerializeField] 
        private Station m_StationBase;
        public bool m_NextRing = false;
        public int m_Count = 0;

        #endregion Fields.

        #region Methods.

        void Start() {
            Instance = this;
        }

        void Update() {
            m_Frames += 1;
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt)) {
                m_AltPressed = true;
            }
            else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftAlt)) {
                m_AltPressed = false;
            }

            int m_BaseCount = 2;
            float m_BaseRadius = 10f;
            float m_IncrementSize = 10f;
            float m_BaseDistance = 20f;
            float m_IncrementDist = 3f;
            if (m_NextRing) {


                float exp = m_Count / 3f;
                int numberOfStations = (int)(Mathf.Pow(2f, exp) * m_BaseCount);

                float radius = (m_BaseDistance + m_IncrementSize) * m_Count;

                float circ = 2f * Mathf.PI * radius;
                float distanceBetweenStations = m_BaseDistance + + m_IncrementDist * m_Count;
                numberOfStations = (int)Mathf.Floor(circ / distanceBetweenStations);
                if (numberOfStations > 4) {
                    numberOfStations += Random.Range(-2, 2);
                }

                GenerateRing(radius,  numberOfStations, 7f);
                m_Count += 1;
                m_NextRing = false;
            }

        }

        public void GenerateRing(float radius, int numberOfStations, float var) {

            for (int i = 0; i < numberOfStations; i++) {
                
                Station station = Instantiate(m_StationBase.gameObject).GetComponent<Station>();
                station.gameObject.SetActive(true);
                station.transform.parent = transform;
                station.transform.localPosition = Quaternion.Euler(0f, 0f, (float)i / (float)numberOfStations *360f) * (Vector2.right * radius);
                station.transform.localPosition += var * (Vector3)Random.insideUnitCircle;
            }
            
        }

        void FixedUpdate() {
            m_Time += Time.fixedDeltaTime;
        }

        // Gets an object in the scene with the given component
        public static object Get<T>() {
            return GameObject.FindObjectOfType(typeof(T));
        }
        
        // Gets all the objects in the scene with the given component.
        public static object[] GetAll<T>() {
            return GameObject.FindObjectsOfType(typeof(T));
        }

        #endregion

    }

}

    

/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy;
using Galaxy.UI;
using Galaxy.Objects;
using GalaxyObject = Galaxy.Objects.GalaxyObject;

namespace Galaxy.UI {

    ///<summary>
    ///
    ///<summary>
    public class PathIndicator : MonoBehaviour {

        #region Fields.

        /* --- Static Variables --- */

        // The period by which this animates.
        public static float Period = 0.5f;

        // The radius this arrow stays at.
        public static float DefaultDrawDistance = 1f / 4f;

        // The amplitude that this oscillates with.
        public static float Amplitude = 0.25f;

        /* --- Member Components --- */

        // The object that this arrow tracks.
        [SerializeField, ReadOnly]
        private Station m_Shuttle = null;

        /* --- Member Variables --- */

        // The sprite for the path dot.
        [SerializeField] 
        private Sprite m_PathDotSprite = null;

        // The array of path dots that have been instantiated.
        [SerializeField, ReadOnly]
        private List<SpriteRenderer> m_PathDots = new List<SpriteRenderer>();

        // The number of path dots that are currently active.
        [SerializeField, ReadOnly] 
        private int m_PathLength = 0;

        #endregion

        #region Methods.

        void Start() {
            m_Shuttle = transform.parent.GetComponent<Station>();
        }

        void Update() {
            m_PathLength = DrawPath((Vector2)m_Shuttle.transform.position, m_Shuttle.Path, m_Shuttle.FuelIndices);
            WavePath();
        }

        public int DrawPath(Vector2 shuttlePosition, List<Vector2> path, List<int> fuelIndices) {
            int increment = (int)Mathf.Floor(DefaultDrawDistance / Station.DefaultStepDistance);

            int pathDotIndex = 0;
            bool crossedMinimumRadius = false;

            for (int i = increment; i < path.Count; i+=increment) {

                if (!crossedMinimumRadius) {
                    if ((path[i] - shuttlePosition).magnitude > 1f) {
                        crossedMinimumRadius = true;
                    }
                }

                if (crossedMinimumRadius && fuelIndices.Contains(i)) {

                    while (pathDotIndex >= m_PathDots.Count) {
                        SpriteRenderer pathDot = new GameObject("Path Dot " + pathDotIndex.ToString(), typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                        pathDot.sprite = m_PathDotSprite;
                        pathDot.transform.parent = transform;
                        m_PathDots.Add(pathDot);
                    }

                    m_PathDots[pathDotIndex].gameObject.SetActive(true);
                    m_PathDots[pathDotIndex].transform.position = (Vector3)path[i];
                    pathDotIndex += 1;

                }

            }

            for (int i = pathDotIndex; i < m_PathDots.Count; i++) {
                m_PathDots[i].gameObject.SetActive(false);
            }

            return pathDotIndex;

        }

        public void WavePath() {

            for (int i = 0; i < m_PathLength; i++) {
                
                float sinVal = Mathf.Sin(-Period * Game.CurrentTime + (Mathf.PI * (float)i / 16f));
                Vector3 scale = (sinVal * Amplitude + 1f - Amplitude) * new Vector3(1f, 1f, 1f);
                m_PathDots[i].transform.localScale = scale;

                if (i > 0) {
                    Vector2 direction = m_PathDots[i-1].transform.position - m_PathDots[i].transform.position;
                    float angle = Vector2.SignedAngle(Vector2.right, direction);
                    m_PathDots[i].transform.eulerAngles = new Vector3(0f, 0f, angle);


                }

                
            }

        }

        #endregion

    }
}
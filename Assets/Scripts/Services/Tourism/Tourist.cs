/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Galaxy.UI;
using Shuttle = Galaxy.Objects.Shuttle;

namespace Galaxy.UI {

    /// <summary>
    /// Tracks all the destinations that a tourist on the shuttle sees.
    /// </summary>
    [RequireComponent(typeof(Shuttle))]
    public class Tourist : GalaxyUI {

        #region Fields.

        /* --- Member Variables --- */
        [Space(2), Header("Tourist Variables")]

        // The shuttle that this tourist is on.
        [HideInInspector]
        private Shuttle m_MainShuttle;

        // A collection holding the total revenue this tourist generates.
        [SerializeField, ReadOnly] 
        private Dictionary<Destination, float> m_ScoreBook = new Dictionary<Destination, float>(); 

        #endregion

        // Runs once before the first frame.
        void Start() {
            // Caching components.
            m_MainShuttle = GetComponent<Shuttle>();

            // Set up the score book.
            Destination[] destinations = (Destination[])Game.GetAll<Destination>();
            float maxScore = 0;
            for (int i = 0; i < destinations.Length; i++) {
                m_ScoreBook.Add(destinations[i], 0);
                maxScore += destinations[i].Score.Max;
            }
            
            Value = 0;
            MaxValue = maxScore;
        }

        // Runs once every frame.
        void FixedUpdate() {
            GetScores();
        }

        private void GetScores() {
            // Update the score book.
            float currentScore = 0;
            Dictionary<Destination, float> newScoreBook = new Dictionary<Destination, float>();
            foreach (Destination destination in m_ScoreBook.Keys) {
                float score = GetScore(destination);
                newScoreBook.Add(destination, score);
                currentScore += score;
            }

            Valuename = "Score";
            Value = currentScore;
            m_ScoreBook = newScoreBook;

        }

        private float GetScore(Destination destination) {

            // Get the minimum distance between the destination and the shuttle path.
            Vector2 minDisplacement = m_MainShuttle.Path[0] - (Vector2)destination.transform.position;
            for (int i = 1; i < m_MainShuttle.Path.Count; i++) {
                Vector2 displacement = m_MainShuttle.Path[i] - (Vector2)destination.transform.position;
                minDisplacement = displacement.sqrMagnitude < minDisplacement.sqrMagnitude ? displacement : minDisplacement;
            }

            // Evaluate the score.
            float minDistance = minDisplacement.magnitude;
            float distanceRatio = destination.Radius.Ratio(minDistance);
            float score = destination.Score.Evaluate(1f - distanceRatio);
            destination.Value = score;

            Color col = new Color(0f, 0.25f + 0.75f * score / destination.Score.Max, 0f, 1f);
            Debug.DrawLine(destination.transform.position, destination.transform.position + (Vector3)minDisplacement, col);
            return score;
        }

        void OnDrawGizmos() {
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            foreach (KeyValuePair<Destination, float> item in m_ScoreBook) {
                Gizmos.DrawLine(transform.position, item.Key.transform.position);
            }
        }

    }

}
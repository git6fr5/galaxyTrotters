/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Objects;

namespace Galaxy.Objects {

    /// <summary>
    /// Tracks the path a shuttle leaving from a station would take.
    /// The path of a shuttle is affected by effects.
    /// </summary>
    public class Shuttle : GalaxyObject {

        #region Fields.

        /* --- Static Variables --- */

        // The distance a shuttle takes in one step.
        public static float DefaultStepDistance = 1f / 16f; 

        /* --- Member Variables --- */
        [Space(2), Header("Shuttle Variables")]

        // The distance that the shuttle can cover on its own fuel.
        [SerializeField] 
        public float MaxLength = 16f;

        // The list of positions along this shuttles path.
        [SerializeField, ReadOnly] 
        private List<Vector2> m_Path = new List<Vector2>();
        public List<Vector2> Path => m_Path;

        // Whether this shuttle has reached an end station.
        [SerializeField, ReadOnly] 
        private bool m_ReachedStation = false;
        public bool ReachedStation => m_ReachedStation;
        
        // The indexes in the list of positions on the path that consume the shuttles fuel.
        [SerializeField, ReadOnly] 
        private List<int> m_FuelIndices = new List<int>();
        
        #endregion

        void FixedUpdate() {
            GetPath();
        }

        // Get the path for this frame.
        private void GetPath() {

            // Get the things that could effect the shuttles path.
            Station station = (Station)Game.Get<Station>();
            Force[] forces = (Force[])Game.GetAll<Force>();
            Laser[] lasers = (Laser[])Game.GetAll<Laser>();
            Teleporter[] teleporters = (Teleporter[])Game.GetAll<Teleporter>();

            // Get the path parameters.
            float steps = (int)Mathf.Floor(MaxLength / DefaultStepDistance);
            Vector2 direction = Direction;
            Vector2 origin = transform.position;
            m_FuelIndices = new List<int>();

            // Calculate the positions for the path.
            m_Path = new List<Vector2>();
            m_Path.Add(origin);
            for (int i = 1; i < steps; i++) {
                m_Path.Add(m_Path[m_Path.Count - 1] + direction.normalized * DefaultStepDistance);
                Check<Force>(forces, ref m_Path);
                Check<Laser>(lasers, ref m_Path);
                Check<Teleporter>(teleporters, ref m_Path);
                m_FuelIndices.Add(m_Path.Count - 1);
                direction = m_Path[m_Path.Count - 1] - m_Path[m_Path.Count - 2];
                m_ReachedStation = station != null && station.Check(m_Path[m_Path.Count - 1]);
                if (m_ReachedStation) {
                    break;
                }
            }

        }

        /* --- Checks --- */
        private void Check<TEffect>(TEffect[] effects, ref List<Vector2> positions) where TEffect : IEffect {
            for (int i = 0; i < effects.Length; i++) {
                if (effects[i].Check(positions[positions.Count - 1])) {
                    effects[i].Apply(ref positions, DefaultStepDistance);
                }
            }
        }

        protected override void Debug() {
            if (m_Path == null) { return; }

            // Draw the shuttle path.
            for (int i = 1; i < m_Path.Count; i++) {

                // Get the color of the path at this point.
                if (m_FuelIndices.Contains(i - 1)) {
                    if (m_ReachedStation) {
                        Gizmos.color = Color.green;
                    }
                    else {
                        Gizmos.color = Color.red;
                    }
                }
                else {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawLine(m_Path[i - 1], m_Path[i]);

            }
            
        }

    }

}


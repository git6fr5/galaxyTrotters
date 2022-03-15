/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Shuttle))]
public class Tourist : MonoBehaviour {

    /* --- HideInInspector --- */
    [HideInInspector] public Shuttle shuttle;

    /* --- Properties --- */
    [SerializeField, ReadOnly] public int m_Score = 0;
    [SerializeField, ReadOnly] public Dictionary<Destination, int> m_ScoreBook = new Dictionary<Destination, int>(); 

    //* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        Init();
    }

    // Runs once every frame.
    void Update() {
        GetScores();
    }

    /* --- Methods --- */
    // Initializes this script.
    public void Init() {
        // Set up the score book.
        Destination[] destinations = (Destination[])GameRules.GetAll<Destination>();
        for (int i = 0; i < destinations.Length; i++) {
            m_ScoreBook.Add(destinations[i], 0);
        }
        // Caching components.
        shuttle = GetComponent<Shuttle>();
    }

    private void GetScores() {
        // Update the score book.
        m_Score = 0;
        Dictionary<Destination, int> newScoreBook = new Dictionary<Destination, int>();
        foreach (Destination destination in m_ScoreBook.Keys) {
            int score = GetScore(destination);
            newScoreBook.Add(destination, score);
            m_Score += score;
        }
        m_ScoreBook = newScoreBook;
    }

    private int GetScore(Destination destination) {
        // Get the minimum distance.
        Vector2 minDisplacement = shuttle.Positions[0];
        for (int i = 1; i < shuttle.Positions.Count; i++) {
            Vector2 displacement = shuttle.Positions[i] - (Vector2)destination.transform.position;
            minDisplacement = displacement.sqrMagnitude < minDisplacement.sqrMagnitude ? displacement : minDisplacement;
        }

        Debug.DrawLine(destination.transform.position, destination.transform.position + (Vector3)minDisplacement);
        if (minDisplacement.sqrMagnitude <= destination.MinRadius * destination.MinRadius) {
            return destination.MaxScore;
        }
        else if (minDisplacement.sqrMagnitude > destination.MaxRadius * destination.MaxRadius) {
            return 0;
        }
        float evalDistance = 1 - ((minDisplacement.magnitude - destination.MinRadius) / (destination.MaxRadius - destination.MinRadius));
        return (int)Mathf.Floor(evalDistance * destination.MaxScore);
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the force position.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.75f);
        // Draw the shuttle path.
        Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
        foreach (KeyValuePair<Destination, int> item in m_ScoreBook) {
            Gizmos.DrawLine(transform.position, item.Key.transform.position);
        }
    }

}

/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Shuttle : MonoBehaviour {

    /* --- Static --- */
    public static float StepDistance;

    /* --- Parameters --- */
    [Space(2), Header("Parameters")]
    [SerializeField] private Vector2 m_LaunchDirection = new Vector2(1f, 0f); // The direction this shuttle launches in.
    [SerializeField] public float m_StepDistance = 1f / 16f; // The distance this shuttle takes in one step.
    [SerializeField] public float m_MaxLength = 16f; // The total distance this shuttle travels.
    [SerializeField] public int m_RotationPrecision = 18; // The increment precision this rotates with.
    [SerializeField] public float m_InputDelay = 0.25f; // The delay between processing inputs.

    /* --- Properties --- */
    [Space(2), Header("Properties")]
    [SerializeField, ReadOnly] private KeyCode m_CounterClockwiseKey = KeyCode.J; // The key to rotate the shuttle launch counter clock-wise.
    [SerializeField, ReadOnly] private KeyCode m_ClockwiseKey = KeyCode.K; // The key to rotate the shuttle launch clock-wise.
    [SerializeField, ReadOnly] private List<Vector2> m_Positions = new List<Vector2>(); // The positions along this shuttles path.
    [SerializeField, ReadOnly] private List<int> m_FuelIndices = new List<int>(); // The positions along this shuttles path.
    [Space(2), Header("Switches")]
    [SerializeField, ReadOnly] private bool reachedStation = false; // If the shuttle has reached the station.
    [SerializeField, ReadOnly] private bool isRotatingCounterClockwise = false; // If the shuttle is being rotated counter clockwise.
    [SerializeField, ReadOnly] private bool isRotatingClockwise = false; // If the shuttle is being rotated clockwise.
    [Space(2), Header("Ticks")]
    [SerializeField, ReadOnly] private float counterClockwiseTicks = 0f; // The ticks until the next counter clockwise input is processed.
    [SerializeField, ReadOnly] private float clockwiseTicks = 0f; // The ticks until the next clockwise input is processed.

    /* --- Callbacks --- */
    public List<Vector2> Positions => m_Positions; // Exposes the positions.

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        Init();
    }

    // Runs once every frame.
    void Update() {
        float deltaTime = Time.deltaTime;
        GetInput();
        ProcessInput(deltaTime);
    }

    void FixedUpdate() {
        GetPath();
    }

    /* --- Methods --- */
    // Initializes this script.
    public void Init() {
        StepDistance = m_StepDistance;
    }

    // Get the input for this frame.
    private void GetInput() {
        // Get the key down events.
        isRotatingCounterClockwise = isRotatingCounterClockwise ? true : Input.GetKeyDown(m_CounterClockwiseKey);
        isRotatingClockwise = isRotatingClockwise ? true : Input.GetKeyDown(m_ClockwiseKey);
        // Get the key up events.
        isRotatingCounterClockwise = isRotatingCounterClockwise ? !Input.GetKeyUp(m_CounterClockwiseKey) : false;
        isRotatingClockwise = isRotatingClockwise ? !Input.GetKeyUp(m_ClockwiseKey) : false;
    }

    // Get the path for this frame.
    private void GetPath() {
        // Get the objects.
        Force[] forces = (Force[])GameRules.GetAll<Force>();
        Laser[] lasers = (Laser[])GameRules.GetAll<Laser>();
        Teleporter[] teleporters = (Teleporter[])GameRules.GetAll<Teleporter>();
        Station station = (Station)GameRules.Get<Station>();
        // Get the path parameters.
        float steps = (int)Mathf.Floor(m_MaxLength / m_StepDistance);
        Vector2 direction = m_LaunchDirection;
        Vector2 origin = transform.position;
        m_FuelIndices = new List<int>();
        // Calculate the positions for the path.
        m_Positions = new List<Vector2>();
        m_Positions.Add(origin);
        for (int i = 1; i < steps; i++) {
            m_Positions.Add(m_Positions[m_Positions.Count - 1] + direction.normalized * m_StepDistance);
            Check<Force>(forces, ref m_Positions);
            Check<Laser>(lasers, ref m_Positions);
            Check<Teleporter>(teleporters, ref m_Positions);
            m_FuelIndices.Add(m_Positions.Count - 1);
            direction = m_Positions[m_Positions.Count - 1] - m_Positions[m_Positions.Count - 2];
            reachedStation = station != null && station.Check(m_Positions[m_Positions.Count - 1]);
            if (reachedStation) {
                break;
            }
        }

    }

    // Processes the input for this frame.
    private void ProcessInput(float deltaTime) {
        // Increment the ticks.
        counterClockwiseTicks -= deltaTime;
        clockwiseTicks -= deltaTime;
        // Rotate the launch direction.
        float angle = 360f / m_RotationPrecision;
        if (isRotatingCounterClockwise && counterClockwiseTicks <= 0f) {
            counterClockwiseTicks = m_InputDelay;
            RotateLaunch(angle);
        }
        if (isRotatingClockwise && clockwiseTicks <= 0f) {
            clockwiseTicks = m_InputDelay;
            RotateLaunch(-angle);
        }
        // Reset the ticks.
        if (!isRotatingCounterClockwise) { counterClockwiseTicks = 0f; }
        if (!isRotatingClockwise) { clockwiseTicks = 0f; }

    }

    // Rotate the launch direction by a give angle.
    private void RotateLaunch(float angle) {
        m_LaunchDirection = Quaternion.Euler(0f, 0f, angle) * m_LaunchDirection.normalized;
    }

    /* --- Checks --- */
    private void Check<TEffect>(TEffect[] effects, ref List<Vector2> positions) where TEffect : IEffect {
        for (int i = 0; i < effects.Length; i++) {
            if (effects[i].Check(positions[positions.Count - 1])) {
                effects[i].Apply(ref positions);
            }
        }
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the shuttle position.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Draw the shuttle path.
        if (m_Positions != null) {
            for (int i = 1; i < m_Positions.Count; i++) {
                Gizmos.color = m_FuelIndices.Contains(i - 1) ? (reachedStation ? Color.green : Color.red) : Color.blue;
                Gizmos.DrawLine(m_Positions[i - 1], m_Positions[i]);
            }
        }

    }

}

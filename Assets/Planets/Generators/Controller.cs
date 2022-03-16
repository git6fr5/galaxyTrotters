/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Controller : MonoBehaviour {

    /* --- Fields --- */
    #region Fields

    // Components.
    [HideInInspector] public Rigidbody body;

    // Movement.
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float resistance;

    // Controls.
    private Vector3 targetDirection;
    [SerializeField] private float right;
    [SerializeField] private float forward;

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start() {
        Init();
    }

    void Update() {
        GetInput();
    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Move(deltaTime);
    }

    #endregion

    /* --- Initialization --- */
    #region Initializtion

    private void Init() {
        // Caching.
        body = GetComponent<Rigidbody>();
    }

    #endregion

    private void GetInput() {
        right = Input.GetAxisRaw("Horizontal");
        forward = Input.GetAxisRaw("Vertical");
    }

    private void Move(float deltaTime) {
        targetDirection = (right * transform.right + forward * transform.forward).normalized;
        body.AddForce(targetDirection * acceleration * body.mass);

        float rightMag = Vector3.Dot(body.velocity, transform.right);
        float forwardMag = Vector3.Dot(body.velocity, transform.forward);
        float upMag = Vector3.Dot(body.velocity, transform.up);

        float sqrMoveSpeed = rightMag * rightMag + forwardMag * forwardMag;
        if (sqrMoveSpeed > speed * speed) {
            Vector3 clampedVelocity = speed * (rightMag * transform.right + forwardMag * transform.forward).normalized;
            body.velocity = clampedVelocity + upMag * transform.up;
        }

        if (targetDirection == Vector3.zero) {
            Vector3 resistanceVelocity = resistance * (rightMag * transform.right + forwardMag * transform.forward);
            body.velocity = resistanceVelocity + upMag * transform.up;
        }

    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.up);
    }

}

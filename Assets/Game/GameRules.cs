/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 
/// </summary>
public class GameRules : MonoBehaviour {

    /* --- Components --- */
    [SerializeField] public static Camera MainCamera; // The main camera.
    [SerializeField] public static Vector2 MousePosition => (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition);
    [SerializeField] public PixelPerfectCamera m_PixelPerfectCamera; // The pixel perfect camera, attached to the main camera.

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        MainCamera = Camera.main;
    }

    // Runs once every frame.
    void Update() {

    }

    public static object Get<T>() {
        return GameObject.FindObjectOfType(typeof(T));
    }
    public static object[] GetAll<T>() {
        return GameObject.FindObjectsOfType(typeof(T));
    }

    /* --- Debug --- */
    void OnDrawGizmos() {
        // Draw the camera bounds.
        if (m_PixelPerfectCamera != null) {
            Gizmos.color = Color.green;
            float width = (float)m_PixelPerfectCamera.refResolutionX / (float)m_PixelPerfectCamera.assetsPPU;
            float height = (float)m_PixelPerfectCamera.refResolutionY / (float)m_PixelPerfectCamera.assetsPPU;
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
        }

    }

}

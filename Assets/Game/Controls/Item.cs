/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class Item : MonoBehaviour {

    /* --- Parameters --- */
    [SerializeField] public string Name;
    [SerializeField] public int Value;
    
    /* --- Properties --- */
    [Space(2), Header("Switches")]
    [SerializeField, ReadOnly] private bool isSelected = false;
    [SerializeField, ReadOnly] private bool isMouseOver = false;

    //* --- Unity --- */
    void Start() {
        Init();
    }

    // Runs once every frame.
    void Update() {
        GetInput();
        float deltaTime = Time.deltaTime;
        ProcessInput(deltaTime);
    }

    // Runs once before the first frame.
    void OnMouseEnter() {
        isMouseOver = true;
    }

    // Runs once before the first frame.
    void OnMouseExit() {
        isMouseOver = false;
    }

    /* --- Methods --- */
    public void Init() {
        //  GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void GetInput() {
        if (isMouseOver && Input.GetMouseButtonDown(0)) {
            isSelected = true;
        }
        else if (Input.GetMouseButtonUp(0)) {
            isSelected = false;
        }

        if (isMouseOver && Input.GetMouseButtonDown(1)) {
            Destroy(gameObject);
        }
    }

    private void ProcessInput(float deltaTime) {
        if (isSelected) { transform.position = GameRules.MousePosition; }
    }
}

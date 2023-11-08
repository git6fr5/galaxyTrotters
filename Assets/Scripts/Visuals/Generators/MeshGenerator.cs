/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Galaxy.Models;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

    public bool regenerate = false;

    public UnityEvent<MeshGenerator> generationEvent; 

    // Mesh.
    [HideInInspector] 
    public MeshRenderer meshRenderer;
    
    [HideInInspector] 
    public MeshFilter meshFilter;
    
    [HideInInspector] 
    public MeshCollider meshCollider;
    
    [HideInInspector]
    public MeshSettings meshSettings = new MeshSettings();
    
    void Start() {
        Init();
        Generate();
    }

    void Update() {
        if (regenerate) {
            Generate();
            // regenerate = false;
        }
    }

    public void Init() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Generate() {
        generationEvent.Invoke(this);
        RenderToMesh(meshSettings);
    }

    private void RenderToMesh(MeshSettings meshSettings) {
        if (meshFilter.mesh = null) { meshFilter.mesh = new Mesh(); }
        
        // meshFilter.mesh.MarkDynamic();
        // meshSettings.ProcessHeights();
        meshFilter.mesh.SetVertices(meshSettings.positions);
        meshFilter.mesh.SetIndices(meshSettings.indices, meshSettings.topology, 0);
        // meshFilter.mesh.colors = tempSettings.colors;
        meshFilter.mesh.RecalculateNormals();
    }

}

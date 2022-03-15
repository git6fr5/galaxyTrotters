/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneGenerator : MonoBehaviour {

    /* --- Data --- */
    #region Data   

    [System.Serializable]
    public struct PlaneSettings {
        public float width;
        public float height;
        public int subdivisions;
        public MeshTopology topology;

        public PlaneSettings(float width, float height, int subdivisions, MeshTopology topology) {
            this.width = width;
            this.height = height;
            this.subdivisions = subdivisions;
            this.topology = topology;
        }
    }

    public struct MeshSettings {
        public Vector3[] positions;
        public int[] indices;
        public Color[] colors;
        public MeshTopology topology;

        public MeshSettings(List<Vector3> positions, List<int> indices, List<Color> colors, MeshTopology topology) {
            this.positions = positions.ToArray();
            this.indices = indices.ToArray();
            this.colors = colors.ToArray();
            this.topology = topology;
        }
    }

    #endregion

    /* --- Fields --- */
    #region Fields

    // Values.
    private static float PI = Mathf.PI;
    private static float PHI = (1f + Mathf.Sqrt(5)) / 2f;

    // Settings.
    [SerializeField] private PlaneSettings planeSettings;

    // Mesh.
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public MeshFilter meshFilter;
    private MeshSettings meshSettings;

    // Switches.
    public bool construct = false;
    public bool animatedConstruction = false;
    public bool resetRotation = false;

    // Animation.
    [SerializeField] private float animationRatio = 0f;
    [SerializeField] private float animationSpeed = 1f;
    public AnimationCurve verticesAnimationCurve;
    public AnimationCurve radiusAnimationCurve;

    // Rotation.
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float rotationSpeed;

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start() {
        Init();
    }

    void Update() {

        if (animatedConstruction) {
            animationRatio += Time.deltaTime * animationSpeed;
            float verticesRatio = verticesAnimationCurve.Evaluate(animationRatio);
            float radiusRatio = radiusAnimationCurve.Evaluate(animationRatio);

            PlaneSettings currSettings = new PlaneSettings(planeSettings.radius * radiusRatio, (int)(planeSettings.vertices * verticesRatio), planeSettings.topology);
            meshSettings = Generate(currSettings);
            RenderToMesh(meshSettings);
            if (animationRatio >= 1f) {
                animationRatio = 0f;
                animatedConstruction = false;
            }
        }

        if (construct) {
            meshSettings = Construct(planeSettings);
            RenderToMesh(meshSettings);
            construct = false;
        }

        if (resetRotation) {
            ResetRotation();
            resetRotation = false;
        }

    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Rotate(deltaTime);
    }

    #endregion

    #region Initialization

    public void Init() {
        // Caching.
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    #endregion

    #region Construction
    
    private void Construct(PlaneSettings planeSettings) {
        List<Vector3> points = new List<Vector3>();

        points.Add(new Vector3(planeSettings.width, planeSettings.height));
        points.Add(new Vector3(planeSettings.width, -planeSettings.height));
        points.Add(new Vector3(-planeSettings.width, planeSettings.height));
        points.Add(new Vector3(-planeSettings.width, -planeSettings.height));

    }

    #endregion

    #region Rendering

    private void RenderToMesh(MeshSettings meshSettings) {
        meshFilter.mesh = new Mesh();
        // meshFilter.mesh.MarkDynamic();

        meshFilter.mesh.SetVertices(meshSettings.positions);
        meshFilter.mesh.SetIndices(meshSettings.indices, meshSettings.topology, 0);
        meshFilter.mesh.colors = meshSettings.colors;
    }

    private void Rotate(float deltaTime) {
        transform.eulerAngles += rotationAxis * rotationSpeed * deltaTime;
    }

    private void ResetRotation() {
        transform.eulerAngles = Vector3.zero;
    }

    #endregion

}

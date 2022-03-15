/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SphereGenerator : MonoBehaviour {

    /* --- Data --- */
    #region Data   

    [System.Serializable]
    public struct SphereSettings {
        public float radius;
        public int vertices;
        public MeshTopology topology;

        public SphereSettings(float radius, int vertices, MeshTopology topology) {
            this.radius = radius;
            this.vertices = vertices;
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
    [SerializeField] private SphereSettings sphereSettings;

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

            SphereSettings currSettings = new SphereSettings(sphereSettings.radius * radiusRatio, (int)(sphereSettings.vertices * verticesRatio), sphereSettings.topology);
            meshSettings = Fibonnaci(currSettings);
            RenderToMesh(meshSettings);
            if (animationRatio >= 1f) {
                animationRatio = 0f;
                animatedConstruction = false;
            }
        }

        if (construct) {
            meshSettings = Fibonnaci(sphereSettings);
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

    private MeshSettings Ico(SphereSettings sphereSettings) {
        List<Vector3> points = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < sphereSettings.vertices; i++) {

            CreateRectangle(PHI, 1f);

        }

        switch (sphereSettings.topology) {
            case MeshTopology.Triangles:
                for (int i = 2; i < sphereSettings.vertices; i++) {
                    indices.Add(i - 2);
                    indices.Add(i - 1);
                    indices.Add(i);
                }
                break;
            default:
                for (int i = 0; i < sphereSettings.vertices; i++) {
                    indices.Add(i);
                }
                break;
        }

        return new MeshSettings(points, indices, colors, sphereSettings.topology);
    }

    private void CreateRectangle(float width, float height) {
        List<Vector3> points = new List<Vector3>();

        pointA = new Vector3(width, height);
        pointB = new Vector3(width, height);
        pointA = new Vector3(width, height);
        pointA = new Vector3(width, height);

    }

    private MeshSettings Fibonnaci(SphereSettings sphereSettings) {
        List<Vector3> points = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < sphereSettings.vertices; i++) {
            float angleA = Mathf.Acos(1f - 2f * (float)i / sphereSettings.vertices);
            float angleB = 2 * PI * PHI * i;
            Vector3 point = sphereSettings.radius * new Vector3(Mathf.Sin(angleA) * Mathf.Cos(angleB), Mathf.Sin(angleA) * Mathf.Sin(angleB), Mathf.Cos(angleA));
            points.Add(point);
            colors.Add(i % 3 == 0 ? Color.red : (i % 3 == 1 ? Color.blue : Color.green));
        }

        switch (sphereSettings.topology) {
            case MeshTopology.Triangles:
                for (int i = 2; i < sphereSettings.vertices; i++) {
                    indices.Add(i - 2);
                    indices.Add(i - 1);
                    indices.Add(i);
                }
                break;
            default:
                for (int i = 0; i < sphereSettings.vertices; i++) {
                    indices.Add(i);
                }
                break;
        }

        return new MeshSettings(points, indices, colors, sphereSettings.topology);
    }

    private MeshSettings Concavity(SphereSettings sphereSettings) {
        List<Vector3> points = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < sphereSettings.vertices; i++) {
            float angleA = Mathf.Acos(1f - 2f * (float)i / sphereSettings.vertices);
            float angleB = 2 * PI * PHI * i;
            Vector3 point = sphereSettings.radius * new Vector3(Mathf.Sin(angleA) * Mathf.Cos(angleB), Mathf.Sin(angleA) * Mathf.Sin(angleA), Mathf.Cos(angleA));
            points.Add(point);
            indices.Add(i);
            colors.Add(Color.white);
        }

        return new MeshSettings(points, indices, colors, sphereSettings.topology);
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

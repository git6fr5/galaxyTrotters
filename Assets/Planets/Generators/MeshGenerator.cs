/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

    /* --- Data --- */
    #region Data   

    public class Vertex {
        public Vector3 position;
        public Color color;
        public int index;

        public Vertex(float x, float y, float z, int index = 0) {
            this.position = new Vector3(x, y, z);
            this.color = Color.white;
            this.index = index;
        }

        public Vertex(float x, float y, float z, Color color, int index = 0) {
            this.position = new Vector3(x, y, z);
            this.color = color;
            this.index = index;
        }
    }

    public class Triangle {
        public Vertex[] vertices;

        public Triangle(Vertex vertexA, Vertex vertexB, Vertex vertexC) {
            this.vertices = new Vertex[3];
            this.vertices[0] = vertexA;
            this.vertices[1] = vertexB;
            this.vertices[2] = vertexC;
        }
    }

    public class Craters {
        public Vector3[] origins;
        public float[] radii;
        public float[] depths;

        public Craters(Vector3[] origins, float[] radii, float[] depths) {
            this.origins = origins;
            this.radii = radii;
            this.depths = depths;
        }
    }

    public class MeshSettings {
        public Vector3[] positions;
        public int[] indices;
        public Color[] colors;
        public MeshTopology topology;
        public float[] heights;

        public Vector3 origin;

        public static MeshSettings operator +(MeshSettings a, MeshSettings b) => Addition(a, b);
        public static MeshSettings operator +(MeshSettings a, int[] indices) => AddIndices(a, indices);
        public static MeshSettings operator *(MeshSettings a, Vector3 v) => Rotate(a, v);
        public static MeshSettings operator *(MeshSettings b, float a) => Scale(b, new Vector3(a, a, a));

        public MeshSettings() {
            this.positions = new Vector3[0];
            this.indices = new int[0];
            this.colors = new Color[0];
            this.topology = MeshTopology.Points;
            SetUniformHeight(this.positions);
        }

        public MeshSettings(Vector3[] positions, int[] indices, Color[] colors, MeshTopology topology = MeshTopology.Triangles) {
            this.positions = positions;
            this.indices = indices;
            this.colors = colors;
            this.topology = topology;
            SetUniformHeight(this.positions);
        }

        public MeshSettings(Vector3[] positions, List<int> indices, Color[] colors, MeshTopology topology = MeshTopology.Triangles) {
            this.positions = positions;
            this.indices = indices.ToArray();
            this.colors = colors;
            this.topology = topology;
            SetUniformHeight(this.positions);
        }

        public MeshSettings(List<Vector3> positions, List<int> indices, List<Color> colors, MeshTopology topology = MeshTopology.Triangles  ) {
            this.positions = positions.ToArray();
            this.indices = indices.ToArray();
            this.colors = colors.ToArray();
            this.topology = topology;
            SetUniformHeight(this.positions);
        }

        public void SetUniformHeight(Vector3[] positions, float height = 1f) {
            this.heights = new float[positions.Length];
            for (int i = 0; i < heights.Length; i++) {
                heights[i] = height; // 1f + 0.05f * Mathf.Sin(positions[i].y);
            }
        }

        public void SetColor(Color color) {
            for (int i = 0; i < colors.Length; i++) {
                colors[i] = color;
            }
        }

        public void SetPositions(Vector4[] v4) {
            for (int i = 0; i < Mathf.Min(v4.Length, positions.Length); i++) {
                positions[i] = (Vector3)v4[i];
            }
        }

        public void ProcessHeights() {
            for (int i = 0; i < positions.Length; i++) {
                print(heights[i]);
                positions[i] *= heights[i];
            }
        }

        public MeshSettings Duplicate() {
            Vector3[] newPositions = new Vector3[positions.Length];
            positions.CopyTo(newPositions, 0);
            return new MeshSettings(newPositions, indices, colors, topology);
        }

        public static MeshSettings Addition(MeshSettings a, MeshSettings b) {
            
            Vector3[] newPositions = new Vector3[a.positions.Length + b.positions.Length];
            a.positions.CopyTo(newPositions, 0);
            b.positions.CopyTo(newPositions, a.positions.Length);

            int[] newIndices = new int[a.indices.Length + b.indices.Length];
            a.indices.CopyTo(newIndices, 0);
            for (int i = 0; i < b.indices.Length; i++) {
                newIndices[a.indices.Length + i] = b.indices[i] + a.positions.Length;
            }

            Color[] newColors = new Color[a.colors.Length + b.colors.Length];
            a.colors.CopyTo(newColors, 0);
            b.colors.CopyTo(newColors, a.colors.Length);

            return new MeshSettings(newPositions, newIndices, newColors, a.topology);

        }

        public static MeshSettings AddIndices(MeshSettings a, int[] indices) {
            int[] newIndices = new int[a.indices.Length + indices.Length];
            a.indices.CopyTo(newIndices, 0);
            indices.CopyTo(newIndices, a.indices.Length);
            return new MeshSettings(a.positions, newIndices, a.colors, a.topology);
        }

        public static MeshSettings Rotate(MeshSettings a, Vector3 v) {
            Vector3[] newPositions = new Vector3[a.positions.Length];
            for (int i = 0; i < a.positions.Length; i++) {
                newPositions[i] = Quaternion.Euler(v.x, v.y, v.z) * a.positions[i];
            }
            return new MeshSettings(newPositions, a.indices, a.colors, a.topology);

        }

        public static MeshSettings Scale(MeshSettings a, Vector3 v) {
            Vector3[] newPositions = new Vector3[a.positions.Length];
            for (int i = 0; i < a.positions.Length; i++) {
                newPositions[i] = new Vector3(v.x * a.positions[i].x, v.y * a.positions[i].y, v.z * a.positions[i].z);
            }
            return new MeshSettings(newPositions, a.indices, a.colors, a.topology);

        }

        public static MeshSettings Spherify(MeshSettings a, float radius) {
            Vector3[] newPositions = new Vector3[a.positions.Length];
            for (int i = 0; i < a.positions.Length; i++) {
                newPositions[i] = a.positions[i].normalized * radius;
            }
            return new MeshSettings(newPositions, a.indices, a.colors, a.topology);
        }

        public static MeshSettings Spherify(MeshSettings a, float radius, Vector3 origin) {
            Vector3[] newPositions = new Vector3[a.positions.Length];
            for (int i = 0; i < a.positions.Length; i++) {
                newPositions[i] = (a.positions[i]-origin).normalized * radius + origin;
            }
            return new MeshSettings(newPositions, a.indices, a.colors, a.topology);
        }

    }

    #endregion

    /* --- Fields --- */
    #region Fields

    // Values.
    public static float PI = Mathf.PI;
    public static float PHI = (1f + Mathf.Sqrt(5)) / 2f;

    // Mesh.
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public MeshFilter meshFilter;
    [HideInInspector] public MeshCollider meshCollider;
    private MeshSettings meshSettings;

    // Switches.
    public bool autoconstruct = false;
    public bool construct = false;
    public bool render = false;
    public bool useshaders = false;

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start() {
        Init();
    }

    void Update() {

        if (autoconstruct) {
            construct = true;
        }

        if (construct) {
            meshSettings = Generate();
            construct = false;
        }

        if (render) {
            RenderToMesh(meshSettings);
        }

        // meshCollider.sharedMesh = meshFilter.mesh;

    }

    #endregion

    #region Initialization

    public void Init() {
        // Caching.
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    #endregion

    public void SetSettings(MeshSettings meshSettings) {
        this.meshSettings = meshSettings;
    }

    #region Generation

    protected virtual MeshSettings Generate() {
        return new MeshSettings();
    }

    #endregion

    #region Rendering

    private void RenderToMesh(MeshSettings meshSettings) {
        meshFilter.mesh = new Mesh();
        // meshFilter.mesh.MarkDynamic();
        MeshSettings tempSettings = meshSettings.Duplicate();

        // We shouldn't need to re-render the mesh just to change the height values.
        if (useshaders) {
            ComputeShaders(ref tempSettings);
        }
        // meshSettings.ProcessHeights();

        meshFilter.mesh.SetVertices(tempSettings.positions);
        meshFilter.mesh.SetIndices(tempSettings.indices, tempSettings.topology, 0);
        // meshFilter.mesh.colors = tempSettings.colors;
        meshFilter.mesh.RecalculateNormals();
    }

    protected virtual void ComputeShaders(ref MeshSettings meshSettings) {

    }

    #endregion

}

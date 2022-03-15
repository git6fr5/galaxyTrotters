/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vertex = MeshGenerator.Vertex;
using Triangle = MeshGenerator.Triangle;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneGenerator : MeshGenerator {

    /* --- Data --- */
    #region Data   

    [System.Serializable]
    public class PlaneSettings {
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

    #endregion

    /* --- Fields --- */
    #region Fields

    // Settings.
    [SerializeField] private PlaneSettings planeSettings;

    #endregion

    /* --- Construction --- */
    #region Construction

    protected override MeshSettings Generate() {
        return Construct(planeSettings);
    }

    public MeshSettings Construct(float width, float height, int subdivisions = 1, MeshTopology topology = MeshTopology.Triangles) {
        PlaneSettings planeSettings = new PlaneSettings(width, height, subdivisions, topology);
        return Construct(planeSettings);
    }

    public MeshSettings Construct(PlaneSettings planeSettings) {
        List<int> indices = new List<int>();

        Vertex[,] vertices = new Vertex[planeSettings.subdivisions + 1, planeSettings.subdivisions + 1];

        float heightIncrement = planeSettings.height / planeSettings.subdivisions;
        float heightOffset = -planeSettings.height / 2f;
        float widthIncrement = planeSettings.width / planeSettings.subdivisions;
        float widthOffset = -planeSettings.width / 2f;

        for (int i = 0; i <= planeSettings.subdivisions; i++) {
            float y = i * heightIncrement + heightOffset;
            for (int j = 0; j <= planeSettings.subdivisions; j++) {
                float x = j * widthIncrement + widthOffset;
                Color color = new Color(i % 2 == 0 && j % 2 == 0 ? 1f : 0f, i % 2 == 1 && j % 2 == 0 ? 1f : 0f, i % 2 == 1 && j % 2 == 1 ? 1f : 0f, 1f);
                if (i % 2 == 0 && j % 2 == 1) {
                    color = Color.yellow;
                }
                vertices[i, j] = new Vertex(x, y, 0, color, i * (planeSettings.subdivisions + 1) + j);
            }
        }

        List<Triangle> triangles = new List<Triangle>();
        for (int i = 0; i < planeSettings.subdivisions; i++) {
            for (int j = 0; j < planeSettings.subdivisions; j++) {
                triangles.Add(new Triangle(vertices[i, j], vertices[i + 1, j], vertices[i, j + 1]));
                triangles.Add(new Triangle(vertices[i, j], vertices[i, j + 1], vertices[i + 1, j]));

                triangles.Add(new Triangle(vertices[i + 1, j + 1], vertices[i, j + 1], vertices[i + 1, j]));
                triangles.Add(new Triangle(vertices[i + 1, j + 1], vertices[i + 1, j], vertices[i, j + 1]));
            }
        }

        Vector3[] points = new Vector3[(planeSettings.subdivisions + 1) * (planeSettings.subdivisions + 1)];
        Color[] colors = new Color[(planeSettings.subdivisions + 1) * (planeSettings.subdivisions + 1)];
        for (int i = 0; i <= planeSettings.subdivisions; i++) {
            for (int j = 0; j <= planeSettings.subdivisions; j++) {
                points[vertices[i, j].index] = vertices[i, j].position;
                colors[vertices[i, j].index] = vertices[i, j].color;
            }
        }

        for (int i = 0; i < triangles.Count; i++) {
            indices.Add(triangles[i].vertices[0].index);
            indices.Add(triangles[i].vertices[1].index);
            indices.Add(triangles[i].vertices[2].index);
        }

        return new MeshSettings(points, indices, colors, MeshTopology.Triangles);

    }

    #endregion

}

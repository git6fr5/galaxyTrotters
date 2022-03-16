/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vertex = MeshGenerator.Vertex;
using Triangle = MeshGenerator.Triangle;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CircleGenerator : MeshGenerator {

    /* --- Data --- */
    #region Data   

    [System.Serializable]
    public class SphereSettings {
        public float radius;
        public int subdivisions;
        public MeshTopology topology;

        public SphereSettings(float radius, int subdivisions, MeshTopology topology) {
            this.radius = radius;
            this.subdivisions = subdivisions;
            this.topology = topology;
        }
    }

    #endregion

    /* --- Fields --- */
    #region Fields

    // Settings.
    [SerializeField] private SphereSettings sphereSettings;
    public SphereSettings Settings => sphereSettings;

    #endregion

    /* --- Construction --- */
    #region Construction

    protected override MeshSettings Generate() {
        return Construct(sphereSettings);
    }

    protected MeshSettings Construct(SphereSettings sphereSettings) {

        List<Vertex> vertices = new List<Vertex>();

        vertices.Add(new Vertex(-1, PHI, 0));
        vertices.Add(new Vertex(1, PHI, 0));
        vertices.Add(new Vertex(-1, -PHI, 0));
        vertices.Add(new Vertex(1, -PHI, 0));

        vertices.Add(new Vertex(0, -1, PHI));
        vertices.Add(new Vertex(0, 1, PHI));
        vertices.Add(new Vertex(0, -1, -PHI));
        vertices.Add(new Vertex(0, 1, -PHI));

        vertices.Add(new Vertex(PHI, 0, -1));
        vertices.Add(new Vertex(PHI, 0, 1));
        vertices.Add(new Vertex(-PHI, 0, -1));
        vertices.Add(new Vertex(-PHI, 0, 1));

        for (int i = 0; i < vertices.Count; i++) {
            vertices[i].index = i;
        }

        // create 20 triangles of the icosahedron
        List<Triangle> triangles = new List<Triangle>();

        // 5 faces around point 0
        triangles.Add(new Triangle(vertices[0], vertices[11], vertices[5]));
        triangles.Add(new Triangle(vertices[0], vertices[5], vertices[1]));
        triangles.Add(new Triangle(vertices[0], vertices[1], vertices[7]));
        triangles.Add(new Triangle(vertices[0], vertices[7], vertices[10]));
        triangles.Add(new Triangle(vertices[0], vertices[10], vertices[11]));

        // pointArray[5] adjacent faces
        triangles.Add(new Triangle(vertices[1], vertices[5], vertices[9]));
        triangles.Add(new Triangle(vertices[5], vertices[11], vertices[4]));
        triangles.Add(new Triangle(vertices[11], vertices[10], vertices[2]));
        triangles.Add(new Triangle(vertices[10], vertices[7], vertices[6]));
        triangles.Add(new Triangle(vertices[7], vertices[1], vertices[8]));

        // 5 faces around point 3
        triangles.Add(new Triangle(vertices[3], vertices[9], vertices[4]));
        triangles.Add(new Triangle(vertices[3], vertices[4], vertices[2]));
        triangles.Add(new Triangle(vertices[3], vertices[2], vertices[6]));
        triangles.Add(new Triangle(vertices[3], vertices[6], vertices[8]));
        triangles.Add(new Triangle(vertices[3], vertices[8], vertices[9]));

        // 5 adjacent faces
        triangles.Add(new Triangle(vertices[4], vertices[9], vertices[5]));
        triangles.Add(new Triangle(vertices[2], vertices[4], vertices[11]));
        triangles.Add(new Triangle(vertices[6], vertices[2], vertices[10]));
        triangles.Add(new Triangle(vertices[8], vertices[6], vertices[7]));
        triangles.Add(new Triangle(vertices[9], vertices[8], vertices[1]));


        // refine triangles
        for (int i = 0; i < sphereSettings.subdivisions; i++) {
            List<Triangle> subdividedTriangles = new List<Triangle>();
            List<Vertex> subdivisionVertices = new List<Vertex>();

            foreach (Triangle triangle in triangles) {

                Vertex[] bisects = new Vertex[3];

                triangle.vertices[0].index = subdivisionVertices.Count;
                subdivisionVertices.Add(triangle.vertices[0]);

                triangle.vertices[1].index = subdivisionVertices.Count;
                subdivisionVertices.Add(triangle.vertices[1]);

                triangle.vertices[2].index = subdivisionVertices.Count;
                subdivisionVertices.Add(triangle.vertices[2]);

                // replace triangle by 4 triangles
                bisects[0] = Bisect(triangle.vertices[0], triangle.vertices[1], ref subdivisionVertices);
                bisects[1] = Bisect(triangle.vertices[1], triangle.vertices[2], ref subdivisionVertices);
                bisects[2] = Bisect(triangle.vertices[2], triangle.vertices[0], ref subdivisionVertices);

                subdividedTriangles.Add(new Triangle(triangle.vertices[0], bisects[0], bisects[2]));
                subdividedTriangles.Add(new Triangle(triangle.vertices[1], bisects[1], bisects[0]));
                subdividedTriangles.Add(new Triangle(triangle.vertices[2], bisects[2], bisects[1]));
                subdividedTriangles.Add(new Triangle(bisects[0], bisects[1], bisects[2]));

            }
            vertices = subdivisionVertices;
            triangles = subdividedTriangles;
        }


        Vector3[] points = new Vector3[vertices.Count];
        Color[] colors = new Color[vertices.Count];
        for (int i = 0; i < vertices.Count; i++) {
            points[vertices[i].index] = vertices[i].position;
            colors[vertices[i].index] = new Color(i % 3 == 0 ? 1f : 0f, i % 3 == 1 ? 1f : 0f, i % 3 == 2 ? 1f : 0f, 1f);
        }

        int[] indices = new int[3 * triangles.Count];
        for (int i = 0; i < triangles.Count; i++) {
            for (int j = 0; j < 3; j++) {
                indices[3 * i + j] = triangles[i].vertices[j].index;
            }
        }

        return MeshSettings.Spherify(new MeshSettings(points, indices, colors), sphereSettings.radius);
    }

    private Vertex Bisect(Vertex v1, Vertex v2, ref List<Vertex> subdivisionCache) {

        Vector3 position = (v1.position + v2.position) / 2f;
        Vertex cachedVertex = subdivisionCache.Find(vert => vert.position == position);
        if (cachedVertex != null) {
            return cachedVertex;
        }
        else {
            Vertex newVertex = new Vertex(position.x, position.y, position.z, (v1.color + v2.color) / 2f, subdivisionCache.Count);
            subdivisionCache.Add(newVertex);
            return newVertex;
        }
    }

    #endregion

}

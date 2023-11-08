/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Models;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeMesh : MonoBehaviour {

    [System.Serializable]
    public class CubeSettings {
        public float length;
        public int subdivisions;
        public MeshTopology topology;
        public Vector3 origin;
    }

    // Settings.
    [SerializeField] private CubeSettings settings;
    public CubeSettings Settings => settings;

    public void Construct(MeshGenerator generator) {

        List<Vertex> vertices = new List<Vertex>();

        float l = settings.length / 2f;
        vertices.Add(new Vertex(-l, l, l)); // 0
        vertices.Add(new Vertex(l, l, l)); // 1
        
        vertices.Add(new Vertex(-l, -l, l)); // 2
        vertices.Add(new Vertex(l, -l, l)); // 3

        vertices.Add(new Vertex(-l, l, -l)); // 4
        vertices.Add(new Vertex(l, l, -l)); // 5
        
        vertices.Add(new Vertex(-l, -l, -l)); // 6
        vertices.Add(new Vertex(l, -l, -l)); // 7

        for (int i = 0; i < vertices.Count; i++) {
            vertices[i].index = i;
        }

        // create 20 triangles of the icosahedron
        List<Triangle> triangles = new List<Triangle>();

        // back face
        triangles.Add(new Triangle(vertices[2], vertices[1], vertices[0]));
        triangles.Add(new Triangle(vertices[1], vertices[2], vertices[3]));

        // front face
        triangles.Add(new Triangle(vertices[0+4], vertices[1+4], vertices[2+4]));
        triangles.Add(new Triangle(vertices[3+4], vertices[2+4], vertices[1+4]));

        // bottom face
        triangles.Add(new Triangle(vertices[6], vertices[3], vertices[2]));
        triangles.Add(new Triangle(vertices[7], vertices[3], vertices[6]));

        // top face
        triangles.Add(new Triangle(vertices[0], vertices[1], vertices[4]));
        triangles.Add(new Triangle(vertices[5], vertices[4], vertices[1]));

        // side face
        triangles.Add(new Triangle(vertices[4], vertices[2], vertices[0]));
        triangles.Add(new Triangle(vertices[2], vertices[4], vertices[6]));

        // side face
        triangles.Add(new Triangle(vertices[1], vertices[3], vertices[5]));
        triangles.Add(new Triangle(vertices[7], vertices[5], vertices[3]));

        // refine triangles
        for (int i = 0; i < settings.subdivisions; i++) {
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
            points[vertices[i].index] = vertices[i].position + settings.origin;
            colors[vertices[i].index] = new Color(i % 3 == 0 ? 1f : 0f, i % 3 == 1 ? 1f : 0f, i % 3 == 2 ? 1f : 0f, 1f);
        }

        int[] indices = new int[3 * triangles.Count];
        for (int i = 0; i < triangles.Count; i++) {
            for (int j = 0; j < 3; j++) {
                indices[3 * i + j] = triangles[i].vertices[j].index;
            }
        }

        generator.meshSettings = new MeshSettings(points, indices, colors);
    }

    // public void CrushIntoSphere(MeshGenerator generator) {
    //     generator.meshSettings = MeshSettings.Spherify(generator.meshSettings, sphereSettings.radius, sphereSettings.origin);
    // }

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

}

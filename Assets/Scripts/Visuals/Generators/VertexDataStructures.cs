/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Models;

namespace Galaxy.Models {

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

}


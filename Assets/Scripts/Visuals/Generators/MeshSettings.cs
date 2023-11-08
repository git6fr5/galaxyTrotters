/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Models;

namespace Galaxy.Models {

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
                Debug.Log(heights[i]);
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

}
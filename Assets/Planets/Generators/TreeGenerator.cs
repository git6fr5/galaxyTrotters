using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour {


    /* --- Data --- */
    #region Data

    public class Sphere {

        public Vector3 origin;
        public float radius;
        public Color color;

        public Sphere(Vector3 origin, float radius, Color color) {
            this.origin = origin;
            this.radius = radius;
            this.color = color;
        }
    }

    #endregion

    /* --- Variables --- */
    #region Variables

    // Colors.
    [Space(2), Header("Colors")]
    [SerializeField] private Color[] leaveColors;

    [SerializeField] private float radius;
    [SerializeField] private float variability;
    [SerializeField] private int count;
    [SerializeField] private int depth = 3;

    [HideInInspector] private Sphere[][] spheres;
    [SerializeField] private SphereGenerator sphereBase;
    [SerializeField] private MeshGenerator meshBase;

    // Switches
    [Space(2), Header("Switches")]
    [SerializeField] private bool getspheres;
    [SerializeField] private bool generate;

    #endregion

    #region Unity

    void Update() {

        if (getspheres) {
            GetSpheres();
            getspheres = false;
        }

        if (generate) {
            GenerateSpheres();
            generate = false;
        }
    }

    #endregion

    /* --- Data Generation --- */
    #region Data Generation

    private void GenerateSpheres() {

        SphereGenerator.SphereSettings sphereSettings = new SphereGenerator.SphereSettings(spheres[0][0].radius, 1, MeshTopology.Triangles);
        MeshGenerator.MeshSettings newMesh = sphereBase.Construct(sphereSettings);

        for (int i = 1; i < spheres.Length; i++) {
            for (int j = 0; j < spheres[i].Length; j++) {
                sphereSettings = new SphereGenerator.SphereSettings(spheres[i][j].radius, 1, MeshTopology.Triangles, spheres[i][j].origin);
                MeshGenerator.MeshSettings settings = sphereBase.Construct(sphereSettings);
                settings.SetColor(spheres[i][j].color);
                newMesh += settings;
            }
        }

        meshBase.SetSettings(newMesh);
        meshBase.render = true;
    }

    private void GetSpheres() {

        spheres = new Sphere[depth][];
        spheres[0] = new Sphere[1];
        spheres[0][0] = new Sphere(Vector3.zero, radius / 2f, leaveColors[0]);

        for (int n = 1; n < depth; n++) {

            spheres[n] = new Sphere[(int)Mathf.Pow(count, n)];
            float radius = this.radius / (n + 1);

            for (int i = 0; i < spheres[n - 1].Length; i++) {
                Vector2 initialAngle = Random.insideUnitSphere.normalized;

                for (int j = 0; j < count; j++) {
                    float angleR = Random.Range(1f - variability, 1f + variability);
                    float radiusR = Random.Range(1f - variability, 1f + variability);

                    float r = radiusR * radius;

                    Vector3 p = (Vector3)(Quaternion.Euler(0f, 0f, (j + angleR) * 360f / count) * initialAngle) * 2f * r;
                    p = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) * p;
                    p = spheres[n - 1][i].origin + p;
                    Color c = leaveColors[Random.Range(0, leaveColors.Length)];

                    spheres[n][i * count + j] = new Sphere(p, r, c);

                }

            }
        }

    }

    #endregion

    void OnDrawGizmos() {
        for (int i = 0; i < spheres.Length; i++) {
            for (int j = 0; j < spheres[i].Length; j++) {
                Gizmos.color = spheres[i][j].color;
                Gizmos.DrawSphere(spheres[i][j].origin, spheres[i][j].radius);
            }
        }
    }

}

/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MoonGenerator : SphereGenerator {

    /* --- Fields --- */
    #region Fields

    // 
    public bool newcraters = false;

    // 
    [SerializeField] private ComputeShader shader;
    public int craterCount;
    private Craters craters;

    public Vector2 craterRadiiRange = new Vector2(1f, 10f);
    public Vector2 craterNormalizedDepthRange = new Vector2(0.01f, 0.05f);

    #endregion

    /* --- Craters --- */
    #region Craters

    private Craters RandomCraterDistribution(MeshSettings meshSettings) {

        Vector3[] craterOrigins = new Vector3[craterCount];
        float[] craterRadii = new float[craterCount];
        float[] craterDepths = new float[craterCount];

        for (int i = 0; i < craterCount; i++) {

            craterOrigins[i] = meshSettings.positions[Random.Range(0, meshSettings.positions.Length)];
            craterRadii[i] = Random.Range(craterRadiiRange.x, craterRadiiRange.y);
            craterDepths[i] = Random.Range(craterNormalizedDepthRange.x, craterNormalizedDepthRange.y);

        }

        return new Craters(craterOrigins, craterRadii, craterDepths);

    }

    #endregion

    /* --- Shader Processing --- */
    #region Shader Processing

    protected override void ComputeShaders(ref MeshSettings meshSettings) {
        if (newcraters || craters == null) {
            craters = RandomCraterDistribution(meshSettings);
            newcraters = false;
        }
        ComputeCraters(ref meshSettings, craters, "ComputeVertexCraters");
    }

    private void ComputeCraters(ref MeshSettings meshSettings, Craters craters, string kernelName) {
        int kernel = shader.FindKernel(kernelName);

        // Send the vertex data to the compute shader.
        ComputeBuffer vertexBuffer = new ComputeBuffer(meshSettings.positions.Length, sizeof(float) * 3);
        vertexBuffer.SetData(meshSettings.positions);
        shader.SetBuffer(kernel, "vertices", vertexBuffer);

        ComputeBuffer heightBuffer = new ComputeBuffer(meshSettings.positions.Length, sizeof(float));
        heightBuffer.SetData(meshSettings.heights);
        shader.SetBuffer(kernel, "heights", heightBuffer);

        ComputeBuffer craterOriginsBuffer = new ComputeBuffer(craters.origins.Length, sizeof(float) * 3);
        craterOriginsBuffer.SetData(craters.origins);
        shader.SetBuffer(kernel, "craterOrigins", craterOriginsBuffer);

        ComputeBuffer craterRadiiBuffer = new ComputeBuffer(craters.radii.Length, sizeof(float));
        craterRadiiBuffer.SetData(craters.radii);
        shader.SetBuffer(kernel, "craterRadii", craterRadiiBuffer);

        ComputeBuffer craterDepthsBuffer = new ComputeBuffer(craters.depths.Length, sizeof(float));
        craterDepthsBuffer.SetData(craters.depths);
        shader.SetBuffer(kernel, "craterDepths", craterDepthsBuffer);

        // Send the wavelength data to the compute shader.
        shader.SetInt("craterCount", craters.origins.Length);
        shader.SetInt("vertexCount", meshSettings.positions.Length);

        // Execute the kernel.
        uint x; uint y; uint z;
        shader.GetKernelThreadGroupSizes(kernel, out x, out y, out z);
        shader.Dispatch(kernel, (int)x, (int)y, (int)z);

        // Get the data back.
        // heightBuffer.GetData(meshSettings.heights);
        vertexBuffer.GetData(meshSettings.positions);

        vertexBuffer.Release();
        heightBuffer.Release();
        craterOriginsBuffer.Release();
        craterRadiiBuffer.Release();
        craterDepthsBuffer.Release();

    }

    #endregion

}

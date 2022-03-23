using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobGenerator : SphereGenerator {

    /* --- Fields --- */
    #region Fields

    // 
    [SerializeField] private ComputeShader shader;

    [SerializeField] private Vector3 waveLengths;
    [SerializeField] private Vector3 amplitudes;
    [SerializeField] private Vector3 offsets;

    #endregion

    protected override void ComputeShaders(ref MeshSettings meshSettings) {
        ComputeVertexWave(ref meshSettings, "ComputeVertexWave");
    }

    private void ComputeVertexWave(ref MeshSettings meshSettings, string kernelName) {
        int kernel = shader.FindKernel(kernelName);

        // Send the vertex data to the compute shader.
        ComputeBuffer vertexBuffer = new ComputeBuffer(meshSettings.positions.Length, sizeof(float) * 3);
        vertexBuffer.SetData(meshSettings.positions);
        shader.SetBuffer(kernel, "vertices", vertexBuffer);

        ComputeBuffer heightBuffer = new ComputeBuffer(meshSettings.positions.Length, sizeof(float));
        heightBuffer.SetData(meshSettings.heights);
        shader.SetBuffer(kernel, "heights", heightBuffer);

        // Send the wavelength data to the compute shader.
        shader.SetInt("vertexCount", meshSettings.positions.Length);

        string[] vars = new string[3] { "X", "Y", "Z" };
        for (int i = 0; i < vars.Length; i++) {
            shader.SetFloat("lambda" + vars[i], waveLengths[i]);
            shader.SetFloat("amp" + vars[i], amplitudes[i]);
            shader.SetFloat("offset" + vars[i], offsets[i]);
        }

        // Execute the kernel.
        uint x; uint y; uint z;
        shader.GetKernelThreadGroupSizes(kernel, out x, out y, out z);
        shader.Dispatch(kernel, (int)x, (int)y, (int)z);

        // Get the data back.
        // heightBuffer.GetData(meshSettings.heights);
        vertexBuffer.GetData(meshSettings.positions);

        vertexBuffer.Release();
        heightBuffer.Release();

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GpuGalaxy : MonoBehaviour {

    struct Particle {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 color;
    }
    const int PARTICLE_DATA_SIZE = 36;

    [Header ("Shaders and Materials")]
    public ComputeShader updateParticleShader;
    public Material material;

    [Header ("Particle Settings")]
    public int particleNumber = 10000;
    public bool updateParticle = true;

    [Header ("Init Settings")]
    public Vector2 startX = new Vector2 (-10, 10);
    public Vector2 startY = new Vector2 (-10, 10);
    public Vector2 startZ = new Vector2 (-10, 10);
    public Vector3 startVelocity = new Vector3 (10, 10, 10);

    ComputeBuffer particleBuffer;
    int computeShaderKernelID;

    CommandBuffer commandBuffer;
    Mesh mesh;

    void Awake () {
        if (updateParticleShader == null) {
            Debug.LogError ("[Error] updateParticleShader must be set");
            this.enabled = false;
            return;
        }
        if (material == null) {
            Debug.LogError ("[Error] material must be set");
            this.enabled = false;
            return;
        }
        if (particleNumber <= 0) {
            Debug.LogError ("[Error] particleNumber must be a positive number");
            this.enabled = false;
            return;
        }

        Particle[] particleData = new Particle[particleNumber];
        for (int i = 0; i < particleNumber; i++) {
            particleData[i].position.x = Random.Range (startX.x, startX.y);
            particleData[i].position.y = Random.Range (startY.x, startY.y);
            particleData[i].position.z = Random.Range (startZ.x, startZ.y);
            particleData[i].velocity = startVelocity;
        }

        particleBuffer = new ComputeBuffer (particleNumber, PARTICLE_DATA_SIZE);
        particleBuffer.SetData (particleData);

        computeShaderKernelID = updateParticleShader.FindKernel ("CSMain");
        updateParticleShader.SetBuffer (computeShaderKernelID, "particleBuffer", particleBuffer);

        material.SetBuffer ("particleBuffer", particleBuffer);

        commandBuffer = new CommandBuffer () { name = "GPU Particle" };
        Camera.main.AddCommandBuffer (CameraEvent.AfterForwardOpaque, commandBuffer);

        mesh = new Mesh ();
        mesh.vertices = new Vector3[particleNumber];
        int[] idx = new int[particleNumber];
        for (int i = 0; i < particleNumber; i++) {
            idx[i] = i;
        }
        mesh.SetIndices (idx, MeshTopology.Points, 0);
    }

    void Update () {
        if (updateParticle) {
            updateParticleShader.SetFloat ("deltaTime", Time.deltaTime);
            updateParticleShader.Dispatch (computeShaderKernelID, particleNumber, 1, 1);
        }
    }

    void OnRenderObject () {
        commandBuffer.Clear ();
        if (material != null) {
            commandBuffer.DrawProcedural (transform.localToWorldMatrix, material, 0, MeshTopology.Points, 1, particleNumber);
            // commandBuffer.DrawMesh (mesh, transform.localToWorldMatrix, material, 0, 0);
        }
    }

    void OnDestroy () {
        if (particleBuffer != null)
            particleBuffer.Release ();
        Camera.main.RemoveCommandBuffer (CameraEvent.AfterForwardOpaque, commandBuffer);
    }
}
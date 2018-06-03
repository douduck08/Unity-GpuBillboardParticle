using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GPUParticle : MonoBehaviour {

    struct Particle {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 color;
    }
    const int PARTICLE_DATA_SIZE = 36;

    [Header ("Shaders and Materials")]
    public ComputeShader updateParticleShader;
    public Material material;

    [Header ("particle Settings")]
    public int particleNumber = 1000;
    public bool updateParticle = true;

    ComputeBuffer particleBuffer;
    int computeShaderKernelID;
    // Camera mainCamera;
    // CommandBuffer commandBuffer;

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
            particleData[i].position.x = Random.Range (-10f, 10f);
            particleData[i].position.y = Random.Range (-10f, 10f);
            particleData[i].position.z = Random.Range (-10f, 10f);

            particleData[i].velocity.x = 20;
            particleData[i].velocity.y = 15;
            particleData[i].velocity.z = 5;
        }

        particleBuffer = new ComputeBuffer (particleNumber, PARTICLE_DATA_SIZE);
        particleBuffer.SetData (particleData);

        computeShaderKernelID = updateParticleShader.FindKernel ("CSMain");
        updateParticleShader.SetBuffer (computeShaderKernelID, "particleBuffer", particleBuffer);

        material.SetBuffer ("particleBuffer", particleBuffer);
    }

    void Update () {
        if (updateParticle) {
            updateParticleShader.SetFloat ("deltaTime", Time.deltaTime);
            updateParticleShader.Dispatch (computeShaderKernelID, 512, 1, 1);
        }
    }

    // void OnEnable () {
    //     commandBuffer = new CommandBuffer ();
    //     commandBuffer.name = "gpu particle";
    //     commandBuffer.DispatchCompute (updateParticleShader, computeShaderKernelID, 512, 1, 1);
    //     commandBuffer.DrawProcedural (transform.localToWorldMatrix, material, 0, MeshTopology.Points, 1, particleNumber);

    //     mainCamera = Camera.main;
    //     mainCamera.AddCommandBuffer (CameraEvent.BeforeImageEffects, commandBuffer);
    // }

    // void OnDisable () {
    //     if (mainCamera != null) {
    //         mainCamera.RemoveCommandBuffer (CameraEvent.BeforeImageEffects, commandBuffer);
    //     }
    // }

    void OnRenderObject () {
        if (material != null) {
            material.SetPass (0);
            Graphics.DrawProcedural (MeshTopology.Points, 1, particleNumber);
        }
    }

    void OnDestroy () {
        if (particleBuffer != null)
            particleBuffer.Release ();
    }
}
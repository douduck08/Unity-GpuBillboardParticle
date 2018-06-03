### GPU Billboarding Particle in Unity
Using Unity 2018.1.2f1

### Demo
![gpu-particle](https://raw.githubusercontent.com/douduck08/Unity-GpuParticleSystem/master/img/gpu-particle.jpg)

[Demo in Youtube](https://www.youtube.com/watch?v=MJnG9-iTw1s)

### Detail
* In script, using `Graphics.DrawProcedural()` to 10000 instancing point.
* Using Geometry Shader to draw quads for every points.
* Updating particle position, velocity and color with a Computer Shader.
* There are 10000 particle in video.
* GPU is GV-R777OC-1GD

#### Some Notes
* I used `Graphics.DrawProcedural()` but `Graphics.DrawMesh()`, which is not supporting lighting pass and model matrix.
* If needed multi-pass or lighting, use `Graphics.DrawMesh()` with a mesh contain numbers of point submesh. You can use `Mesh.SetIndices()` to create one.
* If needed model matrix to apply on particle spawner, use `CommandBuffer.DrawProcedural()`.

#### Ref
* Simple Introduction to Geometry Shaders in GLSL: http://www.geeks3d.com/20111111/simple-introduction-to-geometry-shaders-glsl-opengl-tutorial-part1/
* Particle Billboarding with the Geometry Shader: http://www.geeks3d.com/20140815/particle-billboarding-with-the-geometry-shader-glsl/
* A fast and easy particle system using GPU instancing: http://www.ramroumi.com/graphics/monogame/2016/04/11/fast-particles.html
* Getting Started With Compute Shaders In Unity: http://kylehalladay.com/blog/tutorial/2014/06/27/Compute-Shaders-Are-Nifty.html
* 【github】keijiro/StandardGeometryShader: https://github.com/keijiro/StandardGeometryShader
* 【github】antoinefournier/XParticle: https://github.com/antoinefournier/XParticle
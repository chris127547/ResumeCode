﻿using OpenGLEngine.RenderingEngine.Cameras;
using OpenGLEngine.RenderingEngine.Programs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers.Standard
{
    public class LightingColorAndTextureRenderer : Renderer
    {
        int shapeData;
        int indiceData;
        int triangleCount;
        int textureID;
        TextureWithLightingProgram program;
        Camera camera;
        Engine engine;

        public LightingColorAndTextureRenderer(int shapeData, int indiceData, int textureID, int triangleCount, Engine engine)
        {
            this.shapeData = shapeData; this.textureID = textureID; this.indiceData = indiceData; this.triangleCount = triangleCount;
            this.camera = engine.camera; program = engine.programList.TextureWithLightingProgram; this.engine = engine;
        }

        public void Render()
        {
            Render(Matrix4.Identity);
        }


        public void Render(Matrix4 modelMatrix)
        {
            Matrix4 model = modelMatrix;
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            Matrix3 normalModel = new Matrix3(Matrix4.Transpose(Matrix4.Invert(model)));
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.Uniform3(program.lightPositionHandle, ref engine.light.LightPosition);
            GL.Uniform1(program.diffuseLightHandle, engine.light.Diffuse);

            GL.UniformMatrix4(program.modelMatrixHandle, false, ref model);
            GL.UniformMatrix3(program.normalModelMatrixHandle, false, ref normalModel);
            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);
            GLErrorHelper.CheckError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 48, 0);

            GL.EnableVertexAttribArray(program.normalHandle);
            GL.VertexAttribPointer(program.normalHandle, 3, VertexAttribPointerType.Float, false, 48, 12);

            GL.EnableVertexAttribArray(program.textureHandle);
            GL.VertexAttribPointer(program.textureHandle, 2, VertexAttribPointerType.Float, false, 48, 24);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 48, 32);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedInt, (IntPtr)null);
            GLErrorHelper.CheckError();
        }
    }
}
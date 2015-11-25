﻿using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderingEngine.Programs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers
{
    class LightingAndColorRenderer : Renderer
    {
        int shapeData;
        int indiceData;
        int textureID;
        ColorWithLightingButNoTextureProgram program;
        Camera camera;

        public LightingAndColorRenderer(int shapeData, int indiceData, Engine engine)
        {
            this.shapeData = shapeData; this.textureID = textureID; this.indiceData = indiceData;
            this.camera = engine.camera; program = engine.programList.ColorWithLightingButNoTextureProgram;
        }

        public void Render()
        {
            Matrix4 model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(1, 1, -30);
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            Matrix3 normalModel = new Matrix3(Matrix4.Transpose(Matrix4.Invert(model)));
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            Vector3 Lightpos = new Vector3(5, 25, -30);
            GL.Uniform3(program.lightPositionHandle, ref Lightpos);

            GL.UniformMatrix4(program.modelMatrixHandle, false, ref model);
            GL.UniformMatrix3(program.normalModelMatrixHandle, false, ref normalModel);
            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);
            GLErrorHelper.CheckError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 40, 0);

            GL.EnableVertexAttribArray(program.normalHandle);
            GL.VertexAttribPointer(program.normalHandle, 3, VertexAttribPointerType.Float, false, 40, 12);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 40, 24);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedShort, (IntPtr)null);
            GLErrorHelper.CheckError();
        }
    }
}

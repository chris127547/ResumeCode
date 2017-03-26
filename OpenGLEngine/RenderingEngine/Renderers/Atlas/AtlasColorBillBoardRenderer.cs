using OpenGLEngine.RenderingEngine.Cameras;
using OpenGLEngine.RenderingEngine.Programs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers.Atlas
{
    class AtlasColorBillBoardRenderer : Renderer
    {
        int shapeData;
        int indiceData;
        int triangleCount;
        int textureID;
        TextureAtlasWithColorProgram program;
        Camera camera;
        Engine engine;

        public AtlasColorBillBoardRenderer(int shapeData, int indiceData, int textureID, int triangleCount, Engine engine)
        {
            this.shapeData = shapeData; this.textureID = textureID; this.indiceData = indiceData; this.triangleCount = triangleCount;
            this.camera = engine.camera; program = engine.programList.TextureAtlasWithColorProgram; this.engine = engine;
        }

        public void Render()
        {
            Render(Matrix4.Identity);
        }

        public void Render(Matrix4 modelMatrix)
        {
            Matrix4 model = modelMatrix;

            Matrix4 modelViewMatrix = model * camera.ViewMatrix;
            modelViewMatrix.M11 = 1;
            modelViewMatrix.M12 = 0;
            modelViewMatrix.M13 = 0;
            modelViewMatrix.M21 = 0;
            modelViewMatrix.M22 = 1;
            modelViewMatrix.M23 = 0;
            modelViewMatrix.M31 = 0;
            modelViewMatrix.M32 = 0;
            modelViewMatrix.M33 = 1;

            Matrix4 MVP = modelViewMatrix * camera.ProjectionMatrix;
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);

            GL.Uniform1(program.textureLowHandle, .66f);
            GL.Uniform1(program.textureIncrementHandle, .33f);
            GLErrorHelper.CheckError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 36, 0);
            
            GL.EnableVertexAttribArray(program.textureHandle);
            GL.VertexAttribPointer(program.textureHandle, 2, VertexAttribPointerType.Float, false, 36, 12);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 36, 20);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedInt, (IntPtr)null);
            GLErrorHelper.CheckError();
        }
    }
}

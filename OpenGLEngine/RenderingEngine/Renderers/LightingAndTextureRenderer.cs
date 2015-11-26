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
    class LightingAndTextureRenderer : Renderer
    {
        int shapeData;
        int indiceData;
        int triangleCount;
        int textureID;
        TextureWithLightingButNoColorProgram program;
        Camera camera;

        public LightingAndTextureRenderer(int shapeData, int indiceData, int textureID, int triangleCount, Engine engine)
        {
            this.shapeData = shapeData; this.textureID = textureID; this.indiceData = indiceData; this.triangleCount = triangleCount;
            this.camera = engine.camera; program = engine.programList.TextureWithLightingButNoColorProgram;
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
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 32, 0);

            GL.EnableVertexAttribArray(program.normalHandle);
            GL.VertexAttribPointer(program.normalHandle, 3, VertexAttribPointerType.Float, false, 32, 12);
            
            GL.EnableVertexAttribArray(program.textureHandle);
            GL.VertexAttribPointer(program.textureHandle, 2, VertexAttribPointerType.Float, false, 32, 24);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedShort, (IntPtr)null);
            GLErrorHelper.CheckError();
        }
    }
}

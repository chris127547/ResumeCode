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
    public class ColorRenderer : Renderer
    {
        int shapeData;
        int indiceData;
        int triangleCount;
        SimpleColorProgram program;
        Camera camera;

        public ColorRenderer(int shapeData, int indiceData, int triangleCount, Engine engine)
        {
            this.shapeData = shapeData; this.indiceData = indiceData; this.triangleCount = triangleCount;
            this.camera = engine.camera; program = engine.programList.SimpleColorProgram;
        }

        public void Render()
        {
            Render(Matrix4.Identity);
        }

        public void Render(OpenTK.Matrix4 modelMatrix)
        {
            Matrix4 model = modelMatrix;
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);

            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 28, 0);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 28, 12);
            GLErrorHelper.CheckError();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedShort, (IntPtr)null);
            GLErrorHelper.CheckError();

        }
    }
}

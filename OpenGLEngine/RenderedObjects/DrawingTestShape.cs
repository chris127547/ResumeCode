using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    class DrawingTestShape : RenderedObject
    {
        int shapeData;
        int indiceData;
        SimpleColorProgram program;
        Camera camera;

        public DrawingTestShape(Engine engine)
        {
            camera = engine.camera;
            program = engine.programList.SimpleColorProgram;

            float[] verts = {
                                -1, -1, -3, 1, 0, 0, 0,
                                -1, 1, -3, 0, 1, 0, 1,
                                1, -1, -3, 0, 0, 1, 1
                            };

            short[] indices = {
                                  0, 1, 2
                               };
            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);

            indiceData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(ushort)), indices, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            Matrix4 model = Matrix4.Identity;
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);


            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);
            GLErrorHelper.CheckError();
            GL.EnableVertexAttribArray(program.positionHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 28, 0);
            GL.EnableVertexAttribArray(program.colorHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 28, 12);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedShort, (IntPtr)null);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GLErrorHelper.CheckError();
        }


        public void UpdateMesh(FileToObjectConverters.VertexList vertices, ushort[] indices)
        {
            throw new NotImplementedException();
        }
    }
}

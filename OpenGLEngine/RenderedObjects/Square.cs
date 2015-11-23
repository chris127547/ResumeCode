using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenGLEngine.Rendering_Engine;
using OpenGLEngine.Rendering_Engine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    class Square : RenderedObject
    {
        public int squareData;
        private SimpleColorProgram program;
        private Camera camera;

        public Square(Engine engine)
        {
            camera = engine.camera;
            program = engine.programList.SimpleColorProgram;

            float[] verts = {
                                -100, -4, 100, 1, 0, 0, 1,
			                    -100, -4, -100, 0, 0, 1, 1,
			                    100, -4, 100, 0, 1, 0, 1,
			                    100, -4, -100, 1, 1, 1, 1,
			                    -100, -4, -100, 0, 0, 1, 1,
			                    100, -4, 100, 0, 1, 0, 1
                            };
            squareData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, squareData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, squareData);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 28, 0);
            GL.EnableVertexAttribArray(program.colorHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, squareData);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 28, 12);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadindicedata);
            //GL.DrawElements(PrimitiveType.Quads, 36, DrawElementsType.UnsignedShort, (IntPtr)quadindicedata);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GLErrorHelper.CheckError();
        }
    }
}

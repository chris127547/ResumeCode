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
    public class BoxShape : RenderedObject
    {
        public int cubedata, quadindicedata;
        private SimpleColorProgram program;
        private Camera camera;

        public BoxShape(Engine engine, float sizeX, float sizeZ, float sizeY, float[] color)
        {
            camera = engine.camera;
            program = engine.programList.SimpleColorProgram;
            float Xdim = 1 * sizeX, negXdim = -1 * sizeX;
            float Ydim = 1 * sizeY, negYdim = -1 * sizeY;
            float Zdim = 1 * sizeZ, negZdim = -1 * sizeZ;
            float r = color[0], g = color[1], b = color[2], a = color[3];
            float[] tcubedata = {
				Xdim, negYdim, negZdim, r,g,b,a, 
				Xdim, negYdim, Zdim, r,g,b,a, 
				negXdim, negYdim, Zdim, r,g,b,a, 
				negXdim, negYdim, negZdim, r,g,b,a, 
				Xdim, Ydim, negZdim, r,g,b,a,
				negXdim, Ydim, negZdim, r,g,b,a, 
				negXdim, Ydim, Zdim, r,g,b,a, 
				Xdim, Ydim, Zdim, r,g,b,a, 
				Xdim, negYdim, negZdim, r,g,b,a, 
				Xdim, Ydim, negZdim, r,g,b,a, 
				Xdim, Ydim, Zdim, r,g,b,a, 
				Xdim, negYdim, Zdim, r,g,b,a, 
				Xdim, negYdim, Zdim, r,g,b,a, 
				Xdim, Ydim, Zdim, r,g,b,a,
				negXdim, Ydim, Zdim, r,g,b,a,
				negXdim, negYdim, Zdim, r,g,b,a,
				negXdim, negYdim, Zdim, r,g,b,a,
				negXdim, Ydim, Zdim, r,g,b,a,
				negXdim, Ydim, negZdim, r,g,b,a,
				negXdim, negYdim, negZdim, r,g,b,a,
				Xdim, Ydim, negZdim, r,g,b,a,
				Xdim, negYdim, negZdim, r,g,b,a,
				negXdim, negYdim, negZdim, r,g,b,a,
				negXdim, Ydim, negZdim, r,g,b,a
		    };
            short[] tquadindicedata = {
				0, 1, 2, 
				0, 2, 3, 
				4, 5, 6, 
				4, 6, 7, 
				8, 9, 10, 
				8, 10, 11, 
				12, 13, 14, 
				12, 14, 15, 
				16, 17, 18, 
				16, 18, 19, 
				20, 21, 22, 
				20, 22, 23 
			};

            cubedata = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, cubedata);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(tcubedata.Length * sizeof(float)), tcubedata, BufferUsageHint.StaticDraw);

            quadindicedata = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadindicedata);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(tquadindicedata.Length * sizeof(int)), tquadindicedata, BufferUsageHint.StaticDraw);

        }

        public void Render()
        {
            Matrix4 model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(0, 0, -30);
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);

            GL.BindBuffer(BufferTarget.ArrayBuffer, cubedata);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 28, 0);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 28, 12);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadindicedata);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, (IntPtr)null);
        }


        public void UpdateMesh(FileToObjectConverters.VertexList vertices, int[] indices)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref cubedata);
            GL.DeleteBuffers(1, ref quadindicedata);
        }
    }
}

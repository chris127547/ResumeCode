using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Programs;
using OpenGLEngine.RenderingEngine.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    class TexturedBoxWithLighting : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        TextureWithLightingProgram program;
        Camera camera;

        public TexturedBoxWithLighting(Engine engine, float sizeX, float sizeY, float sizeZ, float[] color)
        {
            camera = engine.camera;
            program = engine.programList.TextureWithLightingProgram;

            float Xdim = 1 * sizeX, negXdim = -1 * sizeX;
            float Ydim = 1 * sizeY, negYdim = -1 * sizeY;
            float Zdim = 1 * sizeZ, negZdim = -1 * sizeZ;//note texture will be off slightly if size is non integer
            float Ux = Xdim + .5f, negUx = negXdim + .5f;
            float Uy = Zdim + .5f, negUy = negZdim + .5f;//note Y and Z values reversed here
            float Uz = Ydim + .5f, negUz = negYdim + .5f;
            float r = color[0], g = color[1], b = color[2], a = color[3];

            float[] cubedata = {
					Xdim, negYdim, negZdim, 0,-1,0, r,g,b,a, Ux, negUy, 
					Xdim, negYdim, Zdim, 0,-1,0, r,g,b,a, Ux, Uy, 
					negXdim, negYdim, Zdim, 0,-1,0, r,g,b,a, negUx, Uy, 
					negXdim, negYdim, negZdim, 0,-1,0, r,g,b,a, negUx, negUy, 
					Xdim, Ydim, negZdim, 0,1,0, r,g,b,a, Ux, negUy, 
					negXdim, Ydim, negZdim, 0,1,0, r,g,b,a, negUx, negUy, 
					negXdim, Ydim, Zdim, 0,1,0, r,g,b,a, negUx, Uy, 
					Xdim, Ydim, Zdim, 0,1,0, r,g,b,a, Ux, Uy, 
					Xdim, negYdim, negZdim, 1,0,0, r,g,b,a, negUz, negUy, 
					Xdim, Ydim, negZdim, 1,0,0, r,g,b,a, Uz, negUy, 
					Xdim, Ydim, Zdim, 1,0,0, r,g,b,a, Uz, Uy, 
					Xdim, negYdim, Zdim, 1,0,0, r,g,b,a, negUz, Uy, 
					Xdim, negYdim, Zdim, 0,0,1, r,g,b,a, Ux, negUz, 
					Xdim, Ydim, Zdim, 0,0,1, r,g,b,a, Ux, Uz, 
					negXdim, Ydim, Zdim, 0,0,1, r,g,b,a, negUx, Uz, 
					negXdim, negYdim, Zdim, 0,0,1, r,g,b,a, negUx, negUz, 
					negXdim, negYdim, Zdim, -1,0,0, r,g,b,a, negUz, Uy, 
					negXdim, Ydim, Zdim, -1,0,0, r,g,b,a, Uz, Uy, 
					negXdim, Ydim, negZdim, -1,0,0, r,g,b,a, Uz, negUy, 
					negXdim, negYdim, negZdim, -1,0,0, r,g,b,a, negUz, negUy, 
					Xdim, Ydim, negZdim, 0,0,-1, r,g,b,a, Ux, Uz, 
					Xdim, negYdim, negZdim, 0,0,-1, r,g,b,a, Ux, negUz, 
					negXdim, negYdim, negZdim, 0,0,-1, r,g,b,a, negUx, negUz, 
					negXdim, Ydim, negZdim, 0,0,-1, r,g,b,a, negUx, Uz,
					
			};
            short[] quadindicedata = {
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

            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(cubedata.Length * sizeof(float)), cubedata, BufferUsageHint.StaticDraw);

            indiceData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quadindicedata.Length * sizeof(int)), quadindicedata, BufferUsageHint.StaticDraw);

            textureID = TextureManager.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Ball Mazer textures\\brick.png");
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
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 48, 0);

            GL.EnableVertexAttribArray(program.normalHandle);
            GL.VertexAttribPointer(program.normalHandle, 3, VertexAttribPointerType.Float, false, 48, 12);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 48, 24);

            GL.EnableVertexAttribArray(program.textureHandle);
            GL.VertexAttribPointer(program.textureHandle, 2, VertexAttribPointerType.Float, false, 48, 40);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, (IntPtr)null);
            GLErrorHelper.CheckError();
        }


        public void UpdateMesh(FileToObjectConverters.VertexList vertices, int[] indices)
        {
            throw new NotImplementedException();
        }
    }
}

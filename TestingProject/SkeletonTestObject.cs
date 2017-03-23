using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Renderers;
using OpenGLEngine.RenderingEngine.Renderers.Skeleton;
using OpenGLEngine.RenderingEngine.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingProject
{
    class SkeletonTestObject : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        public VertexList vertices;
        public int[] indices;
        SkeletonRenderer renderer;
        Matrix4 position;
        Matrix4[] skeleton;

        int animCount = 0;

        public SkeletonTestObject(Engine engine, PlyFileParser objectData)
        {
            UpdateMesh(objectData.vertices, objectData.indices);
            textureID = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Ball Mazer textures\\brick.png");
            renderer = new SkeletonLightingColorAndTextureRenderer(shapeData, indiceData, textureID, objectData.indices.Length, engine);
            SetupTestSkeleton();
        }

        private void SetupTestSkeleton()
        {
            position = Matrix4.Identity;
            skeleton = new Matrix4[4];
            skeleton[0] = Matrix4.CreateTranslation(new Vector3(0, 3, 0));
            skeleton[1] = Matrix4.CreateTranslation(new Vector3(-1, 0, 4));
            //skeleton[0] = Matrix4.CreateRotationX(15) * Matrix4.CreateTranslation(new Vector3(0, 3, 0));
            //skeleton[1] = Matrix4.CreateRotationX(15) * Matrix4.CreateTranslation(new Vector3(-1, 0, 4));
            //skeleton[0] = Matrix4.CreateRotationX(15);
            //skeleton[1] = Matrix4.CreateRotationY(15);
            skeleton[2] = Matrix4.Identity;
            skeleton[3] = Matrix4.Identity;
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref shapeData);
            GL.DeleteBuffers(1, ref indiceData);
            if (textureID != 0) { GL.DeleteTexture(textureID); }
        }

        public void Render()
        {
            animCount++;
            skeleton[0] *= Matrix4.CreateTranslation(new Vector3(0, .1f, 0));
            skeleton[1] *= Matrix4.CreateTranslation(new Vector3(0, -.1f, 0));
            renderer.Render(position, skeleton);
            if (animCount >= 360)
            {
                skeleton[0] = Matrix4.Identity;
                skeleton[1] = Matrix4.Identity;
                animCount = 0;
            }
        }

        public void UpdateMesh(OpenGLEngine.RenderedObjects.FileToObjectConverters.VertexList vertexList, int[] indices)
        {
            this.vertices = vertexList;
            this.indices = indices;
            float[] vertices = vertexList.GetAvailableShapeData();

            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            indiceData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);
        }
    }
}

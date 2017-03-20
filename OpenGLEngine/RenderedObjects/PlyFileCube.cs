using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Enums;
using OpenGLEngine.RenderingEngine.Programs;
using OpenGLEngine.RenderingEngine.Renderers;
using OpenGLEngine.RenderingEngine.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    public class PlyFileCube : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        public VertexList vertices;
        public int[] indices;
        public Matrix4 position = Matrix4.Identity;
        Renderer renderer;

        public PlyFileCube(Engine engine, float[] color, RenderingStyle style, int texture)
        {
            PlyFileParser objectData;
            if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                objectData = new PlyFileParser("C:\\Users\\Chris\\Documents\\3D models\\normalcube.ply", color, new Vector3(1));
            }
            else
            {
                objectData = new PlyFileParser("C:\\Users\\Chris\\Documents\\3D models\\texturedCube.ply", null, new Vector3(1));
            }

            this.vertices = objectData.vertices;
            this.indices = objectData.indices;

            float[] vertices = objectData.vertices.GetAvailableShapeData();

            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            indiceData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(objectData.indices.Length * sizeof(int)), objectData.indices, BufferUsageHint.StaticDraw);

            if (style == RenderingStyle.TextureAndLightingWithNoColorHighlights)
            {
                //textureID = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Ball Mazer textures\\brick.png");
                renderer = new LightingAndTextureRenderer(shapeData, indiceData, texture, objectData.indices.Length, engine);
            }
            else if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                renderer = new LightingAndColorRenderer(shapeData, indiceData, objectData.indices.Length, engine);
            }
        }

        public void Render()
        {
            renderer.Render(position);
        }


        public void UpdateMesh(VertexList vertices, int[] indices)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref shapeData);
            GL.DeleteBuffers(1, ref indiceData);
            if (textureID != 0) { GL.DeleteTexture(textureID); }
        }
    }
}

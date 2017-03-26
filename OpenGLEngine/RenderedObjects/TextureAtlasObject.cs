using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Enums;
using OpenGLEngine.RenderingEngine.Renderers;
using OpenGLEngine.RenderingEngine.Renderers.Atlas;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    public class TextureAtlasObject : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        public VertexList vertices;
        public int[] indices;
        AtlasRenderer renderer;
        
        public TextureAtlasObject(Engine engine, float[] color, Vector3 scale, RenderingStyle style, string filepath, string texturePath, int frames)
        {
            string extension = Path.GetExtension(filepath);
            if (extension == ".ply")
            {
                PlyFileParser objectData = new PlyFileParser(filepath, color, scale);
                UpdateMesh(objectData.vertices, objectData.indices);
                CreateRenderer(style, objectData, engine, texturePath, frames);
            }
            else
            {
                throw new Exception("File type not supported");
            }
        }

        private void CreateRenderer(RenderingStyle style, PlyFileParser objectData, Engine engine, string texturePath, int frames)
        { 
            textureID = engine.LoadTexture(texturePath);

            if (style == RenderingStyle.BillBoardTextureAndColor)
            {
                renderer = new AtlasColorBillBoardRenderer(shapeData, indiceData, textureID, objectData.indices.Length, engine, frames);
            }
            else
            {
                throw new Exception("Unsupported RenderingStyle");
            }
        }

        public void Render()
        {
            renderer.Render(Matrix4.Identity, 0);
        }

        public void Render(Matrix4 modelMatrix, int frame)
        {
            renderer.Render(modelMatrix, frame);
        }

        public void UpdateMesh(VertexList vertexList, int[] indices)
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

        public void Delete()
        {
            GL.DeleteBuffers(1, ref shapeData);
            GL.DeleteBuffers(1, ref indiceData);
            if (textureID != 0) { GL.DeleteTexture(textureID); }
        }
    }
}

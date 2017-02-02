using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Enums;
using OpenGLEngine.RenderingEngine.Renderers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    public class GenericRenderedObject : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        public VertexList vertices;
        public int[] indices;
        Renderer renderer;

        public GenericRenderedObject(Engine engine, float[] color, VertexList vertices, int[] indices, RenderingStyle style)
        {
            UpdateMesh(vertices, indices);
            if(style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                renderer = new LightingAndColorRenderer(shapeData, indiceData, indices.Length, engine);
            }
            else if (style == RenderingStyle.SimpleSolidColors)
            {
                renderer = new ColorRenderer(shapeData, indiceData, indices.Length, engine);
            }
        }

        public GenericRenderedObject(Engine engine, float[] color, VertexList vertices, int[] indices, int textureId, RenderingStyle style)
        {
            UpdateMesh(vertices, indices);
            this.textureID = textureId;
            if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                renderer = new LightingAndColorRenderer(shapeData, indiceData, indices.Length, engine);
            }
            else if (style == RenderingStyle.SimpleSolidColors)
            {
                renderer = new ColorRenderer(shapeData, indiceData, indices.Length, engine);
            }
            else if (style == RenderingStyle.TextureColorAndLighting)
            {
                renderer = new LightingColorAndTextureRenderer(shapeData, indiceData, textureID, indices.Length, engine);
            }
        }
                
        /*private void CreatePlyFileObject(Engine engine, float[] color, RenderingStyle style, string filepath)
        {
            PlyFileParser objectData;
            if (filepath == null)
            {
                if (style == RenderingStyle.ColorAndLightingWithNoTextures)
                {
                    objectData = new PlyFileParser("C:\\Users\\Chris\\Documents\\3D models\\normalcube.ply", color);
                }
                else
                {
                    objectData = new PlyFileParser("C:\\Users\\Chris\\Documents\\3D models\\Chris Cube.ply", null);
                }
            }
            else
            {
                objectData = new PlyFileParser(filepath, color);
            }
            UpdateMesh(objectData.vertices, objectData.indices);

            CreateRenderer(style, objectData, engine);
        }*/

        /*private void CreateRenderer(RenderingStyle style, PlyFileParser objectData, Engine engine)
        {
            if (style == RenderingStyle.TextureAndLightingWithNoColorHighlights)
            {
                textureID = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Ball Mazer textures\\brick.png");
                renderer = new LightingAndTextureRenderer(shapeData, indiceData, textureID, objectData.indices.Length, engine);
            }
            else if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                renderer = new LightingAndColorRenderer(shapeData, indiceData, objectData.indices.Length, engine);
            }            
        }*/

        public void Render()
        {
            renderer.Render();
        }

        public void Render(Matrix4 modelMatrix)
        {
            renderer.Render(modelMatrix);
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

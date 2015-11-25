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
    class PlyFileCube : RenderedObject
    {
        int shapeData;
        int indiceData;
        int textureID;
        Renderer renderer;

        public PlyFileCube(Engine engine, float[] color, RenderingStyle style)
        {
            PlyObject objectData;
            if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                objectData = new PlyObject("C:\\Users\\Chris\\Documents\\3D models\\normalcube.ply", color);
            }
            else
            {
                objectData = new PlyObject("C:\\Users\\Chris\\Documents\\3D models\\Chris Cube.ply", null);
            }

            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(objectData.vertices.Length * sizeof(float)), objectData.vertices, BufferUsageHint.StaticDraw);

            indiceData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(objectData.indices.Length * sizeof(ushort)), objectData.indices, BufferUsageHint.StaticDraw);

            if (style == RenderingStyle.TextureAndLightingWithNoColorHighlights)
            {
                textureID = TextureManager.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Ball Mazer textures\\brick.png");
                renderer = new LightingAndTextureRenderer(shapeData, indiceData, textureID, engine);
            }
            else if (style == RenderingStyle.ColorAndLightingWithNoTextures)
            {
                renderer = new LightingAndColorRenderer(shapeData, indiceData, engine);
            }
        }

        public void Render()
        {
            renderer.Render();
        }
    }
}

using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Renderers;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    public class ObjFileObject : RenderedObject
    {
        int shapeData;
        List<Renderer> renderers = new List<Renderer>();
        List<int> textureIds = new List<int>();
        List<int> indiceDataSets = new List<int>();

        public ObjFileObject(Engine engine, string filePath)
        {
            ObjFileParser objectData = new ObjFileParser(filePath, new float[] { 1, 1, 1, 1 });

            float[] vertices = objectData.vertices.GetAvailableShapeData();

            shapeData = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            foreach (Tuple<MtlFileParser.Material, int[]> set in objectData.materialIndices)
            {
                int texId;
                if (set.Item1.texturePath != "")
                {
                    string fullTexturePath = Path.GetDirectoryName(objectData.material.path) + "\\" + set.Item1.texturePath;
                    texId = engine.textureManager.LoadTexture(fullTexturePath);
                    textureIds.Add(texId);
                }
                else
                {
                    texId = LoadEmptyTexture(engine);
                }
                
                int[] indices = set.Item2;

                int indiceData = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

                
                indiceDataSets.Add(indiceData);

                Renderer renderer = new LightingColorAndTextureRenderer(shapeData, indiceData, texId, indices.Length, engine);
                renderers.Add(renderer);
            }
        }        

        public void Render()
        {
            foreach (Renderer r in renderers)
            {
                r.Render();
            }
        }

        public void UpdateMesh(VertexList vertexList, int[] indices)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref shapeData);
            for (int i = 0; i < indiceDataSets.Count; i++)
            {
                int reference = indiceDataSets[i];
                GL.DeleteBuffers(1, ref reference);
            }
            for (int i = 0; i < textureIds.Count; i++)
            {
                int texref = textureIds[i];
                GL.DeleteTexture(texref);
            }
        }

        private int LoadEmptyTexture(Engine engine)
        {
            Bitmap bitmap = new Bitmap(1, 1);
            bitmap.SetPixel(0, 0, Color.FromArgb(255, 255, 255, 255));
            return engine.textureManager.LoadTexture(bitmap, "EmptyTexure");
        }
    }
}

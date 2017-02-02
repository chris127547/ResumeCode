using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class ObjFileParser
    {
        public int[] indices;
        public VertexList vertices;

        public MtlFileParser material;

        bool hasNormals = false;
        bool hasTextures = false;

        public ObjFileParser(string filepath, float[] rgba)
        {
            if (File.Exists(filepath))
            {
                if (Path.GetExtension(filepath) != ".obj")
                {
                    throw new Exception("An invalid file type was used to construct a ObjObject. Only correctly formatted .obj files will work.");
                }

                List<Vector3> positions = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> textureCoords = new List<Vector2>();
                List<int> positionIndices = new List<int>();
                List<int> normalIndices = new List<int>();
                List<int> textureIndices = new List<int>();

                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                vertices = new VertexList();

                MtlFileParser.Material currentMaterial = null;

                foreach (string s in lines)
                {
                    string[] items = s.Split(' ');
                    if (items[0] == "v")
                    {       
                        Vector3 position = new Vector3();
                        position.X = float.Parse(items[1]);
                        position.Y = float.Parse(items[2]);
                        position.Z = float.Parse(items[3]);
                        positions.Add(position);
                    }
                    else if (items[0] == "vn")
                    {
                        hasNormals = true;
                        Vector3 normal = new Vector3();
                        normal.X = float.Parse(items[1]);
                        normal.Y = float.Parse(items[2]);
                        normal.Z = float.Parse(items[3]);
                        normals.Add(normal);
                    }
                    else if (items[0] == "vt")
                    {
                        hasTextures = true;
                        Vector2 texture = new Vector2();
                        texture.X = float.Parse(items[1]);
                        texture.Y = float.Parse(items[2]);
                        textureCoords.Add(texture);
                    }
                    else if (items[0] == "f")
                    {
                        if (hasNormals && hasTextures)
                        {
                            AddIndiceLineWithNormalsAndTextures(items, positionIndices, normalIndices, textureIndices);
                        }
                        else if (hasNormals)
                        {
                            AddIndiceLineWithNormals(items, positionIndices, normalIndices);
                        }
                        else
                        {
                            AddIndiceLine(items, positionIndices);
                        }                        
                    }
                    else if (items[0] == "mtllib")
                    {
                        string materialPath = Path.GetDirectoryName(filepath);
                        materialPath += "\\" + items[1];
                        material = new MtlFileParser(materialPath);
                    }
                    else if (items[0] == "usemtl")
                    {
                        currentMaterial = material.Materials[items[1]];
                    }
                }
                if (hasNormals && hasTextures)
                {
                    CreateSingleVertexListFromFaceDataWithNormalsAndTextures(positions, normals, textureCoords, positionIndices, normalIndices, textureIndices,
                        currentMaterial);
                }
                else if (hasNormals)
                {
                    CreateSingleVertexListFromFaceDataWithNormals(positions, normals, positionIndices, normalIndices, rgba);
                }
                else
                {
                    CreateVertexListFromFaceData(positions, rgba);
                    indices = positionIndices.ToArray();
                }
            }
        }

        private void CreateSingleVertexListFromFaceDataWithNormalsAndTextures(List<Vector3> positions, List<Vector3> normals, List<Vector2> textureCoords,
            List<int> positionIndices, List<int> normalIndices, List<int> textureIndices, MtlFileParser.Material currentMaterial)
        {
            List<int> finalIndices = new List<int>();
            for (int i = 0; i < positionIndices.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[positionIndices[i]]);
                v.normal = new Vertex.Normal(normals[normalIndices[i]]);
                v.texture = new Vertex.Texture(textureCoords[textureIndices[i]].X * currentMaterial.Umax + currentMaterial.Umin,
                    textureCoords[textureIndices[i]].Y * currentMaterial.Vmax + currentMaterial.Vmin);
                v.color = new Vertex.Color((float)currentMaterial.Color.R / 255, (float)currentMaterial.Color.G / 255,
                    (float)currentMaterial.Color.B / 255, (float)currentMaterial.Color.A / 255); 
                
                vertices.Add(v);
                finalIndices.Add(i);
            }
            indices = finalIndices.ToArray();
        }        

        private void CreateSingleVertexListFromFaceDataWithNormals(List<Vector3> positions, List<Vector3> normals, List<int> positionIndices,
            List<int> normalIndices, float[] color)
        {
            List<int> finalIndices = new List<int>();
            for (int i = 0; i < positionIndices.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[positionIndices[i]]);
                v.normal = new Vertex.Normal(normals[normalIndices[i]]);
                if (color != null)
                {
                    v.color = new Vertex.Color(color[0], color[1], color[2], color[3]);
                }
                vertices.Add(v);
                finalIndices.Add(i);
            }
            indices = finalIndices.ToArray();
        }

        private void CreateVertexListFromFaceData(List<Vector3> positions, float[] color)
        {
            for(int i = 0; i < positions.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[i]);
                if (color != null)
                {
                    v.AddColor(color);
                }
                vertices.Add(v);
            } 
        }

        private void AddIndiceLineWithNormalsAndTextures(string[] items, List<int> positionIndices, List<int> normalIndices, List<int> textureIndices)
        {
            List<int> positionData = new List<int>();
            List<int> normalData = new List<int>();
            List<int> textureData = new List<int>();
            for (int i = 1; i < items.Length; i++)
            {
                string facePoint = items[i];
                string[] faceData = facePoint.Split('/');
                positionData.Add(int.Parse(faceData[0]) - 1);
                textureData.Add(int.Parse(faceData[1]) - 1);
                normalData.Add(int.Parse(faceData[2]) - 1);
            }
            positionIndices.Add(positionData[0]);
            positionIndices.Add(positionData[1]);
            positionIndices.Add(positionData[2]);
            normalIndices.Add(normalData[0]);
            normalIndices.Add(normalData[1]);
            normalIndices.Add(normalData[2]);
            textureIndices.Add(textureData[0]);
            textureIndices.Add(textureData[1]);
            textureIndices.Add(textureData[2]);
            if (items.Length == 5)
            {
                positionIndices.Add(positionData[0]);
                positionIndices.Add(positionData[2]);
                positionIndices.Add(positionData[3]);
                normalIndices.Add(normalData[0]);
                normalIndices.Add(normalData[2]);
                normalIndices.Add(normalData[3]);
                textureIndices.Add(textureData[0]);
                textureIndices.Add(textureData[2]);
                textureIndices.Add(textureData[3]);
            }
        }

        private void AddIndiceLineWithNormals(string[] items, List<int> positionIndices, List<int> normalIndices)
        {
            List<int> positionData = new List<int>();
            List<int> normalData = new List<int>();
            for (int i = 1; i < items.Length; i++)
            {
                string facePoint = items[i];
                string[] faceData = facePoint.Split('/');
                positionData.Add(int.Parse(faceData[0]) - 1);
                normalData.Add(int.Parse(faceData[2]) - 1);
            }
            positionIndices.Add(positionData[0]);
            positionIndices.Add(positionData[1]);
            positionIndices.Add(positionData[2]);
            normalIndices.Add(normalData[0]);
            normalIndices.Add(normalData[1]);
            normalIndices.Add(normalData[2]);
            if (items.Length == 5)
            {
                positionIndices.Add(positionData[0]);
                positionIndices.Add(positionData[2]);
                positionIndices.Add(positionData[3]);
                normalIndices.Add(normalData[0]);
                normalIndices.Add(normalData[2]);
                normalIndices.Add(normalData[3]);
            }
        }

        private void AddIndiceLine(string[] items, List<int> indiceList)
        {
            List<int> data = new List<int>();
            for (int i = 1; i < items.Length; i++)
            {
                string facePoint = items[i];
                string[] faceData = facePoint.Split('/');
                data.Add(int.Parse(faceData[0]) - 1);
            }
            indiceList.Add(data[0]);
            indiceList.Add(data[1]);
            indiceList.Add(data[2]);
            if (items.Length == 5)
            {
                indiceList.Add(data[0]);
                indiceList.Add(data[2]);
                indiceList.Add(data[3]);
            }
        }
    }
}

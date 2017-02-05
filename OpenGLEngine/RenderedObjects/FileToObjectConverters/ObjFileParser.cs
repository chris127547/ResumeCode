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
        public VertexList vertices;

        public MtlFileParser material;

        public List<Tuple<MtlFileParser.Material, int[]>> materialIndices;

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
                materialIndices = new List<Tuple<MtlFileParser.Material, int[]>>();

                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                vertices = new VertexList();

                MtlFileParser.Material currentMaterial = null;

                int lastMaterialIndex = 0;

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
                            AddVertexWithNormalsAndTextures(items, positions, normals, textureCoords, currentMaterial);
                        }
                        else if (hasNormals)
                        {
                            AddVertexWithNormals(items, positions, normals, rgba);
                        }
                        else
                        {
                            AddVertex(items, positions, rgba);
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
                        AddMaterialIndiceSet(currentMaterial, lastMaterialIndex);
                        currentMaterial = material.Materials[items[1]];
                    }
                }
                AddMaterialIndiceSet(currentMaterial, lastMaterialIndex);                             
            }
        }

        private void AddVertex(string[] items, List<Vector3> positions, float[] color)
        {
            List<int> data = new List<int>();
            List<int> positionIndices = new List<int>();
            for (int i = 1; i < items.Length; i++)
            {
                string facePoint = items[i];
                string[] faceData = facePoint.Split('/');
                data.Add(int.Parse(faceData[0]) - 1);
            }
            positionIndices.Add(data[0]);
            positionIndices.Add(data[1]);
            positionIndices.Add(data[2]);
            if (items.Length == 5)
            {
                positionIndices.Add(data[0]);
                positionIndices.Add(data[2]);
                positionIndices.Add(data[3]);
            }
            for (int i = 0; i < positionIndices.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[positionIndices[i]]);
                if (color != null)
                {
                    v.AddColor(color);
                }
                vertices.Add(v);
            }
        }

        private void AddVertexWithNormals(string[] items, List<Vector3> positions, List<Vector3> normals, float[] color)
        {
            List<int> positionData = new List<int>();
            List<int> normalData = new List<int>();
            List<int> positionIndices = new List<int>();
            List<int> normalIndices = new List<int>();
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
            for (int i = 0; i < positionIndices.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[positionIndices[i]]);
                v.normal = new Vertex.Normal(normals[normalIndices[i]]);
                if (color != null)
                {
                    v.AddColor(color);
                }
                vertices.Add(v);
            }
        }

        private void AddVertexWithNormalsAndTextures(string[] items, List<Vector3> positions, List<Vector3> normals, List<Vector2> textureCoords,
            MtlFileParser.Material currentMaterial)
        {
            List<int> positionData = new List<int>();
            List<int> normalData = new List<int>();
            List<int> textureData = new List<int>();
            List<int> positionIndices = new List<int>();
            List<int> normalIndices = new List<int>();
            List<int> textureIndices = new List<int>();
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
            for (int i = 0; i < positionIndices.Count; i++)
            {
                Vertex v = new Vertex();
                v.position = new Vertex.Position(positions[positionIndices[i]]);
                v.normal = new Vertex.Normal(normals[normalIndices[i]]);
                v.texture = new Vertex.Texture(textureCoords[textureIndices[i]].X, textureCoords[textureIndices[i]].Y);
                v.color = new Vertex.Color((float)currentMaterial.Color.R / 255, (float)currentMaterial.Color.G / 255,
                    (float)currentMaterial.Color.B / 255, (float)currentMaterial.Color.A / 255); 
                vertices.Add(v);
            }
        }

        private void AddMaterialIndiceSet(MtlFileParser.Material currentMaterial, int lastMaterialIndex)
        {
            if (currentMaterial != null)
            {
                List<int> currentIndices = new List<int>();
                for (int i = lastMaterialIndex; i < vertices.Count; i++)
                {
                    currentIndices.Add(i);
                }
                materialIndices.Add(new Tuple<MtlFileParser.Material, int[]>(currentMaterial, currentIndices.ToArray()));
            }
        }
    }
}

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
                List<int> positionIndices = new List<int>();
                List<int> normalIndices = new List<int>();

                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                vertices = new VertexList();

                int index = 0;
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
                        Vector3 normal = new Vector3();
                        normal.X = float.Parse(items[1]);
                        normal.Y = float.Parse(items[2]);
                        normal.Z = float.Parse(items[3]);
                        normals.Add(normal);
                    }
                    else if (items[0] == "f")
                    {
                        AddIndiceLine(items, positionIndices, normalIndices);
                    }
                }

                CreateSingleVertexListFromFaceData(positions, normals, positionIndices, normalIndices, rgba);
            }
            int temp = 0;
        }

        private void CreateSingleVertexListFromFaceData(List<Vector3> positions, List<Vector3> normals, List<int> positionIndices, List<int> normalIndices, float[] color)
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

        private void AddIndiceLine(string[] items, List<int> positionIndices, List<int> normalIndices)
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
    }
}

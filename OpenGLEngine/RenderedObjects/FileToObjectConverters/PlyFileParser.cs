using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class PlyFileParser
    {
        //public float[] verticess;
        public int[] indices;
        public VertexList vertices;

        public PlyFileParser(string filepath, float[] rgba)
        {
            if (File.Exists(filepath))
            {
                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                if (lines[0] != "ply")
                {
                    throw new Exception("An invalid file type was used to construct a PlyObject. Only correctly formatted .ply files will work.");
                }
                int numberOfVertices = 0, numberOfFaces = 0, index = 1;
                bool readingVertexProperties = false;
                List<string> vertexProperties = new List<string>();
                while (lines[index] != "end_header")
                {
                    string[] items = lines[index].Split(' ');
                    if (items[0] == "element")
                    {
                        readingVertexProperties = false;
                        if (items[1] == "vertex") { numberOfVertices = Int32.Parse(items[2]); readingVertexProperties = true; }
                        else if (items[1] == "face") { numberOfFaces = Int32.Parse(items[2]); }
                    }
                    else if (readingVertexProperties && items[0] == "property")
                    {
                        vertexProperties.Add(items[2]);
                    }
                    index++;
                }
                index++;
                vertices = new VertexList();
                for (int i = 0; i < numberOfVertices; i++)
                {
                    Vertex vertex = new Vertex();
                    string[] values = lines[index + i].Split(' ');
                    for (int j = 0; j < values.Length; j++)
                    {
                        vertex.AddValue(vertexProperties[j], float.Parse(values[j]));
                    }
                    if (rgba != null)
                    {
                        vertex.AddColor(rgba);
                    }
                    vertices.Add(vertex);
                }
                index = index + numberOfVertices;
                List<int> indiceList = new List<int>();
                for (int i = 0; i < numberOfFaces; i++)
                {
                    string[] values = lines[index + i].Split(' ');
                    indiceList.Add(int.Parse(values[1]));
                    indiceList.Add(int.Parse(values[2]));
                    indiceList.Add(int.Parse(values[3]));
                    if (values.Length > 4)
                    {
                        indiceList.Add(int.Parse(values[1]));
                        indiceList.Add(int.Parse(values[3]));
                        indiceList.Add(int.Parse(values[4]));
                    }
                }
                indices = indiceList.ToArray<int>();
            }
        }
    }
}

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
                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                vertices = new VertexList();
                List<int> indiceList = new List<int>();

                int index = 0;
                foreach (string s in lines)
                {
                    string[] items = s.Split(' ');
                    if (items[0] == "v")
                    {                        
                        Vertex v = new Vertex();
                        Vertex.Position position = new Vertex.Position();
                        position.X = float.Parse(items[1]);
                        position.Y = float.Parse(items[2]);
                        position.Z = float.Parse(items[3]);
                        v.position = position;
                        if (rgba != null)
                        {
                            v.AddColor(rgba);
                        }
                        vertices.Add(v);                        
                    }
                    else if (items[0] == "f")
                    {
                        AddIndiceLine(items, indiceList);
                    }
                }
                indices = indiceList.ToArray();
            }
            int temp = 0;
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

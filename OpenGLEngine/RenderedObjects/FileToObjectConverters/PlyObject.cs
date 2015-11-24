using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class PlyObject
    {
        public float[] vertices;
        public ushort[] indices;

        public PlyObject(string filepath, float[] rgba)
        {
            if (File.Exists(filepath))
            {
                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                if (lines[0] != "ply")
                {
                    throw new Exception("An invalid file type was used to construct a PlyObject. Only correctly formatted .ply files will work.");
                }
                int numberOfVertices = 0, numberOfFaces = 0, index = 1;
                while (lines[index] != "end_header")
                {
                    string[] items = lines[index].Split(' ');
                    if (items[0] == "element")
                    {
                        if (items[1] == "vertex") { numberOfVertices = Int32.Parse(items[2]); }
                        else if (items[1] == "face") { numberOfFaces = Int32.Parse(items[2]); }
                    }
                    index++;
                }
                index++;                
                List<float> verticeList = new List<float>();
                for (int i = 0; i < numberOfVertices; i++)
                {
                    string[] values = lines[index + i].Split(' ');
                    for (int j = 0; j < values.Length; j++)
                    {
                        verticeList.Add(float.Parse(values[j]));
                    }
                    if (rgba != null)
                    {
                        //verticeList.Add(rgba[0]); verticeList.Add(rgba[1]); verticeList.Add(rgba[2]); verticeList.Add(rgba[3]);
                    }
                }
                vertices = verticeList.ToArray<float>();
                index = index + numberOfVertices;
                List<ushort> indiceList = new List<ushort>();
                for (int i = 0; i < numberOfFaces; i++)
                {
                    string[] values = lines[index + i].Split(' ');
                    indiceList.Add(ushort.Parse(values[1]));
                    indiceList.Add(ushort.Parse(values[2]));
                    indiceList.Add(ushort.Parse(values[3]));
                    if (values.Length > 4)
                    {
                        indiceList.Add(ushort.Parse(values[1]));
                        indiceList.Add(ushort.Parse(values[3]));
                        indiceList.Add(ushort.Parse(values[4]));
                    }
                }
                indices = indiceList.ToArray<ushort>();
            }
        }
    }
}

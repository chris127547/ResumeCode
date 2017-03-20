using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class MtlFileParser
    {
        public Dictionary<string, Material> Materials;
        public string path;

        public MtlFileParser(string filepath)
        {
            if (File.Exists(filepath))
            {
                if (Path.GetExtension(filepath) != ".mtl")
                {
                    throw new Exception("An invalid file type was used to construct a ObjObject. Only correctly formatted .mtl files will work.");
                }
                path = filepath;
                Materials = new Dictionary<string, Material>();
                string[] lines = File.ReadLines(filepath, Encoding.ASCII).ToArray<string>();
                List<string[]> currentMaterialArgs = new List<string[]>();
                bool firstMaterialStarted = false;
                foreach (string s in lines)
                {
                    string[] items = s.Split(' ');
                    if (items[0] == "newmtl")
                    {
                        firstMaterialStarted = true;
                        if (currentMaterialArgs.Count > 0)
                        {
                            CreateMaterialFromArgs(currentMaterialArgs);
                            currentMaterialArgs.Clear();
                        }
                        currentMaterialArgs.Add(items);
                    }
                    else if(firstMaterialStarted)
                    {
                        currentMaterialArgs.Add(items);
                    }
                }
                if (currentMaterialArgs.Count > 0) { CreateMaterialFromArgs(currentMaterialArgs); }
            }
        }        

        private void CreateMaterialFromArgs(List<string[]> materialArgs)
        {
            string materialName = "";
            float alpha = 0, red = 0, green = 0, blue = 0;
            string texturePath = "";
            foreach (string[] arg in materialArgs)
            {
                if (arg[0] == "newmtl") { materialName = arg[1]; }
                else if (arg[0] == "Ka") { /*Not supporting ambient color*/ }
                else if (arg[0] == "Kd") { red = float.Parse(arg[1]); green = float.Parse(arg[2]); blue = float.Parse(arg[3]); }
                else if (arg[0] == "Ks") { /*Not supporting specular color either*/}
                else if (arg[0] == "d") { alpha = float.Parse(arg[1]); }
                else if (arg[0] == "Tr") { alpha = 1 - float.Parse(arg[1]); }
                else if (arg[0] == "map_Kd") { texturePath = CorrectSplitsInTexturePaths(arg); }
            }

            if (materialName == "")
            {
                throw new Exception("A material without a name was encountered");
            }

            Material material = new Material();
            material.materialName = materialName;
            material.texturePath = texturePath;
            material.Color = Color.FromArgb((int)(255 * alpha), (int)(255 * red), (int)(255 * green), (int)(255 * blue));

            Materials.Add(materialName, material);
        }

        private string CorrectSplitsInTexturePaths(string[] args)
        {
            string retval = args[1];
            for (int i = 2; i < args.Length; i++)
            {
                retval += " " + args[i];
            }
            return retval;
        }

        public class Material
        {
            public string materialName;
            public string texturePath = "";
            public float Umin, Umax, Vmin, Vmax;
            public Color Color;
        }
    }
}

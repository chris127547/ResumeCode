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
        public Bitmap textureAtlas;

        public MtlFileParser(string filepath)
        {
            if (File.Exists(filepath))
            {
                if (Path.GetExtension(filepath) != ".mtl")
                {
                    throw new Exception("An invalid file type was used to construct a ObjObject. Only correctly formatted .mtl files will work.");
                }
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

                CreateTextureAtlas(Materials.ToList(), Path.GetDirectoryName(filepath));
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
                else if (arg[0] == "map_Kd") { texturePath = arg[1]; }
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

        private void CreateTextureAtlas(List<KeyValuePair<string, Material>> Materials, string path)
        {
            int width = 0, height = 0;
            List<Bitmap> bitmaps = new List<Bitmap>();
            foreach (KeyValuePair<string, Material> material in Materials)
            {
                if (material.Value.texturePath != "")
                {
                    Bitmap image = new Bitmap(path + "\\" + material.Value.texturePath);
                    width += image.Width;
                    if (image.Height > height) { height = image.Height; }
                    bitmaps.Add(image);
                }                
            }
            Bitmap atlas = new Bitmap(width + 1, height);
            int Xpos = 0;
            foreach (Bitmap map in bitmaps)
            {
                for (int i = Xpos; i < Xpos + map.Width; i++)
                {
                    for (int j = 0; j < map.Height; j++)
                    {
                        atlas.SetPixel(i, j, map.GetPixel(i - Xpos, j));
                    }
                }
                Xpos += map.Width;
            }
            atlas.SetPixel(atlas.Width - 1, 0, Color.FromArgb(255, 255, 255, 255));
            textureAtlas = atlas;

            AssignMaterialUVs(bitmaps, Materials);
            //FileStream stream = File.OpenWrite(path + "\\outputfile.jpeg");
            //atlas.Save(stream, ImageFormat.Jpeg);
            foreach (Bitmap bitmap in bitmaps)
            {
                bitmap.Dispose();
            }
        }

        private void AssignMaterialUVs(List<Bitmap> bitmaps, List<KeyValuePair<string, Material>> Materials)
        {
            int mapIndex = 0;
            float widthOffset = 0;
            foreach (KeyValuePair<string, Material> material in Materials)
            {
                if (material.Value.texturePath != "")
                {
                    Bitmap map = bitmaps[mapIndex];
                    float adjustedWidth = (float)map.Width / (float)textureAtlas.Width;
                    float adjustedHeight = (float)map.Height / (float)textureAtlas.Height;
                    material.Value.Umin = widthOffset;
                    material.Value.Umax = widthOffset + adjustedWidth;
                    material.Value.Vmin = 0;
                    material.Value.Vmax = adjustedHeight;

                    widthOffset += adjustedWidth;
                    mapIndex++;
                }
                else
                {
                    material.Value.Umax = 1;
                    material.Value.Umin = 1;
                    material.Value.Vmax = 0;
                    material.Value.Vmin = 0;
                }
            }
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

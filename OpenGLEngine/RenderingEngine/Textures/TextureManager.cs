using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Textures
{
    public class TextureManager
    {
        Dictionary<string, int> textures;

        public TextureManager()
        {
            textures = new Dictionary<string, int>();
        }

        public int LoadTexture(string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                Bitmap TextureBitmap = new Bitmap(filepath);
                return LoadTexture(TextureBitmap, filepath);
            }
            else
            {
                throw new Exception("Could not find texture file.");
            }
        }

        public int LoadTexture(Bitmap bitmap, string key)
        {
            if (textures.ContainsKey(key)) { return textures[key]; }
            System.Drawing.Imaging.BitmapData TextureData =
                bitmap.LockBits(
                        new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureData.Width, TextureData.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, TextureData.Scan0);

            bitmap.UnlockBits(TextureData);

            textures.Add(key, texture);

            return texture;
        }

        public void ClearTextures()
        {
            foreach (KeyValuePair<string, int> texture in textures)
            {
                GL.DeleteTexture(texture.Value);
            }
            textures.Clear();
        }

        public int GetTexture(string key)
        {
            if (textures.ContainsKey(key))
            {
                return textures[key];
            }
            else { return -1; }
        }
    }
}

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine.Textures
{
    public class TextureManager
    {
        public static int LoadTexture(string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
                Bitmap TextureBitmap = new Bitmap(filepath);
                System.Drawing.Imaging.BitmapData TextureData =
                TextureBitmap.LockBits(
                        new System.Drawing.Rectangle(0, 0, TextureBitmap.Width, TextureBitmap.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

                int texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureData.Width, TextureData.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, TextureData.Scan0);

                TextureBitmap.UnlockBits(TextureData);
                return texture;
            }
            else
            {
                throw new Exception("Could not find texture file.");
            }
        }
    }
}

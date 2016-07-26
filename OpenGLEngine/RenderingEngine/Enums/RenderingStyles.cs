using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Enums
{
    public enum RenderingStyle
    {
        SimpleSolidColors,
        SimpleTextureWithOptionalColor,
        TextureOptionalColorAndLighting,
        TextureAndLightingWithNoColorHighlights,
        ColorAndLightingWithNoTextures,
        SkeletonColorAndLightingWithNoTextures
    }
}

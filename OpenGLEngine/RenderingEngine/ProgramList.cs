using OpenGLEngine.RenderingEngine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine
{
    public class ProgramList
    {
        private SimpleColorProgram simpleColorProgram;
        public SimpleColorProgram SimpleColorProgram
        {
            get
            {
                if (simpleColorProgram == null) { simpleColorProgram = new SimpleColorProgram(); }
                return simpleColorProgram;
            }
        }

        private SimpleTextureProgram simpleTextureProgram;
        public SimpleTextureProgram SimpleTextureProgram
        {
            get
            {
                if (simpleTextureProgram == null) { simpleTextureProgram = new SimpleTextureProgram(); }
                return simpleTextureProgram;
            }
        }

        private TextureWithLightingProgram textureWithLightingProgram;
        public TextureWithLightingProgram TextureWithLightingProgram
        {
            get
            {
                if (textureWithLightingProgram == null) { textureWithLightingProgram = new TextureWithLightingProgram(); }
                return textureWithLightingProgram;
            }
        }

        private TextureWithLightingButNoColorProgram textureWithLightingButNoColorProgram;
        public TextureWithLightingButNoColorProgram TextureWithLightingButNoColorProgram
        {
            get
            {
                if (textureWithLightingButNoColorProgram == null) { textureWithLightingButNoColorProgram = new TextureWithLightingButNoColorProgram(); }
                return textureWithLightingButNoColorProgram;
            }
        }

        private ColorWithLightingButNoTextureProgram colorWithLightingButNoTextureProgram;
        public ColorWithLightingButNoTextureProgram ColorWithLightingButNoTextureProgram
        {
            get
            {
                if (colorWithLightingButNoTextureProgram == null) { colorWithLightingButNoTextureProgram = new ColorWithLightingButNoTextureProgram(); }
                return colorWithLightingButNoTextureProgram;
            }
        }

        private SkeletonColorWithLightingButNoTextureProgram skeletonColorWithLightingButNoTextureProgram;
        public SkeletonColorWithLightingButNoTextureProgram SkeletonColorWithLightingButNoTextureProgram
        {
            get
            {
                if (skeletonColorWithLightingButNoTextureProgram == null) { skeletonColorWithLightingButNoTextureProgram = new SkeletonColorWithLightingButNoTextureProgram(); }
                return skeletonColorWithLightingButNoTextureProgram;
            }
        }

        private SkeletonLightingColorAndTextureProgram skeletonLightingColorAndTextureProgram;
        public SkeletonLightingColorAndTextureProgram SkeletonLightingColorAndTextureProgram
        {
            get
            {
                if (skeletonLightingColorAndTextureProgram == null) { skeletonLightingColorAndTextureProgram = new SkeletonLightingColorAndTextureProgram(); }
                return skeletonLightingColorAndTextureProgram;
            }
        }
    }
}

using OpenGLEngine.Rendering_Engine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine
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
    }
}

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine.Programs
{
    public class SimpleColorProgram
    {
        public int MVPMatrixHandle;
        public int positionHandle;
        public int colorHandle;
        public int programHandle;

        public SimpleColorProgram()
        {
            int vertexShader = ProgramCreatorHelper.CreateShader(ShaderType.VertexShader, ShaderCodeDump.GetSimpleColorVertexShader());
            int fragmentShader = ProgramCreatorHelper.CreateShader(ShaderType.FragmentShader, ShaderCodeDump.GetSimpleColorFragmentShader());
            programHandle = ProgramCreatorHelper.CreateProgram(vertexShader, fragmentShader);
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "u_MVPMatrix");
            positionHandle = GL.GetAttribLocation(programHandle, "a_Position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_Color");
        }
    }
}

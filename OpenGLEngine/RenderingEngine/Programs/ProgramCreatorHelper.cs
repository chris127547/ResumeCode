using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine.Programs
{
    static class ProgramCreatorHelper
    {
        public static int CreateShader(ShaderType shaderType, string source)
        {
            int shaderHandle = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderHandle, source);
            GL.CompileShader(shaderHandle);

            int statusCode = -1;
            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string logtxt = "";
                logtxt = GL.GetShaderInfoLog(shaderHandle);
                throw new Exception("Shader compliation unsuccessful " + logtxt);
            }
            return shaderHandle;
        }

        public static int CreateProgram(int vertexShaderHandle, int fragmentShaderHandle)
        {
            int programHandle = GL.CreateProgram();
            GL.AttachShader(programHandle, vertexShaderHandle);
            GL.AttachShader(programHandle, fragmentShaderHandle);
            GL.LinkProgram(programHandle);

            int statusCode = -1;
            GL.GetProgram(programHandle, GetProgramParameterName.LinkStatus, out statusCode);
            if (statusCode != 1)
            {
                string logtxt = "";
                GL.GetProgramInfoLog(programHandle, out logtxt);
                throw new Exception("Error Linking Program " + logtxt);
            }
            return programHandle;
        }
    }
}

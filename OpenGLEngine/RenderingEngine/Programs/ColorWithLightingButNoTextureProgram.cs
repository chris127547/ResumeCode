using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Programs
{
    public class ColorWithLightingButNoTextureProgram
    {
        public int programHandle;

        public int MVPMatrixHandle;
        public int lightPositionHandle;
        public int diffuseLightHandle;
        public int modelMatrixHandle;
        public int normalModelMatrixHandle;

        public int positionHandle;
        public int normalHandle;
        public int colorHandle;

        public ColorWithLightingButNoTextureProgram()
        {
            int vertexShader = ProgramCreatorHelper.CreateShader(ShaderType.VertexShader, ShaderCodeDump.GetColorAndLightingButNoTextureVertexShader());
            int fragmentShader = ProgramCreatorHelper.CreateShader(ShaderType.FragmentShader, ShaderCodeDump.GetColorAndLightingButNoTextureFragmentShader());
            programHandle = ProgramCreatorHelper.CreateProgram(vertexShader, fragmentShader);
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "u_MVPMatrix");
            modelMatrixHandle = GL.GetUniformLocation(programHandle, "u_ModelMatrix");
            normalModelMatrixHandle = GL.GetUniformLocation(programHandle, "u_NormalMatrix");
            lightPositionHandle = GL.GetUniformLocation(programHandle, "u_LightPos");
            diffuseLightHandle = GL.GetUniformLocation(programHandle, "u_DiffuseLight");

            positionHandle = GL.GetAttribLocation(programHandle, "a_position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_color");
            normalHandle = GL.GetAttribLocation(programHandle, "a_normal");
        }
    }
}

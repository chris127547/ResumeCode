using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Programs
{
    public class SkeletonColorWithLightingButNoTextureProgram
    {
        public int programHandle;

        public int MVPMatrixHandle;
        public int lightPositionHandle;
        public int modelMatrixHandle;
        public int normalModelMatrixHandle;
        public int boneArrayHandle;

        public int positionHandle;
        public int normalHandle;
        public int colorHandle;
        public int boneIndexHandle;
        public int boneWeightHandle;

        public SkeletonColorWithLightingButNoTextureProgram()
        {
            int vertexShader = ProgramCreatorHelper.CreateShader(ShaderType.VertexShader, ShaderCodeDump.GetLightingAndColorSkeletonAnimationVertexShader());
            int fragmentShader = ProgramCreatorHelper.CreateShader(ShaderType.FragmentShader, ShaderCodeDump.GetLightingAndColorSkeletonAnimationFragmentShader());
            programHandle = ProgramCreatorHelper.CreateProgram(vertexShader, fragmentShader);
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "u_MVPMatrix");
            modelMatrixHandle = GL.GetUniformLocation(programHandle, "u_ModelMatrix");
            normalModelMatrixHandle = GL.GetUniformLocation(programHandle, "u_NormalMatrix");
            lightPositionHandle = GL.GetUniformLocation(programHandle, "u_LightPos");
            boneArrayHandle = GL.GetUniformLocation(programHandle, "u_Bone");

            positionHandle = GL.GetAttribLocation(programHandle, "a_position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_color");
            normalHandle = GL.GetAttribLocation(programHandle, "a_normal");
            boneIndexHandle = GL.GetAttribLocation(programHandle, "a_boneIndex");
            boneWeightHandle = GL.GetAttribLocation(programHandle, "a_boneWeight");
        }
    }
}

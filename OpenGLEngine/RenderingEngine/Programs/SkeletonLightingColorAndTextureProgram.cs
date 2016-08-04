using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Programs
{
    public class SkeletonLightingColorAndTextureProgram
    {

        public int programHandle;

        public int MVPMatrixHandle;
        public int lightPositionHandle;
        public int modelMatrixHandle;
        public int boneArrayHandle;
        public int normalArrayHandle;
        public int normalModelMatrixHandle;

        public int positionHandle;
        public int normalHandle;
        public int colorHandle;
        public int textureHandle;
        public int boneIndexHandle;
        public int boneWeightHandle;

        public SkeletonLightingColorAndTextureProgram()
        {
            int vertexShader = ProgramCreatorHelper.CreateShader(ShaderType.VertexShader, ShaderCodeDump.GetLightingAndTextureSkeletonAnimationVertexShader());
            int fragmentShader = ProgramCreatorHelper.CreateShader(ShaderType.FragmentShader, ShaderCodeDump.GetLightingAndTextureSkeletonAnimationFragmentShader());
            programHandle = ProgramCreatorHelper.CreateProgram(vertexShader, fragmentShader);
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "u_MVPMatrix");
            modelMatrixHandle = GL.GetUniformLocation(programHandle, "u_ModelMatrix");
            lightPositionHandle = GL.GetUniformLocation(programHandle, "u_LightPos");
            boneArrayHandle = GL.GetUniformLocation(programHandle, "u_Bone");
            normalArrayHandle = GL.GetUniformLocation(programHandle, "u_NormalBone");
            normalModelMatrixHandle = GL.GetUniformLocation(programHandle, "u_NormalMatrix");

            positionHandle = GL.GetAttribLocation(programHandle, "a_position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_color");
            normalHandle = GL.GetAttribLocation(programHandle, "a_normal");
            textureHandle = GL.GetAttribLocation(programHandle, "a_texcord");
            boneIndexHandle = GL.GetAttribLocation(programHandle, "a_boneIndex");
            boneWeightHandle = GL.GetAttribLocation(programHandle, "a_boneWeight");
        }
    }
}

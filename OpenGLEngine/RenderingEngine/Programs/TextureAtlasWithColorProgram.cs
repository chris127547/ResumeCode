﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Programs
{
    public class TextureAtlasWithColorProgram
    {
        public int programHandle;
        public int MVPMatrixHandle;
        public int positionHandle;
        public int colorHandle;
        public int textureHandle;
        public int textureLowHandle;
        public int textureIncrementHandle;

        public TextureAtlasWithColorProgram()
        {
            int vertexShader = ProgramCreatorHelper.CreateShader(ShaderType.VertexShader, ShaderCodeDump.GetTextureAtlasWithColorVertexShader());
            int fragmentShader = ProgramCreatorHelper.CreateShader(ShaderType.FragmentShader, ShaderCodeDump.GetTextureAtlasWithColorFragmentShader());
            programHandle = ProgramCreatorHelper.CreateProgram(vertexShader, fragmentShader);
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "u_mvpmatrix");
            textureLowHandle = GL.GetUniformLocation(programHandle, "u_texlow");
            textureIncrementHandle = GL.GetUniformLocation(programHandle, "u_texincrement");            
            positionHandle = GL.GetAttribLocation(programHandle, "a_position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_color");
            textureHandle = GL.GetAttribLocation(programHandle, "a_texcord");            
        }
    }
}

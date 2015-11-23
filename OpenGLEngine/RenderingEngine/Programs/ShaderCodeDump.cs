﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine.Programs
{
    public static class ShaderCodeDump
    {

        public static string GetSimpleColorVertexShader()
        {
            string vertexshader =
                            "uniform mat4 u_MVPMatrix;      \n"
                          + "attribute vec4 a_Position;     \n"
                          + "attribute vec4 a_Color;        \n"
                          + "varying vec4 v_Color;          \n"
                          + "void main()                    \n"
                          + "{                              \n"
                          + "   v_Color = a_Color;          \n"
                          + "   gl_Position = u_MVPMatrix   \n"
                          + "               * a_Position;   \n"
                          + "}                              \n";
            return vertexshader;
        }
        public static string GetSimpleColorFragmentShader()
        {
            string fragmentshader =
                            "varying vec4 v_Color;          \n"
                          + "void main()                    \n"
                          + "{                              \n"
                          + "   gl_FragColor = v_Color;     \n"
                          + "}                              \n";
            return fragmentshader;
        }

        public static string GetSimpleTextureVertexShader()
        {
            string vertexshader =
                "uniform mat4 u_mvpmatrix;   \n"
                + "attribute vec4 a_position; \n"
                + "attribute vec4 a_color; \n"
                + "attribute vec2 a_texcord; \n"
                + "varying vec4 v_color; \n"
                + "varying vec2 v_texcord; \n"
                + "void main(){ \n"
                + "v_color = a_color; \n"
                + "v_texcord = a_texcord; \n"
                + "gl_Position = u_mvpmatrix*a_position;} \n";
            return vertexshader;
        }
        public static string GetSimpleTextureFragmentShader()
        {
            string fragmentshader =
                "uniform sampler2D u_texture; \n"
                + "varying vec4 v_color; \n"
                + "varying vec2 v_texcord; \n"
                + "void main(){ \n"
                + "gl_FragColor = v_color * texture2D(u_texture, v_texcord);} \n";
            return fragmentshader;
        }

        public static string GetTextureAndLightingVertexShader()
        {
            string vertexshader =
                " uniform mat4 u_MVPMatrix; \n"

              + "attribute vec4 a_position; \n"
              + "attribute vec4 a_color; \n"
              + "attribute vec3 a_normal;       \n"
              + "attribute vec2 a_texcord; \n"

              + " varying vec4 v_position; \n"
              + " varying vec4 v_color; \n"
              + " varying vec2 v_texcord; \n"
              + " varying vec3 v_normal; \n"

              + " void main() { \n"

              + "     v_position = a_position; \n"
              + "     v_texcord = a_texcord; \n"
              + "     v_normal = a_normal; \n"
              + "     v_color = a_color; \n"

              + "     gl_Position = u_MVPMatrix * a_position; \n"
              + " }";
            return vertexshader;
        }
        public static string GetTextureAndLightingFragmentShader()
        {
            string fragmentshader =
                  " uniform mat4 u_ModelMatrix; \n"
                + " uniform mat3 u_NormalMatrix; \n"
                + " uniform sampler2D u_texture; \n"
                + " uniform vec3 u_LightPos; \n"

                + " varying vec4 v_position; \n"
                + " varying vec4 v_color; \n"
                + " varying vec2 v_texcord; \n"
                + " varying vec3 v_normal; \n"

                + " void main() { \n"
                + "    vec3 normal = normalize(u_NormalMatrix * v_normal); \n"

                + "    vec3 fragPosition = vec3(u_ModelMatrix * v_position); \n"

                + "    vec3 surfaceToLight = u_LightPos - fragPosition; \n"

                + "    float brightness = dot(normal, surfaceToLight) / (length(surfaceToLight) * length(normal)); \n"
                + "    brightness = max(brightness, 0.3f); \n"
                + "    vec4 surfaceColor = texture2D(u_texture, v_texcord); \n"
                + "    surfaceColor = surfaceColor * v_color; \n"
                + "    gl_FragColor = vec4(brightness * surfaceColor.rgb, surfaceColor.a); \n"
                + " }";
            return fragmentshader;
        }
    }
}

using OpenGLEngine.RenderingEngine.Cameras;
using OpenGLEngine.RenderingEngine.Programs;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers.Skeleton
{
    public class SkeletonLightingAndColorRenderer : SkeletonRenderer
    {
        int shapeData;
        int indiceData;
        int triangleCount;
        SkeletonColorWithLightingButNoTextureProgram program;
        Camera camera;
        Engine engine;

        public SkeletonLightingAndColorRenderer(int shapeData, int indiceData, int triangleCount, Engine engine)
        {
            this.shapeData = shapeData; this.indiceData = indiceData; this.triangleCount = triangleCount;
            this.camera = engine.camera; program = engine.programList.SkeletonColorWithLightingButNoTextureProgram;
            this.engine = engine;
        }

        public void Render(Matrix4 modelMatrix, Matrix4[] skeleton)
        {
            Matrix4 model = modelMatrix;
            Matrix4 MVP = (model * camera.ViewMatrix) * camera.ProjectionMatrix;
            Matrix3 normalModel = new Matrix3(Matrix4.Transpose(Matrix4.Invert(model)));
            float[] skeletonValues = Matrix4ArrayToFloatArray(skeleton);
            float[] normalValues = Matrix4ArrayToInvertedAndTransposedMatrix3FloatArray(skeleton);
            GLErrorHelper.CheckError();
            GL.UseProgram(program.programHandle);

            GL.Uniform3(program.lightPositionHandle, ref engine.light.LightPosition);

            GL.UniformMatrix4(program.modelMatrixHandle, false, ref model);
            GL.UniformMatrix4(program.MVPMatrixHandle, false, ref MVP);
            GL.UniformMatrix4(program.boneArrayHandle, skeleton.Length, false, skeletonValues);
            GL.UniformMatrix3(program.normalArrayHandle, skeleton.Length, false, normalValues);
            GL.UniformMatrix3(program.normalModelMatrixHandle, false, ref normalModel);
            GLErrorHelper.CheckError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shapeData);

            GL.EnableVertexAttribArray(program.positionHandle);
            GL.VertexAttribPointer(program.positionHandle, 3, VertexAttribPointerType.Float, false, 56, 0);

            GL.EnableVertexAttribArray(program.normalHandle);
            GL.VertexAttribPointer(program.normalHandle, 3, VertexAttribPointerType.Float, false, 56, 12);

            GL.EnableVertexAttribArray(program.colorHandle);
            GL.VertexAttribPointer(program.colorHandle, 4, VertexAttribPointerType.Float, false, 56, 24);
            
            GL.EnableVertexAttribArray(program.boneIndexHandle);
            GL.VertexAttribPointer(program.boneIndexHandle, 2, VertexAttribPointerType.Float, false, 56, 40);
            
            GL.EnableVertexAttribArray(program.boneWeightHandle);
            GL.VertexAttribPointer(program.boneWeightHandle, 2, VertexAttribPointerType.Float, false, 56, 48);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indiceData);
            GL.DrawElements(PrimitiveType.Triangles, triangleCount, DrawElementsType.UnsignedInt, (IntPtr)null);
            GLErrorHelper.CheckError();
        }

        private float[] Matrix4ArrayToFloatArray(Matrix4[] array)
        {
            float[] output = new float[array.Length * 16];
            for (int i = 0; i < array.Length; i++)
            {
                int outputIndex = i * 16;
                output[outputIndex++] = array[i].M11;
                output[outputIndex++] = array[i].M12;
                output[outputIndex++] = array[i].M13;
                output[outputIndex++] = array[i].M14;
                output[outputIndex++] = array[i].M21;
                output[outputIndex++] = array[i].M22;
                output[outputIndex++] = array[i].M23;
                output[outputIndex++] = array[i].M24;
                output[outputIndex++] = array[i].M31;
                output[outputIndex++] = array[i].M32;
                output[outputIndex++] = array[i].M33;
                output[outputIndex++] = array[i].M34;
                output[outputIndex++] = array[i].M41;
                output[outputIndex++] = array[i].M42;
                output[outputIndex++] = array[i].M43;
                output[outputIndex++] = array[i].M44;
            }
            return output;
        }

        private float[] Matrix4ArrayToInvertedAndTransposedMatrix3FloatArray(Matrix4[] array)
        {
            float[] output = new float[array.Length * 9];
            for (int i = 0; i < array.Length; i++)
            {
                Matrix3 updatedArray = new Matrix3(Matrix4.Transpose(Matrix4.Invert(array[i])));
                int outputIndex = i * 9;
                output[outputIndex++] = updatedArray.M11;
                output[outputIndex++] = updatedArray.M12;
                output[outputIndex++] = updatedArray.M13;
                output[outputIndex++] = updatedArray.M21;
                output[outputIndex++] = updatedArray.M22;
                output[outputIndex++] = updatedArray.M23;
                output[outputIndex++] = updatedArray.M31;
                output[outputIndex++] = updatedArray.M32;
                output[outputIndex++] = updatedArray.M33;
            }
            return output;
        }
    }
}

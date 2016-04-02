using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Enums;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingProject
{
    class Program
    {
        private static string fileToRender = "C:\\Users\\Chris\\Documents\\3D models\\Monster Parts\\Deer Parts\\deertorso.ply";

        static Engine engine;
        static private Light light;

        static void Main(string[] args)
        {
            engine = new Engine();
            engine.clearColor = new float[] { 0.4f, 0.7f, 1f, 1 };
            light = new Light(new Vector3(-10, 0, 10));

            //engine.renderedObjects.Add(new Square(engine));
            //engine.renderedObjects.Add(new PlyFileObject(engine, new float[]{1,1,1,1}, RenderingStyle.ColorAndLightingWithNoTextures, fileToRender));
            //engine.renderedObjects.Add(new PlyFileCube(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.ColorAndLightingWithNoTextures));
            //engine.renderedObjects.Add(new PlyFileCube(engine, null, RenderingStyle.TextureAndLightingWithNoColorHighlights));


            engine.renderedObjects.Add(new PlyFileObject(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.ColorAndLightingWithNoTextures));
            engine.renderedObjects.Add(new Square(engine));
            engine.game.RenderFrame += MoveLight;
            engine.Start();
        }

        private static void MoveLight(object sender, EventArgs e)
        {
            float angle = (float)((double)(System.DateTime.Now.Millisecond + ((System.DateTime.Now.Second % 5) * 1000)) / 5000) * 360;
            Vector3 light2 = RotatePointHorizontally(light.LightPosition, new Vector3(0, 0, 0), angle);
            engine.light = new Light(light2);
        }

        public static Vector3 RotatePointHorizontally(Vector3 point, Vector3 around, float degree)
        {
            double radians = MathHelper.DegreesToRadians(degree);
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);

            float X = (float)(cosTheta * (point.X - around.X) - sinTheta * (point.Y - around.Y) + around.X);
            float Z = (float)(sinTheta * (point.X - around.X) - cosTheta * (point.Y - around.Y) + around.Y);

            return new Vector3(X, 0, Z);
        }
    }
}

using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderingEngine;
using OpenGLEngine.RenderingEngine.Enums;
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

        static void Main(string[] args)
        {
            Engine engine = new Engine();
            engine.clearColor = new float[] { 0.4f, 0.7f, 1f, 1 };

            //engine.renderedObjects.Add(new Square(engine));
            //engine.renderedObjects.Add(new PlyFileObject(engine, new float[]{1,1,1,1}, RenderingStyle.ColorAndLightingWithNoTextures, fileToRender));
            //engine.renderedObjects.Add(new PlyFileCube(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.ColorAndLightingWithNoTextures));
            //engine.renderedObjects.Add(new PlyFileCube(engine, null, RenderingStyle.TextureAndLightingWithNoColorHighlights));

            engine.renderedObjects.Add(new OpenGLEngine.RenderedObjects.Square(engine));
            engine.renderedObjects.Add(new OpenGLEngine.RenderedObjects.PlyFileObject(engine, new float[] { 1, 0.5f, 0.5f, 1 }, RenderingStyle.ColorAndLightingWithNoTextures));
            engine.Start();
        }
    }
}

using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderedObjects.FileToObjectConverters;
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
        //private static string fileToRender = "C:\\Users\\Chris\\Documents\\3D models\\Monster Parts\\Deer Parts\\Torso\\deertorso.ply";
        private static string fileToRender = "C:\\Users\\Chris\\Documents\\3D models\\normalcube.ply";

        static Engine engine;
        static private Light light;

        static void Main(string[] args)
        {
            //oldTest();
            string materialPath = "C:\\Users\\Chris\\Documents\\3D models\\Downloaded\\js18ym62b5-ShuttleRayderSidonia\\Shuttle Rayder Sydonia\\Shuttle_Rayder_Sydonia.mtl";

            //MtlFileParser mtlParser = new MtlFileParser(materialPath);

            string shuttlePath = "C:\\Users\\Chris\\Documents\\3D models\\Downloaded\\js18ym62b5-ShuttleRayderSidonia\\Shuttle Rayder Sydonia\\Shuttle Rayder Sydonia.obj";
            string testobj = "C:\\Users\\Chris\\Documents\\3D models\\testcube.obj";
            ObjFileParser parser = new ObjFileParser(shuttlePath, new float[]{1,1,1,1});
            //Console.ReadKey();
            engine = new Engine();
            engine.clearColor = new float[] { 0.4f, 0.7f, 1f, 1 };
            light = new Light(new Vector3(-10, 0, 10));
            //GenericRenderedObject obj = new GenericRenderedObject(engine, new float[] { 1, 1, 1, 1 }, parser.vertices, parser.indices, RenderingStyle.ColorAndLightingWithNoTextures);
            int texId = engine.textureManager.LoadTexture(parser.material.textureAtlas, "test");
            GenericRenderedObject obj = new GenericRenderedObject(engine, new float[] { 1, 1, 1, 1 }, parser.vertices, parser.indices, texId, RenderingStyle.TextureColorAndLighting);
            engine.renderedObjects.Add(obj);
            engine.game.RenderFrame += MoveLight;
            engine.Start();
        }

        private static void oldTest()
        {
            engine = new Engine();
            engine.clearColor = new float[] { 0.4f, 0.7f, 1f, 1 };
            light = new Light(new Vector3(-10, 0, 10));

            //engine.renderedObjects.Add(new Square(engine));
            //engine.renderedObjects.Add(new PlyFileObject(engine, new float[]{1,1,1,1}, RenderingStyle.ColorAndLightingWithNoTextures, fileToRender));
            //engine.renderedObjects.Add(new PlyFileCube(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.ColorAndLightingWithNoTextures));
            //engine.renderedObjects.Add(new PlyFileCube(engine, null, RenderingStyle.TextureAndLightingWithNoColorHighlights));

            //PlyFileObject ply = new PlyFileObject(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.ColorAndLightingWithNoTextures, fileToRender);
            //PlyFileObject ply = new PlyFileObject(engine, new float[] { 1, 1, 1, 1 }, RenderingStyle.SkeletonColorAndLightingWithNoTextures, fileToRender);
            //PlyFileParser ply = new PlyFileParser(fileToRender, new float[] { 1, 1, 1, 1 });
            //AddBoneData(ply.vertices);
            //AddTextureData(ply.vertices);
            //PlyFileObject plyObject = new PlyFileObject(engine, RenderingStyle.ColorAndLightingWithNoTextures, ply);
            //SkeletonTestObject testObject = new SkeletonTestObject(engine, ply);

            //engine.renderedObjects.Add(testObject);
            int bread = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Textures\\bread.jpg");
            int sand = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Textures\\beach_sand.jpg");
            int concrete = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Textures\\concrete.jpg");
            bread = engine.LoadTexture("C:\\Users\\Chris\\Documents\\Image bin\\Textures\\bread.jpg");

            engine.renderedObjects.Add(new PlyFileCube(engine, null, RenderingStyle.TextureAndLightingWithNoColorHighlights, bread));
            PlyFileCube cube = new PlyFileCube(engine, null, RenderingStyle.TextureAndLightingWithNoColorHighlights, concrete);
            cube.position = Matrix4.CreateTranslation(new Vector3(-3, 0, 0));
            engine.renderedObjects.Add(cube);

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

        public static void AddBoneData(VertexList vertices)
        {
            Vector3 bottomBackLeft = new Vector3(-1, -1, -1);
            Vector3 bottomFrontLeft = new Vector3(-1, -1, 1);
            Vector3 topBackLeft = new Vector3(-1, 1, -1);
            Vector3 topFrontLeft = new Vector3(-1, 1, 1);
            Vector3 bottomBackRight = new Vector3(1, -1, -1);
            Vector3 bottomFrontRight = new Vector3(1, -1, 1);
            Vector3 topBackRight = new Vector3(1, 1, -1);
            Vector3 topFrontRight = new Vector3(1, 1, 1);
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].position.vector == bottomBackLeft)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(1, 0);
                    vertices[i].texture = new Vertex.Texture(-1, -1);
                }
                if (vertices[i].position.vector == bottomFrontLeft)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(1, 0);
                    vertices[i].texture = new Vertex.Texture(-1, 1);
                }
                if (vertices[i].position.vector == topBackLeft)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(1, 0);
                    vertices[i].texture = new Vertex.Texture(-1, -1);
                }
                if (vertices[i].position.vector == topFrontLeft)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(1, 0);
                    vertices[i].texture = new Vertex.Texture(-1, 1);
                }
                if (vertices[i].position.vector == bottomBackRight)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(0, 1);
                    vertices[i].texture = new Vertex.Texture(-1, -1);
                }
                if (vertices[i].position.vector == bottomFrontRight)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(0, 1);
                    vertices[i].texture = new Vertex.Texture(-1, 1);
                }
                if (vertices[i].position.vector == topBackRight)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(0, 1);
                    vertices[i].texture = new Vertex.Texture(1, -1);
                }
                if (vertices[i].position.vector == topFrontRight)
                {
                    vertices[i].boneIndex = new Vertex.BoneIndex(0, 1);
                    vertices[i].boneWeight = new Vertex.BoneWeight(0, 1);
                    vertices[i].texture = new Vertex.Texture(1, 1);
                }
            }
        }

        private static void AddTextureData(VertexList verts)
        {
            Random rng = new Random();
            for (int i = 0; i < verts.Count; i++)
            {
                verts[i].texture = new Vertex.Texture(rng.Next(0, 1), rng.Next(0, 1));
            }
        }
    }
}

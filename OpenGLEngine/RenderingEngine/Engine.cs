using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenGLEngine.RenderedObjects;
using OpenGLEngine.RenderingEngine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLEngine.RenderingEngine.Textures;
using OpenGLEngine.RenderingEngine.Cameras;

namespace OpenGLEngine.RenderingEngine
{
    public class Engine
    {
        public GameWindow game = null;
        public Camera camera;
        public Light light;
        public List<RenderedObject> renderedObjects;
        public ProgramList programList;
        public TextureManager textureManager;
        public float[] clearColor = new float[]{1, 1, 1, 1};

        public Engine()
        {
            CreateEngine(new FreeCamera());
        }

        public Engine(Camera camera)
        {
            CreateEngine(camera);
        }

        public Engine(Camera camera, int windowWidth, int windowHeight)
        {
            CreateEngine(camera);
            game.Width = windowWidth; game.Height = windowHeight;
        }

        private void CreateEngine(Camera camera)
        {
            game = new GameWindow();
            this.camera = camera;
            light = new Light(5, 25, 30);
            renderedObjects = new List<RenderedObject>();
            programList = new ProgramList();
            textureManager = new TextureManager();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.VertexArray);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            game.Load += OnGameLoad;

            game.Resize += OnResize;

            game.UpdateFrame += OnUpdateFrame;

            game.RenderFrame += onRenderFrame;

            game.KeyDown += onKeyDown;

            game.Closing += onClosing;

        }
        public void Start()
        {
            game.Run(60.0);
        }

        public int LoadTexture(string filepath)
        {
            return textureManager.LoadTexture(filepath);
        }

        private void OnGameLoad(object sender, EventArgs e)
        {
            game.VSync = VSyncMode.On;
        }
        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, game.Width, game.Height);
        }
        private void OnUpdateFrame(object sender, EventArgs e)
        {
            KeyboardState state = Keyboard.GetState();
            if (state[Key.Escape])
            {
                game.Exit();
            }
            else
            {
                camera.OnUpdateFrame();                
            }
        }
        private void onRenderFrame(object sender, EventArgs e)
        {

            GL.ClearColor(clearColor[0], clearColor[1], clearColor[2], clearColor[3]);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < renderedObjects.Count; i++)
            {
                renderedObjects[i].Render();
            }
            game.SwapBuffers();
        }
        private void onKeyDown(object sender, KeyboardKeyEventArgs e)
        {

        }

        private void onClosing(object sender, EventArgs e)
        {
            for (int i = 0; i < renderedObjects.Count; i++)
            {
                renderedObjects[i].Delete();
            }
            textureManager.ClearTextures();
        }
    }
}

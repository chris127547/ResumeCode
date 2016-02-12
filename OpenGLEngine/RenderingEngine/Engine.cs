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

namespace OpenGLEngine.RenderingEngine
{
    public class Engine
    {
        public GameWindow game = null;
        public Camera camera;
        public Light light;
        public List<RenderedObject> renderedObjects;
        public ProgramList programList;
        public float[] clearColor = new float[]{1, 1, 1, 1};
        public Engine()
        {
            game = new GameWindow();
            camera = new Camera();
            light = new Light(5, 25, 30);
            renderedObjects = new List<RenderedObject>();
            programList = new ProgramList();

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

        private void OnGameLoad(object sender, EventArgs e)
        {
            game.VSync = VSyncMode.On;
            //renderedObjects.Add(new PlyFileCube(this, new float[] { 1, 0.5f, 0, 1 }, Enums.RenderingStyle.ColorAndLightingWithNoTextures));
            //renderedObjects.Add(new PlyFileCube(this, 2, 2, 2, new float[] { 1, 1, 1, 1 }));
            //renderedObjects.Add(new TexturedBoxWithLighting(this, 2, 2, 2, new float[] { 1, 1, 1, 1 }));
            //renderedObjects.Add(new TexturedBox(this, 2, 2, 2, new float[] { 0.2f, 0.2f, 1, 0.8f }));
            //renderedObjects.Add(new BoxShape(this, 2, 2, 2, new float[] { 1, 1, 1, 1 }));
            //renderedObjects.Add(new Square(this));
            //renderedObjects.Add(new DrawingTestShape(this));
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
                if (state[Key.Up])
                {
                    camera.LookUpOrDown(true);
                }
                if (state[Key.Down])
                {
                    camera.LookUpOrDown(false);
                }
                if (state[Key.Left])
                {
                    camera.Turn(false);
                }
                if (state[Key.Right])
                {
                    camera.Turn(true);
                }
                if (state[Key.W])
                {
                    camera.MoveForward();
                }
                if (state[Key.S])
                {
                    camera.MoveBackward();
                }
                if (state[Key.A])
                {
                    camera.StrafeLeft();
                }
                if (state[Key.D])
                {
                    camera.StrafeRight();
                }
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
        }

    }
}

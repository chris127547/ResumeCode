using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenGLEngine.RenderedObjects;
using OpenGLEngine.Rendering_Engine.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.Rendering_Engine
{
    public class Engine
    {
        GameWindow game = null;
        SimpleColorProgram program = null;
        public Camera camera;
        public List<RenderedObject> renderedObjects;
        public ProgramList programList;
        public Engine()
        {
            game = new GameWindow();
            camera = new Camera();
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

        }
        public void Start()
        {
            game.Run(60.0);
        }

        private void OnGameLoad(object sender, EventArgs e)
        {
            game.VSync = VSyncMode.On;
            program = new SimpleColorProgram();
            renderedObjects.Add(new TexturedBoxWithLighting(this, 2, 2, 2, new float[] { 1, 1, 1, 1 }));
            //renderedObjects.Add(new TexturedBox(this, 2, 2, 2, new float[] { 0.2f, 0.2f, 1, 0.8f }));
            //renderedObjects.Add(new BoxShape(this, 2, 2, 2, new float[] { 1, 1, 1, 1 }));
            renderedObjects.Add(new Square(this));
            renderedObjects.Add(new DrawingTestShape(this));
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

            GL.ClearColor(255f, 0, 150f, 255f);
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
    }
}

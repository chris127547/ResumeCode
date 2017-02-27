using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Cameras
{
    public class FreeCamera : Camera
    {
        private Matrix4 projection;
        public Matrix4 ProjectionMatrix { get { return projection; } }
        private Matrix4 view;
        public Matrix4 ViewMatrix { get { return view; } }
        private Matrix4 translation = Matrix4.Identity;
        private Matrix4 rotation = Matrix4.Identity;
        private float transX = 0, transY = 0, transZ = 0;
        private float leftright = 0, updown = 0;

        public FreeCamera()
        {
            projection = Matrix4.CreatePerspectiveOffCenter(-1, 1, -1, 1, 2, 100);
            view = Matrix4.LookAt(new Vector3(0, 0, 0.1f), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }
        public FreeCamera(Matrix4 projection)
        {
            this.projection = projection;
            view = Matrix4.LookAt(new Vector3(4, 3, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        public void setPosition(Matrix4 lookat)
        {
            view = lookat;
        }
        public void Turn(bool right)
        {
            if (right)
            {
                leftright += 1;
                if (leftright > 360) { leftright = leftright - 360; }
            }
            else
            {
                leftright += -1;
                if (leftright <= 0) { leftright = leftright + 360; }
            }
            SetCameraPosition();
        }
        public void LookUpOrDown(bool up)
        {//restrict degrees to make work like FPS camera
            if (up)
            {
                updown += 1;
                if (updown > 360) { updown = updown - 360; }
            }
            else
            {
                updown += -1;
                if (updown <= 0) { updown = updown + 360; }
            }
            SetCameraPosition();
        }
        public void MoveForward()
        {
            TranslateCamera(.1f, false);
            SetCameraPosition();
        }
        public void MoveBackward()
        {
            TranslateCamera(-.1f, false);
            SetCameraPosition();
        }
        public void StrafeRight()
        {
            TranslateCamera(-.1f, true);
            SetCameraPosition();
        }
        public void StrafeLeft()
        {
            TranslateCamera(.1f, true);
            SetCameraPosition();
        }
        private void TranslateCamera(float speed, bool strafe)
        {
            float degreeoffset = 90;
            if (strafe) { degreeoffset = 0; }
            float xtran = (float)Math.Cos((Math.PI / 180) * (leftright + degreeoffset));
            float ztran = (float)Math.Sin((Math.PI / 180) * (leftright + degreeoffset));
            float ytran = (float)Math.Sin((Math.PI / 180) * (updown));
            float xmod = (float)Math.Cos((Math.PI / 180) * (updown));
            xtran = xtran * speed;
            ztran = ztran * speed;
            ytran = ytran * speed;
            xtran = xtran * xmod;
            ztran = ztran * xmod;
            transZ += ztran;
            transX += xtran;
            transY += ytran;

            /*float degreeoffset = 90; //This version works like FPS camera
            if (strafe) { degreeoffset = 0; }
            float xtran = (float)Math.Cos((Math.PI / 180) * (leftright + degreeoffset));
            float ztran = (float)Math.Sin((Math.PI / 180) * (leftright + degreeoffset));
            xtran = xtran * speed;
            ztran = ztran * speed;
            transZ += ztran;
            transX += xtran;*/
        }

        public void SetCameraPosition(float Xtranslation, float Ytranslation, float Ztranslation, float updown, float leftright)
        {
            transX = Xtranslation; transY = Ytranslation; transZ = Ztranslation;
            this.updown = updown; this.leftright = leftright;
            SetCameraPosition();
        }

        private void SetCameraPosition()
        {
            Matrix4 viewtemp = ApplyRotation(Matrix4.Identity, leftright, 0, 1, 0);
            viewtemp = ApplyRotation(viewtemp, updown, (float)Math.Cos((Math.PI / 180) * leftright), 0, (float)Math.Sin((Math.PI / 180) * leftright));
            translation = Matrix4.CreateTranslation(transX, transY, transZ);
            view = translation * viewtemp;
        }
        private Matrix4 ApplyRotation(Matrix4 matrix, float a, float x, float y, float z)
        {
            float[] rm = new float[16];
            rm[3] = 0;
            rm[7] = 0;
            rm[11] = 0;
            rm[12] = 0;
            rm[13] = 0;
            rm[14] = 0;
            rm[15] = 1;
            a = a * (float)(Math.PI / 180);
            float s = (float)Math.Sin(a);
            float c = (float)Math.Cos(a);
            if (x == 1.0f && y == 0 && z == 0)
            {
                rm[5] = c; rm[10] = c;
                rm[6] = s; rm[9] = -s;
                rm[1] = 0; rm[2] = 0;
                rm[4] = 0; rm[8] = 0;
                rm[0] = 1;
            }
            else if (x == 0 && y == 1.0f && z == 0)
            {
                rm[0] = c; rm[10] = c;
                rm[8] = s; rm[2] = -s;
                rm[1] = 0; rm[4] = 0;
                rm[6] = 0; rm[9] = 0;
                rm[5] = 1;
            }
            else if (x == 0 && y == 0 && z == 1.0f)
            {
                rm[0] = c; rm[5] = c;
                rm[1] = s; rm[4] = -s;
                rm[2] = 0; rm[6] = 0;
                rm[8] = 0; rm[9] = 0;
                rm[10] = 1;
            }
            else
            {
                float len = (float)Math.Sqrt((x * x) + (y * y) + (z * z));
                if (1.0f != len)
                {
                    float recipLen = 1.0f / len;
                    x *= recipLen;
                    y *= recipLen;
                    z *= recipLen;
                }
                float nc = 1.0f - c;
                float xy = x * y;
                float yz = y * z;
                float zx = z * x;
                float xs = x * s;
                float ys = y * s;
                float zs = z * s;
                rm[0] = x * x * nc + c;
                rm[4] = xy * nc - zs;
                rm[8] = zx * nc + ys;
                rm[1] = xy * nc + zs;
                rm[5] = y * y * nc + c;
                rm[9] = yz * nc - xs;
                rm[2] = zx * nc - ys;
                rm[6] = yz * nc + xs;
                rm[10] = z * z * nc + c;
            }
            Matrix4 rMatrix = new Matrix4(rm[0], rm[1], rm[2], rm[3], rm[4], rm[5], rm[6], rm[7], rm[8], rm[9], rm[10], rm[11], rm[12], rm[13], rm[14], rm[15]);
            return rMatrix * matrix;
        }


        public void OnUpdateFrame()
        {
            KeyboardState state = Keyboard.GetState();
            if (state[Key.Up])
            {
                LookUpOrDown(true);
            }
            if (state[Key.Down])
            {
                LookUpOrDown(false);
            }
            if (state[Key.Left])
            {
                Turn(false);
            }
            if (state[Key.Right])
            {
                Turn(true);
            }
            if (state[Key.W])
            {
                MoveForward();
            }
            if (state[Key.S])
            {
                MoveBackward();
            }
            if (state[Key.A])
            {
                StrafeLeft();
            }
            if (state[Key.D])
            {
                StrafeRight();
            }
        }
    }
}

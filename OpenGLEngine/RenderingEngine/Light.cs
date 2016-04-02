using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine
{
    public class Light
    {
        public Vector3 LightPosition;

        public Light(float posX, float posY, float posZ)
        {
            LightPosition = new Vector3(posX, posY, posZ);
        }

        public Light(Vector3 lightPosition)
        {
            LightPosition = lightPosition;
        }
    }
}

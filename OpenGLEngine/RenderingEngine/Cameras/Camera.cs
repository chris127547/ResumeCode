using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Cameras
{
    public interface Camera
    {
        Matrix4 ProjectionMatrix { get; }
        Matrix4 ViewMatrix { get; }
        void OnUpdateFrame();
    }
}

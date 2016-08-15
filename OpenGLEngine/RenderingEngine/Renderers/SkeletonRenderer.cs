using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers
{
    public interface SkeletonRenderer
    {
        void Render(Matrix4 modelMatrix, Matrix4[] skeleton);
    }
}

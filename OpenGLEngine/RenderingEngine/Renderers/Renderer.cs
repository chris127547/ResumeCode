using OpenGLEngine.RenderedObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderingEngine.Renderers
{
    public interface Renderer
    {
        void Render();

        void Render(Matrix4 modelMatrix);
    }
}

using OpenGLEngine.RenderedObjects.FileToObjectConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects
{
    public interface RenderedObject
    {
        void Render();

        void UpdateMesh(VertexList vertexList, int[] indices);

        void Delete();
    }
}

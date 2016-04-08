using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class Triangle
    {
        public Vertex Vertex1, Vertex2, Vertex3;
        public Vector3 Normal;

        public Triangle(Vertex Vertex1, Vertex Vertex2, Vertex Vertex3, Vector3 Normal)
        {
            this.Vertex1 = Vertex1; this.Vertex2 = Vertex2; this.Vertex3 = Vertex3; this.Normal = Normal;
        }

        public Triangle(Vertex Vertex1, Vertex Vertex2, Vertex Vertex3)
        {
            this.Vertex1 = Vertex1; this.Vertex2 = Vertex2; this.Vertex3 = Vertex3;
            Normal = CalculateSurfaceNormal();
        }

        private Vector3 CalculateSurfaceNormal()
        {
            Vector3 v2 = Vertex2.position.vector;
            Vector3 v3 = Vertex3.position.vector;
            Vector3 v1 = Vertex1.position.vector;
            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
            if (normal == new Vector3(0, 0, 0))
            {
                Vector3 temp = new Vector3(v1.X + 1, v1.Y + 1, v1.Z + 1);
                Vector3 temp2 = new Vector3(v2.X + 1, v2.Y + 1, v2.Z + 1);
                Vector3 temp3 = new Vector3(v3.X + 1, v3.Y + 1, v3.Z + 1);
                normal = Vector3.Cross(temp2 - temp, temp3 - temp);
            }
            return normal.Normalized();
        }
    }
}

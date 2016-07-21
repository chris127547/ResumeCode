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
        public int index;
        private List<Triangle> adjacentTriangles;
        public List<Triangle> AdjacentTriangles
        {
            get
            {
                if (adjacentTriangles == null)
                {
                    adjacentTriangles = GetAdjacentTriangles();                    
                }
                return adjacentTriangles;
            }
            set { adjacentTriangles = value; }
        }

        public Triangle(Vertex Vertex1, Vertex Vertex2, Vertex Vertex3, Vector3 Normal)
        {
            this.Vertex1 = Vertex1; this.Vertex2 = Vertex2; this.Vertex3 = Vertex3; this.Normal = Normal;
        }

        public Triangle(Vertex Vertex1, Vertex Vertex2, Vertex Vertex3)
        {
            this.Vertex1 = Vertex1; this.Vertex2 = Vertex2; this.Vertex3 = Vertex3;
            Normal = CalculateSurfaceNormal();
        }

        public Triangle(Vector3 position1, Vector3 position2, Vector3 position3)
        {
            Vertex1 = new Vertex();
            Vertex1.position = new Vertex.Position();
            Vertex1.position.vector = position1;
            Vertex2 = new Vertex();
            Vertex2.position = new Vertex.Position();
            Vertex2.position.vector = position2;
            Vertex3 = new Vertex();
            Vertex3.position = new Vertex.Position();
            Vertex3.position.vector = position3;
            ReCalculateNormal();
            Vertex1.normal = new Vertex.Normal();
            Vertex1.normal.vector = Normal;
            Vertex2.normal = new Vertex.Normal();
            Vertex2.normal.vector = Normal;
            Vertex3.normal = new Vertex.Normal();
            Vertex3.normal.vector = Normal;
        }

        public void ReCalculateNormal()
        {
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
                normal = Vector3.Cross(v1 - v3, v2 - v3);
            }
            return normal.Normalized();
        }

        private List<Triangle> GetAdjacentTriangles()
        {
            List<Triangle> tris = new List<Triangle>();
            for (int i = 0; i < Vertex1.adjacentTriangles.Count; i++)
            {
                if (!tris.Contains(Vertex1.adjacentTriangles[i]))
                {
                    tris.Add(Vertex1.adjacentTriangles[i]);
                }
            }
            for (int i = 0; i < Vertex2.adjacentTriangles.Count; i++)
            {
                if (!tris.Contains(Vertex2.adjacentTriangles[i]))
                {
                    tris.Add(Vertex2.adjacentTriangles[i]);
                }
            }
            for (int i = 0; i < Vertex3.adjacentTriangles.Count; i++)
            {
                if (!tris.Contains(Vertex3.adjacentTriangles[i]))
                {
                    tris.Add(Vertex3.adjacentTriangles[i]);
                }
            }
            return tris;
        }
    }
}

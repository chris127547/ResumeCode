using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class Vertex
    {

        public Position position = null;
        public Normal normal = null;
        public Color color = null;
        public Texture texture = null;
        public BoneIndex boneIndex = null;
        public BoneWeight boneWeight = null;

        public List<Triangle> adjacentTriangles = new List<Triangle>();
        private int index;

        public void AddValue(string valueType, float value)
        {
            switch (valueType)
            {
                case "x":
                    if (position == null) { position = new Position(); }
                    position.X = value;
                    break;
                case "y":
                    if (position == null) { position = new Position(); }
                    position.Y = value;
                    break;
                case "z":
                    if (position == null) { position = new Position(); }
                    position.Z = value;
                    break;

                case "nx":
                    if (normal == null) { normal = new Normal(); }
                    normal.X = value;
                    break;
                case "ny":
                    if (normal == null) { normal = new Normal(); }
                    normal.Y = value;
                    break;
                case "nz":
                    if (normal == null) { normal = new Normal(); }
                    normal.Z = value;
                    break;

                case "s":
                    if (texture == null) { texture = new Texture(); }
                    texture.S = value;
                    break;
                case "t":
                    if (texture == null) { texture = new Texture(); }
                    texture.T = value;
                    break;
            }
        }

        public void AddColor(float[] color)
        {
            if (this.color == null) { this.color = new Color(); }
            this.color.Red = color[0];
            this.color.Green = color[1];
            this.color.Blue = color[2];
            this.color.Alpha = color[3];
        }

        public void AddAdjacentTriangle(Triangle v)
        {
            adjacentTriangles.Add(v);
        }

        public void RemoveAdjacentTriangle(Triangle v)
        {
            adjacentTriangles.Remove(v);
        }

        public bool IsAdjacent(Triangle v)
        {
            return adjacentTriangles.Contains(v);
        }

        /// <summary>Gets this Vertices index within a VertexList. Value must be set by calling VertexList.UpdateIndices</summary>
        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public class Position
        {
            public Vector3 vector = new Vector3(0,0,0);
            public float X
            {
                get { return vector.X; }
                set { vector.X = value; }
            }
            public float Y
            {
                get { return vector.Y; }
                set { vector.Y = value; }
            }
            public float Z
            {
                get { return vector.Z; }
                set { vector.Z = value; }
            }
            public Position(Vector3 points)
            {
                vector = points;
            }
            public Position() { }
        }
        public class Normal
        {
            public Vector3 vector = new Vector3(0, 0, 0);
            public float X
            {
                get { return vector.X; }
                set { vector.X = value; }
            }
            public float Y
            {
                get { return vector.Y; }
                set { vector.Y = value; }
            }
            public float Z
            {
                get { return vector.Z; }
                set { vector.Z = value; }
            }
            public Normal(Vector3 direction)
            {
                vector = direction;
            }
            public Normal() { }
        }
        public class Color
        {
            public float Red, Green, Blue, Alpha;
            public Color(float red, float green, float blue, float alpha)
            {
                Red = red; Green = green; Blue = blue; Alpha = alpha;
            }
            public Color() { }
        }
        public class Texture
        {
            public float S, T;
            public Texture(float s, float t)
            {
                S = s; T = t;
            }
            public Texture() { }
        }
        public class BoneIndex
        {
            public int A, B;
            public BoneIndex(int a, int b)
            {
                A = a; B = b;
            }
            public BoneIndex() { }
        }
        public class BoneWeight
        {
            public float A, B;
            public BoneWeight(float a, float b)
            {
                A = a; B = b;
            }
            public BoneWeight() { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine.RenderedObjects.FileToObjectConverters
{
    public class VertexList : List<Vertex>
    {
        public float[] GetVertexPositions()
        {
            float[] positions = new float[this.Count * 3];
            for (int i = 0; i < this.Count; i++)
            {
                int arraySpot = i * 3;
                positions[arraySpot] = this[i].position.X;
                positions[arraySpot + 1] = this[i].position.Y;
                positions[arraySpot + 2] = this[i].position.Z;
            }
            return positions;
        }

        public float[] GetVertexNormals()
        {
            float[] normals = new float[this.Count * 3];
            for (int i = 0; i < this.Count; i++)
            {
                int arraySpot = i * 3;
                normals[arraySpot] = this[i].normal.X;
                normals[arraySpot + 1] = this[i].normal.Y;
                normals[arraySpot + 2] = this[i].normal.Z;
            }
            return normals;
        }

        public float[] GetVertexColors()
        {
            float[] colors = new float[this.Count * 4];
            for (int i = 0; i < this.Count; i++)
            {
                int arraySpot = i * 4;
                colors[arraySpot] = this[i].color.Red;
                colors[arraySpot + 1] = this[i].color.Green;
                colors[arraySpot + 2] = this[i].color.Blue;
                colors[arraySpot + 3] = this[i].color.Alpha;
            }
            return colors;
        }

        public float[] GetVertexTextures()
        {
            float[] textures = new float[this.Count * 2];
            for (int i = 0; i < this.Count; i++)
            {
                int arraySpot = i * 2;
                textures[arraySpot] = this[i].texture.S;
                textures[arraySpot + 1] = this[i].texture.T;
            }
            return textures;
        }

        public void UpdateIndices()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].SetIndex(i);
            }
        }

        /// <summary>
        /// For compatability with old code. Creates array with all available data order from position, normal, texture, color
        /// if no data of a type was added then that data will not be included.
        /// </summary>
        public float[] GetAvailableShapeData()
        {
            bool hasPositions = this[0].position != null, hasNormals = this[0].normal != null, hasTextures = this[0].texture != null, hasColors = this[0].color != null;
            int stride = 0;
            if (hasPositions) { stride += 3; }
            if (hasNormals) { stride += 3; }
            if (hasTextures) { stride += 2; }
            if (hasColors) { stride += 4; }

            float[] shapeData = new float[this.Count * stride];
            for (int i = 0; i < this.Count; i++)
            {
                int arraySpot = i * stride;
                if(hasPositions)
                {
                    shapeData[arraySpot++] = this[i].position.X;
                    shapeData[arraySpot++] = this[i].position.Y;
                    shapeData[arraySpot++] = this[i].position.Z;
                }
                if (hasNormals)
                {
                    shapeData[arraySpot++] = this[i].normal.X;
                    shapeData[arraySpot++] = this[i].normal.Y;
                    shapeData[arraySpot++] = this[i].normal.Z;
                }
                if (hasTextures)
                {
                    shapeData[arraySpot++] = this[i].texture.S;
                    shapeData[arraySpot++] = this[i].texture.T;
                }
                if (hasColors)
                {
                    shapeData[arraySpot++] = this[i].color.Red;
                    shapeData[arraySpot++] = this[i].color.Green;
                    shapeData[arraySpot++] = this[i].color.Blue;
                    shapeData[arraySpot++] = this[i].color.Alpha;
                }
            }
            return shapeData;
        }
    }
}

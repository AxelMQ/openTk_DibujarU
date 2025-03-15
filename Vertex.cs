using OpenTK.Mathematics;

namespace OpenTK_DibujarU
{
    public class Vertex
    {
        public Vector3 Position { get; set; } // Coordenadas (x, y, z)
        public Vector4 Color { get; set; } // RGBA (Rojo, Verde, Azul, Alpha)

        public Vertex(float x, float y, float z, Vector4 color)
        {
            Position = new Vector3(x, y, z);
            Color = color;
        }
    }
}
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Poligono
    {
        public List<Vector3> Vertices { get;  set; }
        public List<int> Indices { get; set; }
        public Vector4 Color { get; private set; }
        public Vector3 CentroRelativo { get; private set; }

        public Poligono(List<Vector3> vertices, List<int> indices, Vector4 color)
        {
            Vertices = vertices;
            Indices = indices;
            Color = color;
            CalcularCentroDeMasa();
        }

        public void CalcularCentroDeMasa()
        {
            if (Vertices.Count == 0) return;

            Vector3 suma = Vector3.Zero;
            foreach (var vertice in Vertices)
                suma += vertice;

            CentroRelativo = suma / Vertices.Count;

            // Convertir vértices a coordenadas relativas al centro de masa
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i] -= CentroRelativo;
        }


        public void Rotar(float angulo, Vector3 eje, Vector3 centroRotacion)
        {
            Quaternion rotacion = Quaternion.FromAxisAngle(eje.Normalized(),
                                MathHelper.DegreesToRadians(angulo));

            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector3 verticeRelativo = Vertices[i] - centroRotacion;
                Vertices[i] = centroRotacion + Vector3.Transform(verticeRelativo, rotacion);
            }
        }

        public void Escalar(Vector3 escala, Vector3 centroEscalado)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector3 verticeRelativo = Vertices[i] - centroEscalado;
                Vertices[i] = centroEscalado + Vector3.Multiply(verticeRelativo, escala);
            }
        }

        public float[] GetVerticesAsFloatArray(Vector3? offset = null)
        {
            Vector3 actualOffset = offset ?? Vector3.Zero;
            List<float> result = new List<float>();
            foreach (var vertex in Vertices)
            {
                Vector3 verticeGlobal = vertex + actualOffset;
                result.Add(verticeGlobal.X);
                result.Add(verticeGlobal.Y);
                result.Add(verticeGlobal.Z);
                result.Add(Color.X);
                result.Add(Color.Y);
                result.Add(Color.Z);
                result.Add(Color.W);
            }
            return result.ToArray();
        }

        public void Dibujar(Renderer renderer, Vector3 posicionGlobalParte)
        {
            // Posición final = posición de la parte + centro del polígono
            renderer.RenderPoligono(this, posicionGlobalParte + CentroRelativo);
        }


    }
}

using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK_DibujarU;

namespace OpenTK_DibujarU
{
    public class Shape
    {
        public List<Vertex> Vertices { get; set; } // Lista de vértices
        public List<int> Indices { get; set; }    // Lista de índices para dibujar triángulos
        public Vector3 Position { get; set; }     // Posición de la figura (3D)

        private Vector3 _velocity; // Campo privado
        public Vector3 Velocity // Velocidad de la figura (3D)
        {
            get => _velocity;
            set => _velocity = value;
        }     

        public Shape()
        {
            Vertices = new List<Vertex>();
            Indices = new List<int>();
            Position = Vector3.Zero; // Inicializar en el origen
            Velocity = new Vector3(0.1f, 0.1f, 0.1f); // Velocidad inicial
        }

        // Método para generar una 'U' en 3D con rectángulos
        public void GenerateU(float baseWidth, float verticalHeight, float depth)
        {
            Vertices.Clear();
            Indices.Clear();

            float baseHeight = baseWidth / 4; // Altura de la base (horizontal)
            float verticalWidth = baseWidth / 4; // Ancho de los lados verticales

            // Posiciones ajustadas para que los cubos se conecten perfectamente
            // ----------------------------------------------------------------
            // Base horizontal: Centrada en Y=0, con su parte superior en Y=0
            Vector3 basePosition = new Vector3(0.0f, -baseHeight / 2, 0.0f);

            // Lados verticales:
            // - Centrados en X para que su borde interno coincida con la base
            // - Comienzan desde Y=0 (parte superior de la base)
            Vector3 leftPosition = new Vector3(
                x: -baseWidth / 2 + verticalWidth / 2, // Alinea el borde interno con la base
                y: verticalHeight / 2, // Centro del cubo vertical (su altura parte desde Y=0)
                z: 0.0f
            );

            Vector3 rightPosition = new Vector3(
                x: baseWidth / 2 - verticalWidth / 2, // Alinea el borde interno con la base
                y: verticalHeight / 2,
                z: 0.0f
            );

            // Generar los cubos
            GenerateCubeGeometry(basePosition, baseWidth, baseHeight, depth); // Base
            GenerateCubeGeometry(leftPosition, verticalWidth, verticalHeight, depth); // Lado izquierdo
            GenerateCubeGeometry(rightPosition, verticalWidth, verticalHeight, depth); // Lado derecho
        }

        // Método auxiliar para generar un cubo en una posición específica
        private void GenerateCubeGeometry(Vector3 position, float width, float height, float depth)
        {
            float halfW = width / 2;
            float halfH = height / 2;
            float halfD = depth / 2;

            // Vértices del cubo (8 vértices)
            List<Vertex> cubeVertices = new List<Vertex>
            {
                // Frontal
                new Vertex(position.X - halfW, position.Y - halfH, position.Z + halfD, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)), // 0
                new Vertex(position.X + halfW, position.Y - halfH, position.Z + halfD, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)), // 1
                new Vertex(position.X + halfW, position.Y + halfH, position.Z + halfD, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)), // 2
                new Vertex(position.X - halfW, position.Y + halfH, position.Z + halfD, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)), // 3
                // Trasera
                new Vertex(position.X - halfW, position.Y - halfH, position.Z - halfD, new Vector4(0.0f, 0.0f, 1.0f, 1.0f)), // 4
                new Vertex(position.X + halfW, position.Y - halfH, position.Z - halfD, new Vector4(0.0f, 0.0f, 1.0f, 1.0f)), // 5
                new Vertex(position.X + halfW, position.Y + halfH, position.Z - halfD, new Vector4(0.0f, 0.0f, 1.0f, 1.0f)), // 6
                new Vertex(position.X - halfW, position.Y + halfH, position.Z - halfD, new Vector4(0.0f, 0.0f, 1.0f, 1.0f)), // 7
            };

            // Índices del cubo (12 triángulos)
            List<int> cubeIndices = new List<int>
            {
                0, 1, 2, 0, 2, 3,   // Frontal
                4, 5, 6, 4, 6, 7,   // Trasera
                0, 3, 7, 0, 7, 4,   // Izquierda
                1, 5, 6, 1, 6, 2,   // Derecha
                3, 2, 6, 3, 6, 7,   // Superior
                0, 4, 5, 0, 5, 1    // Inferior
            };

            // Añadir vértices e índices al modelo principal
            int vertexOffset = Vertices.Count;
            Vertices.AddRange(cubeVertices);

            foreach (int index in cubeIndices)
            {
                Indices.Add(index + vertexOffset);
            }
        }

        // Método para obtener los vértices como un array de floats
        public float[] GetVerticesAsFloatArray()
        {
            List<float> result = new List<float>();
            foreach (var vertex in Vertices)
            {
                // Posición (x, y, z)
                result.Add(vertex.Position.X);
                result.Add(vertex.Position.Y);
                result.Add(vertex.Position.Z);

                // Color (r, g, b, a)
                result.Add(vertex.Color.X);
                result.Add(vertex.Color.Y);
                result.Add(vertex.Color.Z);
                result.Add(vertex.Color.W);
            }
            return result.ToArray();
        }

    }
}
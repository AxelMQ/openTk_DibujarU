using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Poligono : IDisposable
    {
        public List<Vector3> VerticesRelativos { get; set; } = new();
        public Vector3 CentroRelativo { get;  set; }
        public Vector4 Color { get; set; }

        private int _vao;
        private int _vbo;
        private bool _initialized = false;

        public Poligono() { }


        public Poligono(List<Vector3> vertices, Vector4 color)
        {
            Console.WriteLine($"Creando polígono con {vertices.Count} vértices");
            VerticesRelativos = vertices;
            Color = color;
            CalcularCentroDeMasa();
            InitializeGL();
        }

        public void CalcularCentroDeMasa()
        {
            if (VerticesRelativos.Count == 0) return;

            CentroRelativo = Vector3.Zero;
            foreach (var v in VerticesRelativos)
                CentroRelativo += v;
            CentroRelativo /= VerticesRelativos.Count;

            // Normalizar los vertices
            for (int i = 0; i < VerticesRelativos.Count; i++)
                VerticesRelativos[i] -= CentroRelativo;
        }

        private void InitializeGL()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            // Configuración del layout del vértice
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _initialized = true;
            UpdateBufferData();
        }

        private void UpdateBufferData()
        {
            float[] vertexData = new float[VerticesRelativos.Count * 6];
            for (int i = 0; i < VerticesRelativos.Count; i++)
            {
                vertexData[i * 6] = VerticesRelativos[i].X;
                vertexData[i * 6 + 1] = VerticesRelativos[i].Y;
                vertexData[i * 6 + 2] = VerticesRelativos[i].Z;
                vertexData[i * 6 + 3] = Color.X;
                vertexData[i * 6 + 4] = Color.Y;
                vertexData[i * 6 + 5] = Color.Z;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.DynamicDraw);
        }

        public void Dibujar(Matrix4 modelMatrix)
        {
            Shaders.DefaultShader.SetMatrix4("model", modelMatrix); 
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, VerticesRelativos.Count);
        }

        public void Dispose()
        {
            if (_initialized)
            {
                GL.DeleteBuffer(_vbo);
                GL.DeleteVertexArray(_vao);
            }
        }
        public void Inicializar()
        {
            InitializeGL();
        }
        public void Trasladar(Vector3 desplazamiento)
        {
            for (int i = 0; i < VerticesRelativos.Count; i++)
            {
                var v = VerticesRelativos[i]; 
                v += desplazamiento;           
                VerticesRelativos[i] = v;      
            }

            CentroRelativo += desplazamiento;

            UpdateBufferData();
        }
        public void Rotar(Vector3 anguloEnRadianes, Vector3 centro)
        {
            Matrix4 transform = Matrix4.Identity;

            // Trasladar al origen
            transform *= Matrix4.CreateTranslation(-centro);

            // Rotaciones individuales
            if (anguloEnRadianes.X != 0)
                transform *= Matrix4.CreateRotationX(anguloEnRadianes.X);
            if (anguloEnRadianes.Y != 0)
                transform *= Matrix4.CreateRotationY(anguloEnRadianes.Y);
            if (anguloEnRadianes.Z != 0)
                transform *= Matrix4.CreateRotationZ(anguloEnRadianes.Z);

            // Volver a trasladar al centro
            transform *= Matrix4.CreateTranslation(centro);

            // Aplicar transformación a cada vértice
            for (int i = 0; i < VerticesRelativos.Count; i++)
            {
                Vector4 v = new Vector4(VerticesRelativos[i], 1.0f);
                v = transform * v;
                VerticesRelativos[i] = new Vector3(v.X, v.Y, v.Z);
            }

            CalcularCentroDeMasa();
            UpdateBufferData();
        }

        public void Escalar(Vector3 factor, Vector3 centro)
        {
            for (int i = 0; i < VerticesRelativos.Count; i++)
            {
                var v = VerticesRelativos[i];
                Vector3 desplazado = v - centro;
                Vector3 escalado = new Vector3(
                    desplazado.X * factor.X,
                    desplazado.Y * factor.Y,
                    desplazado.Z * factor.Z
                );
                VerticesRelativos[i] = centro + escalado;
            }

            CalcularCentroDeMasa();
            UpdateBufferData();
        }
        public void Escalar(float factor, Vector3 centro)
        {
            Escalar(new Vector3(factor, factor, factor), centro);
        }

        public void Escalar(Vector3 factor)
        {
            Escalar(factor, CentroRelativo);
        }

    }
}
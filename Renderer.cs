using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace OpenTK_DibujarU
{
    public class Renderer : IDisposable
    {
        private int _shaderProgram;
        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        public Renderer()
        {
        }

        public void Initialize()
        {
            _shaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, Shaders.VertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, Shaders.FragmentShaderSource);
            GL.CompileShader(fragmentShader);

            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Desactivar Backface Culling para pruebas
            GL.Disable(EnableCap.CullFace);

            // Activar el Depth Test
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            // Configuración de la cámara
            _viewMatrix = Matrix4.LookAt(
                new Vector3(3, 3, 3), // Posición cámara
                Vector3.Zero,          // Punto de mira
                Vector3.UnitY           // Vector arriba
            );
        }

        public void Render(Escenario escenario)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_shaderProgram);

            escenario.CalcularCentroDeMasa();
            escenario.Dibujar(this);
        }

        public void RenderParte(Parte parte, Vector3 posicionObjeto, Vector3 centroRelativoObjeto)
        {
            foreach (var poligono in parte.Poligonos)
            {
                poligono.CalcularCentroDeMasa();
                RenderPoligono(poligono, posicionObjeto + parte.PosicionRelativa - centroRelativoObjeto);
            }
        }

        public void RenderPoligono(Poligono poligono, Vector3 posicionGlobal)
        {
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            var vertices = poligono.GetVerticesAsFloatArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, poligono.Indices.Count * sizeof(int), poligono.Indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            Matrix4 modelMatrix = Matrix4.CreateTranslation(posicionGlobal);

            int projectionLoc = GL.GetUniformLocation(_shaderProgram, "uProjection");
            int viewLoc = GL.GetUniformLocation(_shaderProgram, "uView");
            int modelLoc = GL.GetUniformLocation(_shaderProgram, "uModel");

            GL.UniformMatrix4(projectionLoc, false, ref _projectionMatrix);
            GL.UniformMatrix4(viewLoc, false, ref _viewMatrix);
            GL.UniformMatrix4(modelLoc, false, ref modelMatrix);

            GL.DrawElements(PrimitiveType.Triangles, poligono.Indices.Count, DrawElementsType.UnsignedInt, 0);

            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
        }

        public void OnResize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            float aspectRatio = width / (float)height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                aspectRatio,
                0.1f,
                100f
            );
        }

        public void Dispose()
        {
            GL.DeleteProgram(_shaderProgram);
        }
    }
}

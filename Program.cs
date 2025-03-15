using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenTK_DibujarU
{
    class Program : GameWindow
    {
        private int _vertexBufferObject; // Buffer para almacenar los vértices
        private int _vertexArrayObject;  // Objeto de vértice (VAO)
        private int _elementBufferObject; // Buffer de índices (EBO)
        private int _shaderProgram;      // Programa de shaders
        private float _rotationAngle = 0.0f; // Ángulo acumulado de rotación
        private Shape _triangle;         // Figura (triángulo)
        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        public Program()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Title = "OpenTK Test - Figura 3D";
            Size = new Vector2i(800, 600);
            VSync = VSyncMode.On;
            _triangle = new Shape();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f); // Color de fondo
            GL.Enable(EnableCap.DepthTest);

            // Configurar la matriz de proyección (perspectiva)
            float aspectRatio = Size.X / (float)Size.Y;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver3, // 60 grados (más amplio que 45°)
                aspectRatio,        // Relación de aspecto
                0.001f,               // Plano cercano
                100.0f              // Plano lejano
            );

            // Generar la "U" con parámetros específicos
            float baseWidth = 3.0f;
            float verticalHeight = 4.0f;
            _triangle.GenerateU(baseWidth, verticalHeight, 0.5f);

            // Configurar la matriz de vista (cámara)
            _viewMatrix = Matrix4.LookAt(
                new Vector3(0, 2, 5), // Posición de la cámara (X, Y, Z)
                new Vector3(0, 1, 0), // Enfocar al centro de la "U"
                Vector3.UnitY         // Vector "arriba"
            );


            //  Configurar VAO primero
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Configurar VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                _triangle.GetVerticesAsFloatArray().Length * sizeof(float),
                _triangle.GetVerticesAsFloatArray(),
                BufferUsageHint.StaticDraw
            );

            // Configurar atributos del vértice (posición y color)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            //  Configurar EBO (con el VAO vinculado)
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                _triangle.Indices.Count * sizeof(int),
                _triangle.Indices.ToArray(),
                BufferUsageHint.StaticDraw
            );

            // Compilar shaders
            CompileShaders();
        }

        private void CompileShaders()
        {
            // Crear el programa de shaders
            _shaderProgram = GL.CreateProgram();

            // Compilar el vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, Shaders.VertexShaderSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexShaderStatus);
            if (vertexShaderStatus == (int)All.False)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine("Error al compilar el vertex shader: " + infoLog);
            }

            // Compilar el fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, Shaders.FragmentShaderSource);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentShaderStatus);
            if (fragmentShaderStatus == (int)All.False)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine("Error al compilar el fragment shader: " + infoLog);
            }

            // Enlazar los shaders al programa
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            // Verificar si el programa se enlazó correctamente
            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out int programStatus);
            if (programStatus == (int)All.False)
            {
                string infoLog = GL.GetProgramInfoLog(_shaderProgram);
                Console.WriteLine("Error al enlazar el programa de shaders: " + infoLog);
            }

            // Limpiar los shaders (ya no son necesarios)
            GL.DetachShader(_shaderProgram, vertexShader);
            GL.DetachShader(_shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            // Actualizar el ángulo de rotación
            _rotationAngle += (float)args.Time * 50.0f;

            // Crear matriz de rotación (sin traslación)
            Matrix4 rotationMatrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotationAngle));

            // Matriz de modelo: solo rotación (sin movimiento)
            Matrix4 modelMatrix = rotationMatrix;

            // Pasar matrices al shader
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "uProjection"), false, ref _projectionMatrix);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "uView"), false, ref _viewMatrix);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "uModel"), false, ref modelMatrix);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _triangle.Indices.Count, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Cerrar la ventana si se presiona la tecla ESC
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            //// Mover la figura automáticamente
            //_triangle.Position += _triangle.Velocity * (float)args.Time;

            //// Ajustar rebote
            //Vector3 currentVelocity = _triangle.Velocity;
            //if (MathF.Abs(_triangle.Position.X) > 1.0f) currentVelocity.X *= -1;
            //if (MathF.Abs(_triangle.Position.Y) > 1.0f) currentVelocity.Y *= -1;
            //_triangle.Velocity = currentVelocity;

            //// Opcional: Rotación basada en la velocidad
            //_rotationAngle += _triangle.Velocity.Length * (float)args.Time * 10.0f;
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                program.Run();
            }
        }
    }
}
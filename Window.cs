using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_DibujarU
{
    public class Window : GameWindow
    {
        private Escenario _escenario;
        private Renderer _renderer;
        private float _rotationAngle = 0f;
        private Vector3 _position = Vector3.Zero;
        private float _scale = 1f;
        private float _moveSpeed = 2f;
        private float _rotationSpeed = 2f;

        public Window(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(width, height),
                Title = title
            })
        {
            _escenario = new Escenario();
            _renderer = new Renderer();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            // Crear la "U" usando UFactory y agregarla al escenario
            var uObjeto = UFactory.CrearU("MiU", 2.0f, 3.0f, 1.0f, new Vector4(1f, 0f, 0f, 1f));
            //uObjeto.PosicionRelativa = new Vector3(0f, 2f, 0f);
            _escenario.AgregarObjeto(uObjeto);

            // Segundo objeto
            //var cubo = CuboFactory.CrearCubo("MiCubo", 1f, 1f, 1f);
            //cubo.PosicionRelativa = new Vector3(-3f, 0f, 0f);
            //_escenario.AgregarObjeto(new Objeto("CuboAzul") { Partes = new List<Parte> { cubo } });
            var uObjeto2 = UFactory.CrearU("MiU", 2.0f, 3.0f, 1.0f, new Vector4(1f, 0f, 0f, 1f));
            uObjeto2.PosicionRelativa = new Vector3(5f, 0f, 0f);
            _escenario.AgregarObjeto(uObjeto2);

            // Inicializar el renderer
            _renderer.Initialize();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Dibujar el escenario
            _renderer.Render(_escenario);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            _renderer.OnResize(Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var keyboard = KeyboardState;
            var deltaTime = (float)args.Time;

            // Rotación con teclas Q/E
            if (keyboard.IsKeyDown(Keys.Q))
                _rotationAngle -= _rotationSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.E))
                _rotationAngle += _rotationSpeed * deltaTime;

            // Movimiento con teclas WASD
            float moveSpeed = _moveSpeed * deltaTime;
            if (keyboard.IsKeyDown(Keys.W))
                _position.Z -= moveSpeed;
            if (keyboard.IsKeyDown(Keys.S))
                _position.Z += moveSpeed;
            if (keyboard.IsKeyDown(Keys.A))
                _position.X -= moveSpeed;
            if (keyboard.IsKeyDown(Keys.D))
                _position.X += moveSpeed;

            // Prueba de movimiento relativo
            if (keyboard.IsKeyDown(Keys.T)) // Tecla T para test
            {
                Console.WriteLine("=== Jerarquía de posiciones ===");
                foreach (var objeto in _escenario.Objetos)
                {
                    Console.WriteLine($"Objeto '{objeto.Nombre}':");
                    Console.WriteLine($"- Posición global: {objeto.Posicion}");
                    Console.WriteLine($"- Centro relativo: {objeto.CentroRelativo}");

                    foreach (var parte in objeto.Partes)
                    {
                        Console.WriteLine($"  Parte en {parte.PosicionRelativa}");
                        Console.WriteLine($"  Centro parte: {parte.CentroRelativo}");
                    }
                }
            }

            foreach (var objeto in _escenario.Objetos)
            {
                objeto.Rotar(_rotationAngle, Vector3.UnitY);
                objeto.Mover(_position);
            }

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _renderer.Dispose();
        }
    }
}

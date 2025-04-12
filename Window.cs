using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Text.Json;
using System.IO;
using System.Drawing;

namespace OpenTK_DibujarU
{
    public class Window : GameWindow
    {
        private Escenario _escenario;
        private Matrix4 _projection;
        private Matrix4 _view;

        public Window(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(width, height),
                Title = title
            })
        {
            _escenario = new Escenario();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Configuración básica de OpenGL
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            // Inicializar shader
            Shaders.DefaultShader = new Shader(Shaders.VertexShaderSource, Shaders.FragmentShaderSource);

            // Configurar cámara
            _view = Matrix4.LookAt(
                new Vector3(3, 2, 5), 
                Vector3.Zero,
                Vector3.UnitY
            );

            var uObjeto = ObjetoU.CrearU(new Vector4(1f, 0f, 0f, 1f));
            uObjeto.Posicion = new Vector3(0f, 0f, 0f);
            _escenario.Objetos.Add(uObjeto);

            var uObjeto2 = ObjetoU.CrearU(new Vector4(1f, 0f, 1f, 1f));
            uObjeto2.Posicion = new Vector3(2f, 0f, 0f);
            _escenario.Objetos.Add(uObjeto2);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Shaders.DefaultShader.Use();
            Shaders.DefaultShader.SetMatrix4("projection", _projection);
            Shaders.DefaultShader.SetMatrix4("view", _view);
            Shaders.DefaultShader.SetVector4("overrideColor", Vector4.One);

            _escenario.Dibujar();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                e.Width / (float)e.Height,
                0.1f,
                100f
            );
        }

        protected override void OnUnload()
        {
            Shaders.DefaultShader?.Dispose();
            base.OnUnload();
        }
    }
}
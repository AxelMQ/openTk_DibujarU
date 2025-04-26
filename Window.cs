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
        private enum NivelTransformacion
        {
            Escenario,
            Objeto,
            Parte,
            Poligono
        }

        private NivelTransformacion _nivelActual = NivelTransformacion.Objeto;

        private int objetoSeleccionado = 0;
        private int parteSeleccionada = 0;
        private int poligonoSeleccionado = 0;



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

            //var uObjeto = ObjetoU.CrearU(new Vector4(1f, 0f, 0f, 1f));
            //uObjeto.Posicion = new Vector3(0f, 0f, 0f);
            //_escenario.Objetos.Add(uObjeto);

            //var uObjeto2 = ObjetoU.CrearU(new Vector4(1f, 0f, 1f, 1f));
            //uObjeto2.Posicion = new Vector3(2f, 0f, 0f);
            //_escenario.Objetos.Add(uObjeto2);
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

            float velocidad = 2.0f; // Unidades por segundo
            Vector3 movimiento = Vector3.Zero;

            // Cambiar nivel con teclas numéricas
            if (KeyboardState.IsKeyPressed(Keys.D1))
            {
                _nivelActual = NivelTransformacion.Escenario;
                Console.WriteLine("Nivel actual: Escenario");
            }
            else if (KeyboardState.IsKeyPressed(Keys.D2))
            {
                _nivelActual = NivelTransformacion.Objeto;
                Console.WriteLine("Nivel actual: Objeto");
            }
            else if (KeyboardState.IsKeyPressed(Keys.D3))
            {
                _nivelActual = NivelTransformacion.Parte;
                Console.WriteLine("Nivel actual: Parte");
            }
            else if (KeyboardState.IsKeyPressed(Keys.D4))
            {
                _nivelActual = NivelTransformacion.Poligono;
                Console.WriteLine("Nivel actual: Polígono");
            }


            // Cambiar entre objetos
            if (KeyboardState.IsKeyPressed(Keys.O) && _escenario.Objetos.Count > 0)
            {
                if (_escenario.Objetos.Count > 0)
                {
                    objetoSeleccionado = (objetoSeleccionado + 1) % _escenario.Objetos.Count;
                    Console.WriteLine($"Objeto seleccionado: {objetoSeleccionado}");
                }
            }

            // Cambiar entre las partes del objeto
            if (KeyboardState.IsKeyPressed(Keys.P) && _escenario.Objetos.Count > 0)
            {
                var objeto = _escenario.Objetos[objetoSeleccionado];
                if (objeto.Partes.Count > 0)
                {
                    parteSeleccionada = (parteSeleccionada + 1) % objeto.Partes.Count;
                    Console.WriteLine($"Parte seleccionada: {parteSeleccionada}");
                }
            }



            if (KeyboardState.IsKeyDown(Keys.W))
                movimiento += new Vector3(0f, 0f, -1f); // Adelante
            if (KeyboardState.IsKeyDown(Keys.S))
                movimiento += new Vector3(0f, 0f, 1f);  // Atrás
            if (KeyboardState.IsKeyDown(Keys.A))
                movimiento += new Vector3(-1f, 0f, 0f); // Izquierda
            if (KeyboardState.IsKeyDown(Keys.D))
                movimiento += new Vector3(1f, 0f, 0f);  // Derecha
            if (KeyboardState.IsKeyDown(Keys.Q))
                movimiento += new Vector3(0f, -1f, 0f); // Abajo
            if (KeyboardState.IsKeyDown(Keys.E))
                movimiento += new Vector3(0f, 1f, 0f);  // Arriba

            if (movimiento != Vector3.Zero)
            {
                movimiento = movimiento.Normalized() * velocidad * (float)e.Time;

                switch (_nivelActual)
                {
                    case NivelTransformacion.Escenario:
                        _escenario.Trasladar(movimiento);
                        break;

                    case NivelTransformacion.Objeto:
                        if (_escenario.Objetos.Count > 0)
                            _escenario.Objetos[objetoSeleccionado].Trasladar(movimiento);
                        break;

                    case NivelTransformacion.Parte:
                        if (_escenario.Objetos.Count > 0)
                        {
                            var obj = _escenario.Objetos[objetoSeleccionado];
                            if (obj.Partes.Count > parteSeleccionada)
                                obj.Partes[parteSeleccionada].Trasladar(movimiento);

                        }
                        break;

                    case NivelTransformacion.Poligono:
                        if (_escenario.Objetos.Count > 0)
                        {
                            var obj = _escenario.Objetos[objetoSeleccionado];
                            if (obj.Partes.Count > parteSeleccionada)
                            {
                                var parte = obj.Partes[parteSeleccionada];
                                if (parte.Poligonos.Count > poligonoSeleccionado)
                                    parte.Poligonos[poligonoSeleccionado].Trasladar(movimiento);
                            }
                        }
                        break;
                }
            }



            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyPressed(Keys.G))
            {
                Serializador.GuardarObjeto("objetoU.json", _escenario.Objetos.First());
                Console.WriteLine("Objeto guardado en objetoU.json");
            }

            if (KeyboardState.IsKeyPressed(Keys.L))
            {
                var cargado = Serializador.CargarObjeto("objetoU.json");
                cargado.Posicion = new Vector3(-2f, 0f, 0f);
                cargado.Inicializar();

                _escenario.Objetos.Add(cargado);
                Console.WriteLine("Objeto cargado desde objetoU.json");
            }

            if (KeyboardState.IsKeyPressed(Keys.H))
            {
                Serializador.GuardarEscenario("escenario.json", _escenario);
                Console.WriteLine("Escenario guardado en escenario.json");
            }

            if (KeyboardState.IsKeyPressed(Keys.K))
            {
                _escenario = Serializador.CargarEscenario("escenario.json");
                _escenario.Inicializar();
                Console.WriteLine("Escenario cargado desde escenario.json");
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
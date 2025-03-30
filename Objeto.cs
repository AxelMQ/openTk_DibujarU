using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Objeto
    {
        public List<Parte> Partes { get;  set; }
        public string Nombre { get; private set; }
        public Vector3 Posicion { get; private set; }
        public Vector3 PosicionRelativa { get; set; }
        public Vector3 CentroRelativo { get; private set; }
        private float _anguloRotacion = 0f;
        private Vector3 _ejeRotacion = Vector3.UnitY;

        public Objeto(string nombre, Vector3 posicion = new Vector3())
        {
            Nombre = nombre;
            Posicion = posicion;
            PosicionRelativa = Vector3.Zero;
            Partes = new List<Parte>();
            CentroRelativo = Vector3.Zero;
        }

        // Método para mover el objeto ajustando las posiciones de sus partes
        public void Mover(Vector3 desplazamiento)
        {
            Posicion += desplazamiento;  // Mover el objeto sumando el desplazamiento

            foreach (var parte in Partes)
            {
                parte.PosicionRelativa += desplazamiento;
            }
        }

        // Método para agregar partes
        public void AgregarParte(Parte parte)
        {
            Partes.Add(parte);
        }

        public void CalcularCentroDeMasa()
        {
            if (Partes.Count == 0) return;

            Vector3 suma = Vector3.Zero;
            foreach (var parte in Partes)
            {
                parte.CalcularCentroDeMasa();
                suma += parte.CentroRelativo + parte.PosicionRelativa;
            }

            CentroRelativo = suma / Partes.Count;

            // Ajustar posición global
            for (int i = 0; i < Partes.Count; i++)
            {
                Partes[i].PosicionRelativa -= CentroRelativo;
            }

            PosicionRelativa -= CentroRelativo;
        }

        public void Rotar(float angulo, Vector3 eje)
        {
            _anguloRotacion = angulo;
            _ejeRotacion = eje;

            // Aplicar rotación a todas las partes
            foreach (var parte in Partes)
            {
                // Calcular posición relativa al centro del objeto
                Vector3 posRelativa = parte.PosicionRelativa - CentroRelativo;

                // Rotar la posición relativa
                Quaternion rotacion = Quaternion.FromAxisAngle(_ejeRotacion,
                                      MathHelper.DegreesToRadians(_anguloRotacion));
                posRelativa = Vector3.Transform(posRelativa, rotacion);

                // Actualizar posición
                parte.PosicionRelativa = CentroRelativo + posRelativa;

                foreach (var poligono in parte.Poligonos)
                {
                    poligono.Rotar(angulo, eje, Vector3.Zero); // Rotar sobre su propio centro
                }
            }
        }

        public void Dibujar(Renderer renderer, Vector3 posicionGlobal)
        {
            Vector3 posicionGlobalObjeto = posicionGlobal + PosicionRelativa;

            foreach (var parte in Partes)
            {
                parte.Dibujar(renderer, posicionGlobalObjeto);
            }
        }

    }

}

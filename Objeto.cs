using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Objeto
    {
        public List<Parte> Partes { get; set; } = new List<Parte>();
        public Vector3 CentroRelativo { get; set; }
        public Vector3 Posicion { get; set; } = Vector3.Zero;

        public Objeto()
        {
            Partes = new List<Parte>();
            CentroRelativo = Vector3.Zero;
        }
        
        public void CalcularCentroDeMasa()
        {
            Vector3 suma = Vector3.Zero;
            foreach (var parte in Partes)
            {
                parte.CalcularCentroDeMasa();
                suma += parte.CentroRelativo;
            }
            CentroRelativo = Partes.Count > 0 ? suma / Partes.Count : Vector3.Zero;
        }


        public void Dibujar(Vector3 offsetEscenario)
        {
            Vector3 offsetGlobal = offsetEscenario + Posicion;
            foreach (var parte in Partes)
            {
                parte.Dibujar(offsetGlobal);
            }
        }
        public void Inicializar()
        {
            foreach (var parte in Partes)
            {
                parte.Inicializar();
            }
        }
        public void Trasladar(Vector3 desplazamiento)
        {
            foreach (var parte in Partes)
            {
                parte.Trasladar(desplazamiento);
            }

            CentroRelativo += desplazamiento;
            Posicion += desplazamiento;
        }
        public void Rotar(Vector3 anguloEnRadianes)
        {
            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateTranslation(-CentroRelativo);
            if (anguloEnRadianes.X != 0)
                transform *= Matrix4.CreateRotationX(anguloEnRadianes.X);
            if (anguloEnRadianes.Y != 0)
                transform *= Matrix4.CreateRotationY(anguloEnRadianes.Y);
            if (anguloEnRadianes.Z != 0)
                transform *= Matrix4.CreateRotationZ(anguloEnRadianes.Z);
            transform *= Matrix4.CreateTranslation(CentroRelativo);

            foreach (var parte in Partes)
            {
                Vector4 pos = new Vector4(parte.PosicionRelativa, 1.0f);
                pos = transform * pos;
                parte.PosicionRelativa = new Vector3(pos.X, pos.Y, pos.Z);

                parte.Rotar(anguloEnRadianes); 
            }
        }
        public void Escalar(Vector3 factor)
        {
            foreach (var parte in Partes)
            {
                Vector3 desplazado = parte.PosicionRelativa - CentroRelativo;
                Vector3 escalado = desplazado * factor;
                parte.PosicionRelativa = CentroRelativo + escalado;

                parte.Escalar(factor);
            }

            CalcularCentroDeMasa();
        }

        public void Escalar(float factor)
        {
            Escalar(new Vector3(factor, factor, factor));
        }

    }

}

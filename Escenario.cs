using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Escenario
    {
        public List<Objeto> Objetos { get; set; } = new List<Objeto>();
        public Vector3 CentroRelativo { get; set; }

        public Escenario()
        {
            Objetos = new List<Objeto>();
            CentroRelativo = Vector3.Zero;
        }

        public void CalcularCentroDeMasa()
        {
            Vector3 suma = Vector3.Zero;
            foreach (var obj in Objetos)
            {
                obj.CalcularCentroDeMasa();
                suma += obj.CentroRelativo;
            }
            CentroRelativo = Objetos.Count > 0 ? suma / Objetos.Count : Vector3.Zero;
        }

        public void Dibujar()
        {
            foreach (var objeto in Objetos)
            {
                objeto.Dibujar(CentroRelativo);
            }
        }
        public void Inicializar()
        {
            foreach (var obj in Objetos)
            {
                obj.Inicializar(); 
            }
        }
        public void Trasladar(Vector3 desplazamiento)
        {
            foreach (var objeto in Objetos)
            {
                objeto.Trasladar(desplazamiento);
            }

            CentroRelativo += desplazamiento;
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

            foreach (var objeto in Objetos)
            {
                Vector4 pos = new Vector4(objeto.Posicion, 1.0f);
                pos = transform * pos;
                objeto.Posicion = new Vector3(pos.X, pos.Y, pos.Z);

                objeto.Rotar(anguloEnRadianes); // ahora rota sus partes
            }

        }
        public void Escalar(Vector3 factor)
        {
            foreach (var objeto in Objetos)
            {
                Vector3 desplazado = objeto.Posicion - CentroRelativo;
                Vector3 escalado = desplazado * factor;
                objeto.Posicion = CentroRelativo + escalado;

                objeto.Escalar(factor); // escalar internamente
            }

        }
        public void Escalar(float factor)
        {
            Escalar(new Vector3(factor, factor, factor));
        }

    }
}

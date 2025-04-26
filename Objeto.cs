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


    }

}

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

    }
}

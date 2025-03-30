using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Escenario
    {
        public List<Objeto> Objetos { get; private set; }
        public Vector3 CentroRelativo { get; private set; }

        public Escenario()
        {
            Objetos = new List<Objeto>();
            CentroRelativo = Vector3.Zero;
        }

        public void CalcularCentroDeMasa()
        {
            if (Objetos.Count == 0) return;

            Vector3 suma = Vector3.Zero;
            foreach (var objeto in Objetos)
            {
                objeto.CalcularCentroDeMasa();
                suma += objeto.CentroRelativo + objeto.PosicionRelativa;
            }

            CentroRelativo = suma / Objetos.Count;

            // Ajustar la posición relativa de cada Objeto
            for (int i = 0; i < Objetos.Count; i++)
            {
                Objetos[i].PosicionRelativa -= CentroRelativo;
            }
        }


        public void AgregarObjeto(Objeto objeto)
        {
            Objetos.Add(objeto);
        }

        public void Dibujar(Renderer renderer)
        {
            foreach (var objeto in Objetos)
            {
                objeto.Dibujar(renderer, CentroRelativo);
            }
        }

    }
}

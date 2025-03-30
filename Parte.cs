using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Parte
    {
        public string Nombre { get; set; }
        public List<Poligono> Poligonos { get; private set; }
        public Vector3 PosicionRelativa { get; set; }
        public Vector3 CentroRelativo { get; private set; }

        public Parte(string nombre = "", Vector3 posicionRelativa = default)
        {
            Nombre = nombre;
            Poligonos = new List<Poligono>();
            PosicionRelativa = posicionRelativa;
            CentroRelativo = Vector3.Zero;
        }

        public void CalcularCentroDeMasa()
        {
            if (Poligonos.Count == 0)
            {
                CentroRelativo = Vector3.Zero;
                return;
            }

            Vector3 suma = Vector3.Zero;
            foreach (var poligono in Poligonos)
            {
                poligono.CalcularCentroDeMasa();
                suma += poligono.CentroRelativo;
            }

            CentroRelativo = suma / Poligonos.Count;

            for (int i = 0; i < Poligonos.Count; i++)
            {
                Poligonos[i].Vertices = Poligonos[i].Vertices.ConvertAll(v => v - CentroRelativo);
            }

            PosicionRelativa -= CentroRelativo;
        }


        public void Mover(Vector3 desplazamiento)
        {
            PosicionRelativa += desplazamiento;  // Mover la parte con el desplazamiento
        }

        // Método para agregar un polígono
        public void AgregarPoligono(Poligono poligono)
        {
            Poligonos.Add(poligono);
        }

        public void Dibujar(Renderer renderer, Vector3 posicionGlobalObjeto)
        {
            Vector3 posicionGlobalParte = posicionGlobalObjeto + PosicionRelativa;

            foreach (var poligono in Poligonos)
            {
                poligono.Dibujar(renderer, posicionGlobalParte);  // Llamada al método Dibujar() de Poligono
            }
        }



    }

}

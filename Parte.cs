using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public class Parte
    {
        public List<Poligono> Poligonos { get; set; } = new List<Poligono>();
        public Vector3 CentroRelativo { get; set; }
        public Vector3 PosicionRelativa { get; set; } = Vector3.Zero;

        public Parte()
        {
            Poligonos = new List<Poligono>();
            CentroRelativo = Vector3.Zero;
        }

        public void Dibujar(Vector3 offsetObjeto)
        {
            Matrix4 model = Matrix4.CreateTranslation(offsetObjeto + PosicionRelativa + CentroRelativo);
            foreach (var poligono in Poligonos)
            {
                poligono.Dibujar(model); 
            }
        }

        public void CalcularCentroDeMasa()
        {
            Vector3 suma = Vector3.Zero;
            foreach (var poligono in Poligonos)
            {
                poligono.CalcularCentroDeMasa();
                suma += poligono.CentroRelativo;
            }
            CentroRelativo = Poligonos.Count > 0 ? suma / Poligonos.Count : Vector3.Zero;
        }
        public void Inicializar()
        {
            foreach (var poligono in Poligonos)
            {
                poligono.CalcularCentroDeMasa();
                poligono.Inicializar();
            }
        }
        public void Trasladar(Vector3 desplazamiento)
        {
            foreach (var poligono in Poligonos)
            {
                poligono.Trasladar(desplazamiento);
            }

            CentroRelativo += desplazamiento;
            PosicionRelativa += desplazamiento;
        }
        public void Rotar(Vector3 anguloEnRadianes)
        {
            foreach (var poligono in Poligonos)
            {
                poligono.Rotar(anguloEnRadianes, CentroRelativo);
            }

            CalcularCentroDeMasa(); 
        }

        public void Escalar(Vector3 factor)
        {
            foreach (var poligono in Poligonos)
            {
                poligono.Escalar(factor, CentroRelativo);
            }

            CalcularCentroDeMasa();
        }
        public void Escalar(float factor)
        {
            Escalar(new Vector3(factor, factor, factor));
        }

    }

}

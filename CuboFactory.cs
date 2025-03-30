using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public static class CuboFactory
    {
        // Colores por defecto para cada cara (pasteles para mejor visualización)
        private static readonly List<Vector4> _coloresPorDefecto = new List<Vector4>
        {
                new Vector4(1f, 0f, 0f, 1f), // Rojo
                new Vector4(0f, 1f, 0f, 1f), // Verde
                new Vector4(0f, 0f, 1f, 1f), // Azul
                new Vector4(1f, 1f, 0f, 1f), // Amarillo
                new Vector4(1f, 0f, 1f, 1f), // Magenta
                new Vector4(0f, 1f, 1f, 1f)  // Cyan
        };

        public static Parte CrearCubo(string nombre, float width, float height, float depth,
                                    List<Vector4> coloresCarasPersonalizados = null)
        {
            var parte = new Parte { Nombre = nombre };
            float hw = width / 2, hh = height / 2, hd = depth / 2;

            var vertices = new List<Vector3>
            {
                new Vector3(-hw, -hh, -hd), // 0: Esquina inferior trasera izquierda
                new Vector3(hw, -hh, -hd),  // 1: Esquina inferior trasera derecha
                new Vector3(hw, hh, -hd),   // 2: Esquina superior trasera derecha
                new Vector3(-hw, hh, -hd),  // 3: Esquina superior trasera izquierda
                new Vector3(-hw, -hh, hd),  // 4: Esquina inferior delantera izquierda
                new Vector3(hw, -hh, hd),   // 5: Esquina inferior delantera derecha
                new Vector3(hw, hh, hd),    // 6: Esquina superior delantera derecha
                new Vector3(-hw, hh, hd)    // 7: Esquina superior delantera izquierda
            };

            // Manejo seguro de colores
            List<Vector4> coloresFinales;
            if (coloresCarasPersonalizados == null)
            {
                // Usar colores por defecto
                coloresFinales = _coloresPorDefecto;
            }
            else if (coloresCarasPersonalizados.Count >= 6)
            {
                // Usar colores personalizados (los primeros 6)
                coloresFinales = coloresCarasPersonalizados.GetRange(0, 6);
            }
            else
            {
                // Combinar: personalizados + completar con defecto
                coloresFinales = new List<Vector4>(coloresCarasPersonalizados);
                while (coloresFinales.Count < 6)
                {
                    coloresFinales.Add(_coloresPorDefecto[coloresFinales.Count]);
                }
            }

            // Definición de caras del cubo (vértices y color correspondiente)
            var definicionCaras = new List<(int[], Vector4)>
            {
                (new[] {0, 1, 2, 3}, coloresFinales[0]), // Cara trasera
                (new[] {4, 5, 6, 7}, coloresFinales[1]), // Cara delantera
                (new[] {0, 3, 7, 4}, coloresFinales[2]), // Cara izquierda
                (new[] {1, 2, 6, 5}, coloresFinales[3]), // Cara derecha
                (new[] {0, 1, 5, 4}, coloresFinales[4]), // Cara inferior
                (new[] {3, 2, 6, 7}, coloresFinales[5])  // Cara superior
            };

            // Crear polígonos para cada cara
            foreach (var (indicesCara, colorCara) in definicionCaras)
            {
                // Convertir quad en dos triángulos (0-1-2 y 0-2-3)
                var indices = new List<int> {
                    indicesCara[0], indicesCara[1], indicesCara[2], // Primer triángulo
                    indicesCara[0], indicesCara[2], indicesCara[3]  // Segundo triángulo
                };

                var poligono = new Poligono(vertices, indices, colorCara);
                parte.AgregarPoligono(poligono);
            }

            return parte;
        }
    }
}
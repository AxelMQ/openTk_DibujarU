using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTK_DibujarU
{
    public static class ObjetoU
    {
        // Valores por defecto (constantes)
        private const float AnchoDefault = 1.0f;
        private const float AlturaDefault = 2.0f;
        private const float ProfundidadDefault = 0.5f;

        // Método simplificado
        public static Objeto CrearU(Vector4 color)
        {
            return CrearU(AnchoDefault, AlturaDefault, ProfundidadDefault, color);
        }
        public static Objeto CrearU(float anchoTotal, float alturaTotal, float profundidad, Vector4 color)
        {
            var objeto = new Objeto();

            // Dimensiones proporcionales
            float grosor = anchoTotal * 0.2f;  // Grosor de las patas
            float alturaBase = alturaTotal * 0.2f; // Altura de la base

            // Crear las tres partes principales
            var baseU = CrearBase(anchoTotal, alturaBase, profundidad, color);
            var pataIzquierda = CrearPata(grosor, alturaTotal, profundidad, color);
            var pataDerecha = CrearPata(grosor, alturaTotal, profundidad, color);

            // Posicionamiento relativo
            baseU.PosicionRelativa = new Vector3(0, -alturaTotal / 2 + alturaBase / 2, 0);
            pataIzquierda.PosicionRelativa = new Vector3(-anchoTotal / 2 + grosor / 2, alturaBase / 2, 0);
            pataDerecha.PosicionRelativa = new Vector3(anchoTotal / 2 - grosor / 2, alturaBase / 2, 0);

            objeto.Partes.Add(baseU);
            objeto.Partes.Add(pataIzquierda);
            objeto.Partes.Add(pataDerecha);

            return objeto;
        }

        private static Parte CrearBase(float ancho, float altura, float profundidad, Vector4 color)
        {
            return Cubo.CrearCuboSolido(ancho, altura, profundidad, color);
        }

        private static Parte CrearPata(float ancho, float altura, float profundidad, Vector4 color)
        {
            return Cubo.CrearCuboSolido(ancho, altura, profundidad, color);
        }
    }

    public static class Cubo
    {
        public static Parte CrearCuboSolido(float width, float height, float depth, Vector4 color)
        {
            var parte = new Parte();
            float hw = width / 2, hh = height / 2, hd = depth / 2;

            // Definición de los 8 vértices del cubo
            var vertices = new Vector3[]
            {
                new Vector3(-hw, -hh, -hd), // 0: abajo-atrás-izq
                new Vector3(hw, -hh, -hd),  // 1: abajo-atrás-der
                new Vector3(hw, hh, -hd),   // 2: arriba-atrás-der
                new Vector3(-hw, hh, -hd),  // 3: arriba-atrás-izq
                new Vector3(-hw, -hh, hd),  // 4: abajo-adelante-izq
                new Vector3(hw, -hh, hd),   // 5: abajo-adelante-der
                new Vector3(hw, hh, hd),    // 6: arriba-adelante-der
                new Vector3(-hw, hh, hd)    // 7: arriba-adelante-izq
            };

            // Definición de los 12 triángulos (6 caras × 2 triángulos)
            var triangulos = new List<Vector3>
            {
                // Cara trasera
                vertices[0], vertices[1], vertices[2],
                vertices[0], vertices[2], vertices[3],
                
                // Cara delantera
                vertices[4], vertices[6], vertices[5],
                vertices[4], vertices[7], vertices[6],
                
                // Cara izquierda
                vertices[0], vertices[3], vertices[7],
                vertices[0], vertices[7], vertices[4],
                
                // Cara derecha
                vertices[1], vertices[5], vertices[6],
                vertices[1], vertices[6], vertices[2],
                
                // Cara superior
                vertices[3], vertices[2], vertices[6],
                vertices[3], vertices[6], vertices[7],
                
                // Cara inferior
                vertices[0], vertices[4], vertices[5],
                vertices[0], vertices[5], vertices[1]
            };

            // Crear un solo polígono con todos los triángulos (mejor rendimiento)
            parte.Poligonos.Add(new Poligono(triangulos, color));

            return parte;
        }
    }
}
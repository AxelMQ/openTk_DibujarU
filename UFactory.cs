using OpenTK.Mathematics;

namespace OpenTK_DibujarU
{
    public static class UFactory
    {
        public static Objeto CrearU(string nombre, float baseWidth, float verticalHeight, float depth, Vector4 color, 
                                    float baseHeightRatio = 0.5f, float verticalWidthRatio = 0.33f)
        {
            var objeto = new Objeto(nombre);

            // Parámetros de la "U"
            float baseHeight = baseWidth * baseHeightRatio;
            float verticalWidth = baseWidth * verticalWidthRatio;

            // Posiciones ajustadas para cada cubo
            var basePosition = new Vector3(0.0f, -verticalHeight / 2 + baseHeight / 2, 0.0f);
            var leftPosition = new Vector3(-baseWidth / 2 + verticalWidth / 2, 0.0f, 0.0f);
            var rightPosition = new Vector3(baseWidth / 2 - verticalWidth / 2, 0.0f, 0.0f);

            // Crear cubos para cada parte de la "U"
            var baseParte = CuboFactory.CrearCubo("Base", baseWidth, baseHeight, depth);
            var leftParte = CuboFactory.CrearCubo("LadoIzquierdo", verticalWidth, verticalHeight, depth);
            var rightParte = CuboFactory.CrearCubo("LadoDerecho", verticalWidth, verticalHeight, depth);

            // Asignar las posiciones relativas a cada parte
            baseParte.PosicionRelativa = basePosition;
            leftParte.PosicionRelativa = leftPosition;
            rightParte.PosicionRelativa = rightPosition;

            // Agregar las partes al objeto
            objeto.AgregarParte(baseParte);
            objeto.AgregarParte(leftParte);
            objeto.AgregarParte(rightParte);

            return objeto;
        }

    }
}

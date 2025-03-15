public static class Shaders
{
    // Vertex Shader
    public static string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 aPosition;
        layout (location = 1) in vec4 aColor;

        uniform mat4 uProjection;
        uniform mat4 uView;
        uniform mat4 uModel;

        out vec4 vColor;

        void main()
        {
            gl_Position = uProjection * uView * uModel * vec4(aPosition, 1.0);
            vColor = aColor;
        }
    ";

    // Fragment Shader
    public static string FragmentShaderSource = @"
        #version 330 core
        in vec4 vColor;
        out vec4 fragColor;

        void main()
        {
            fragColor = vColor;
        }
    ";
}
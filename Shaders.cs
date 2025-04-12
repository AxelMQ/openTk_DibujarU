using OpenTK_DibujarU;

public static class Shaders
{
    public static Shader DefaultShader { get;  set; }

    static Shaders()
    {
        DefaultShader = new Shader(VertexShaderSource, FragmentShaderSource);
    }

    public const string VertexShaderSource = @"
        #version 330 core
        
        layout (location = 0) in vec3 aPosition;
        layout (location = 1) in vec3 aColor;

        uniform mat4 projection;
        uniform mat4 view;
        uniform mat4 model;

        out vec3 fragmentColor;

        void main()
        {
            gl_Position = projection * view * model * vec4(aPosition, 1.0);
            fragmentColor = aColor;
        }";

    public const string FragmentShaderSource = @"
        #version 330 core
        
        in vec3 fragmentColor;
        uniform vec4 overrideColor;
        
        out vec4 outputColor;

        void main()
        {
            outputColor = overrideColor * vec4(fragmentColor, 1.0);
        }";
}
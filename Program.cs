using System;

namespace OpenTK_DibujarU
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var window = new Window(800, 600, "OpenTK Test - Figura 3D"))
            {
                window.Run();
            }
        }
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK_DibujarU;
using System.IO;

namespace OpenTK_DibujarU
{
    public static class Serializador
    {
        private static JsonSerializerOptions GetSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Converters =
                {
                    new Vector3Converter(),
                    new Vector4Converter()
                }
            };
        }

        public static void GuardarObjeto(string path, Objeto obj)
        {
            var options = GetSerializerOptions();
            var json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(path, json);
        }

        public static Objeto CargarObjeto(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo {path} no existe.");

            var json = File.ReadAllText(path);
            var options = GetSerializerOptions();

            var objeto = JsonSerializer.Deserialize<Objeto>(json, options);
            if (objeto == null)
                throw new Exception("Error al deserializar el objeto desde JSON.");
            return objeto;
        }

        public static void GuardarEscenario(string path, Escenario escenario)
        {
            var options = GetSerializerOptions();
            var json = JsonSerializer.Serialize(escenario, options);
            File.WriteAllText(path, json);
        }

        public static Escenario CargarEscenario(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"El archivo {path} no existe.");

            var json = File.ReadAllText(path);
            var options = GetSerializerOptions();

            var escenario = JsonSerializer.Deserialize<Escenario>(json, options);
            if (escenario == null)
                throw new Exception("Error al deserializar el escenario.");

            foreach (var obj in escenario.Objetos)
                obj.Inicializar();

            return escenario;
        }
    }
}

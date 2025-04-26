// Converters.cs
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTK_DibujarU;

namespace OpenTK_DibujarU
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float x = 0, y = 0, z = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) break;
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var name = reader.GetString();
                    reader.Read();
                    switch (name)
                    {
                        case "X": x = reader.GetSingle(); break;
                        case "Y": y = reader.GetSingle(); break;
                        case "Z": z = reader.GetSingle(); break;
                    }
                }
            }
            return new Vector3(x, y, z);
        }

        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Z", value.Z);
            writer.WriteEndObject();
        }
    }

    public class Vector4Converter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            float x = 0, y = 0, z = 0, w = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) break;
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var name = reader.GetString();
                    reader.Read();
                    switch (name)
                    {
                        case "X": x = reader.GetSingle(); break;
                        case "Y": y = reader.GetSingle(); break;
                        case "Z": z = reader.GetSingle(); break;
                        case "W": w = reader.GetSingle(); break;
                    }
                }
            }
            return new Vector4(x, y, z, w);
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Z", value.Z);
            writer.WriteNumber("W", value.W);
            writer.WriteEndObject();
        }
    }
}

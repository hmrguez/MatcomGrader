using Newtonsoft.Json;

namespace AutoGrader;

// public class Int32Converter : JsonConverter
// {
//     public override bool CanConvert(Type objectType)
//     {
//         return objectType == typeof(int);
//     }
//
//     public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//     {
//         return Convert.ToInt32(reader.Value);
//     }
//
//     public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//     {
//         writer.WriteValue((int)value);
//     }
// }
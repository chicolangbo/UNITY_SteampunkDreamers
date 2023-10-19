using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class ReinforceDataDictionaryConverter : JsonConverter<Dictionary<string, ReinforceData>>
{
    public override Dictionary<string, ReinforceData> ReadJson(JsonReader reader, Type objectType, Dictionary<string, ReinforceData> existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var result = new Dictionary<string, ReinforceData>();

        foreach (var property in jsonObject.Properties())
        {
            var reinforceData = property.Value.ToObject<ReinforceData>();
            result.Add(property.Name, reinforceData);
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, Dictionary<string, ReinforceData> value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);
            serializer.Serialize(writer, kvp.Value);
        }

        writer.WriteEndObject();
    }
}

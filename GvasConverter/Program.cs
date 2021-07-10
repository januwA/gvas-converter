using System;
using System.IO;
using System.Text;
using GvasFormat;
using GvasFormat.Serialization;
using GvasFormat.Serialization.UETypes;
using Newtonsoft.Json;


namespace GvasConverter
{
  class Program
  {
    static void Main(string[] args)
    {
      if(args.Length == 0)
      {
        Console.WriteLine("\n> GvasConverter.exe \"your.sva\"");
        return;
      }

      var GVASPath = args[0];
      // var GVASPath = $"C:\\Users\\ajanuw\\Documents\\My Games\\Octopath_Traveler\\76561197960267366\\SaveGames\\SaveData0.sav";
      var jsonoutpath = GVASPath + ".json";

      // 解析
      Gvas save = UESerializer.Read(File.Open(GVASPath, FileMode.Open, FileAccess.Read, FileShare.Read));

      // object to json
      // var json = JsonConvert.SerializeObject(save, new JsonSerializerSettings{Formatting = Formatting.Indented}); 
      var json = JsonConvert.SerializeObject(save, Formatting.Indented, new MyJsonConvert());

      var writer = new StreamWriter(File.Open(jsonoutpath, FileMode.Create, FileAccess.Write, FileShare.Read), new UTF8Encoding(true));
      writer.Write(json);
      writer.Close();
    }

  }

  /// <summary>
  /// 优化json输出
  /// </summary>
  public class MyJsonConvert : JsonConverter<UEProperty>
  {
    public override UEProperty ReadJson(JsonReader reader, Type objectType, UEProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, UEProperty value, JsonSerializer serializer)
    {
      writer.WriteRawValue(JsonConvert.SerializeObject(value.ToObject(), Formatting.Indented));
    }
  }
}

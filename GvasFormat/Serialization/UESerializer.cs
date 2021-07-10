using System;
using System.IO;
using System.Linq;
using System.Text;
using GvasFormat.Serialization.UETypes;
using GvasFormat.Utils;

namespace GvasFormat.Serialization
{
  public static partial class UESerializer
  {
    public static Gvas Read(Stream stream)
    {
      using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
      {
        var header = reader.ReadBytes(Gvas.Header.Length);
        if (!Gvas.Header.SequenceEqual(header))
          throw new FormatException($"Invalid header, expected {Gvas.Header.AsHex()}");

        var result = new Gvas();
        result.SaveGameVersion = reader.ReadInt32();
        result.PackageVersion = reader.ReadInt32();
        result.EngineVersion.Major = reader.ReadInt16();
        result.EngineVersion.Minor = reader.ReadInt16();
        result.EngineVersion.Patch = reader.ReadInt16();
        result.EngineVersion.Build = reader.ReadInt32();
        result.EngineVersion.BuildId = reader.ReadUEString();

        result.CustomFormatVersion = reader.ReadInt32();
        result.CustomFormatData.Count = reader.ReadInt32();

        // Entries 好像没什么用，不输出json
        result.CustomFormatData.Entries = null;
        reader.BaseStream.Position += (16 + 4) * result.CustomFormatData.Count;
        /*
        result.CustomFormatData.Entries = new CustomFormatDataEntry[result.CustomFormatData.Count];
        for (var i = 0; i < result.CustomFormatData.Count; i++)
        {
          var entry = new CustomFormatDataEntry();
          entry.Id = new Guid(reader.ReadBytes(16));
          entry.Value = reader.ReadInt32();
          result.CustomFormatData.Entries[i] = entry;
        }
         */

        result.SaveGameType = reader.ReadUEString();

        try
        {
          // 这个过程很容易出问题，将当前的数据，输出到json
          while (UEProperty.Read(reader) is UEProperty prop)
            result.Properties.Add(prop);
        }
        catch (Exception e)
        {
          Console.WriteLine($"Bad Result Json: {e}");
          return result;
        }

        return result;
      }
    }
  }
}

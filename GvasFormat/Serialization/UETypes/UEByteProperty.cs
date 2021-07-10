using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GvasFormat.Utils;

namespace GvasFormat.Serialization.UETypes
{
  [DebuggerDisplay("{Value}", Name = "{Name}")]
  public sealed class UEByteProperty : UEProperty
  {
    public UEByteProperty() { }
    public static UEByteProperty Read(BinaryReader reader, long valueLength)
    {
      if (valueLength == 1)
      {
        reader.ReadUEString(); // None
        reader.ReadInt16();// 不清楚
        return new UEByteProperty { Value = "" };
      }

      if (reader.PeekChar() == 0)
      {
        var terminator = reader.ReadByte();
        if (terminator != 0)
          throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected terminator (0x00), but was (0x{terminator:x2})");
        // valueLength starts here
        var arrayLength = reader.ReadInt32();
        var bytes = reader.ReadBytes(arrayLength);
        return new UEByteProperty { Value = bytes.AsHex() };
      }
      else
      {
        var str = "";
        while (true)
        {
          str += reader.ReadUEString();
          if (reader.PeekChar() != 0)
            break;
          reader.ReadByte(); // 0
        }
        return new UEByteProperty { Value = str };
      }

    }

    public static UEProperty[] Read(BinaryReader reader, long valueLength, int count)
    {
      if (valueLength == 0x18)
      {
        // valueLength starts here
        var bytes = reader.ReadBytes(count);
        return new UEProperty[] { new UEByteProperty { Value = bytes.AsHex() } };
      }
      else
      {
        var str = "";
        for (int i = 0; i < count; i++)
        {
          str += reader.ReadUEString();
        }
        return new UEProperty[] { new UEByteProperty { Value = str } };
      }
    }

    public override void Serialize(BinaryWriter writer) => throw new NotImplementedException();

    public string Value;

    public override object ToObject()
    {
      if (Name == null)
        return Value;
      return new { Offset, Name, Value };
    }
  }
}
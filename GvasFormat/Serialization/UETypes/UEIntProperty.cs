using System;
using System.Diagnostics;
using System.IO;

namespace GvasFormat.Serialization.UETypes
{
  [DebuggerDisplay("{Value}", Name = "{Name}")]
  public sealed class UEIntProperty : UEProperty
  {
    public UEIntProperty() { }
    public UEIntProperty(BinaryReader reader, long valueLength)
    {
      if (valueLength != -1)
      {
        var terminator = reader.ReadByte();
        if (terminator != 0)
          throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected terminator (0x00), but was (0x{terminator:x2})");
      }
      Value = reader.ReadInt32();
    }

    public override void Serialize(BinaryWriter writer) { throw new NotImplementedException(); }

    public int Value;

    public override object ToObject()
    {
      if (Name == null)
        return Value;

      return new { Offset, Name, Value };
    }
  }

  [DebuggerDisplay("{Value}", Name = "{Name}")]
  public sealed class UEUInt64Property : UEProperty
  {
    public UEUInt64Property() { }
    public UEUInt64Property(BinaryReader reader, long valueLength)
    {
      // valueLength = -1 来自 array
      if (valueLength != -1)
      {
        var terminator = reader.ReadByte();
        if (terminator != 0)
          throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected terminator (0x00), but was (0x{terminator:x2})");
      }
      Value = reader.ReadUInt64();
    }

    public override void Serialize(BinaryWriter writer) { throw new NotImplementedException(); }

    public ulong Value;
    public override object ToObject()
    {
      if (Name == null)
        return Value;

      return new { Offset, Name, Value };
    }
  }

}
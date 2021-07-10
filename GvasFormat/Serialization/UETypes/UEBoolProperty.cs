using System;
using System.Diagnostics;
using System.IO;

namespace GvasFormat.Serialization.UETypes
{
  [DebuggerDisplay("{Value}", Name = "{Name}")]
  public sealed class UEBoolProperty : UEProperty
  {
    public UEBoolProperty() { }
    public UEBoolProperty(BinaryReader reader, long valueLength)
    {
      var val = valueLength == -1 ? reader.ReadByte() : reader.ReadInt16();
      if (val == 0)
        Value = false;
      else if (val == 1)
        Value = true;
      else
        throw new InvalidOperationException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected bool value, but was {val}");
    }

    public override void Serialize(BinaryWriter writer) { throw new NotImplementedException(); }
    
    
    public bool Value { get; set; }
    public override object ToObject()
    {
      if (Name == null)
        return Value;

      return new { Offset, Name, Value };
    }
  }
}
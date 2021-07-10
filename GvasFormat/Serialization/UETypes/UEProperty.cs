using System.IO;

namespace GvasFormat.Serialization.UETypes
{
  public abstract class UEProperty
  {
    public string Name;
    public string Type;

    // 给出一个大致的位置，以便使用二进制编辑器寻找
    public long Offset;

    public virtual object ToObject()
    {
      return this;
    }

    public abstract void Serialize(BinaryWriter writer);

    public static UEProperty Read(BinaryReader reader)
    {
      if (reader.PeekChar() < 0)
        return null;

      var name = reader.ReadUEString();
      if (name == null)
        return null;

      if (name == "None")
      {
        return null;
        // 节约json空间，不返回none 
        // return new UENoneProperty { Name = name, Offset = reader.BaseStream.Position - 5 };
      }

      var type = reader.ReadUEString();
      var valueLength = reader.ReadInt64();
      return UESerializer.Deserialize(name, type, valueLength, reader);
    }

    public static UEProperty[] Read(BinaryReader reader, int count)
    {
      if (reader.PeekChar() < 0)
        return null;

      var name = reader.ReadUEString();
      var type = reader.ReadUEString();
      var valueLength = reader.ReadInt64();
      return UESerializer.Deserialize(name, type, valueLength, count, reader);
    }
  }
}
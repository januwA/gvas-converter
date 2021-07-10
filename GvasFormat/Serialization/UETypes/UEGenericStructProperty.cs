using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("Count = {Properties.Count}", Name = "{Name}")]
    public sealed class UEGenericStructProperty : UEStructProperty
    {
        public List<UEProperty> Properties = new List<UEProperty>();

    public override object ToObject()
    {
      var _Properties = Properties.Select(it => it.ToObject());
      if (Name == null)
        return _Properties;

      return new { Offset, Name, StructType, Properties = _Properties };
    }
  }
}
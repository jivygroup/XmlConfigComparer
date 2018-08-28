using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MP.XmlConfigComparer.Core.Helpers
{
  public static class XElementExtentions
  {
    public static int? GetLineNumber(this XElement element)
    {
      if (element is IXmlLineInfo lineinfo && lineinfo.HasLineInfo())
      {
        return lineinfo.LineNumber;
      }

      return null;
    }
  }
}

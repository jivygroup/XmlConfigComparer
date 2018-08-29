using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MP.XmlConfigComparer.Core.Helpers
{
  public static class XElementExtensions
  {
    public static int? GetLineNumber(this XElement element)
    {
      if (element is IXmlLineInfo lineinfo && lineinfo.HasLineInfo())
      {
        return lineinfo.LineNumber;
      }

      return null;
    }


    public static bool DeepEqualsWithNormalization(XElement element1, XElement element2)
    {
      XElement d1 = NormalizeElement(element1);
      XElement d2 = NormalizeElement(element2);
      return XNode.DeepEquals(d1, d2);
    }

    private static XElement NormalizeElement(XElement element)
    {
      if (element == null)
      {
        return null;
      }
      return new XElement(element.Name,
          NormalizeAttributes(element),
          element.Nodes().Select(NormalizeNode)
      );

    }


    private static IEnumerable<XAttribute> NormalizeAttributes(XElement element)
    {
      return element.Attributes()
        .OrderBy(a => a.Name.NamespaceName)
        .ThenBy(a => a.Name.LocalName);
        
    }


    private static XNode NormalizeNode(XNode node)
    {
      // trim comments and processing instructions from normalized tree
      if (node is XComment || node is XProcessingInstruction)
        return null;
      if (node is XElement e)
        return NormalizeElement(e);
      // Only thing left is XCData and XText, so clone them
      return node;
    }
  }
}

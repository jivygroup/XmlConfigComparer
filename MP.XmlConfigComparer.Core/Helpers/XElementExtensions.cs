using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
      return DeepEqualsWithNormalizationString(element1, element2).Diff == null;
    }

    public static  (string Diff,string Value) DeepEqualsWithNormalizationString(XElement element1, XElement element2)
    {
      XElement d1 = NormalizeElement(element1);
      XElement d2 = NormalizeElement(element2);

      var res = DeepEquals(d1, d2);
      //in case of bug !!!!
      if (res.Diff == null &&  !XNode.DeepEquals(d1,d2))
      {
        return ($"element:{d1.Name}", d1.Value);
      }

      return res;

    }

    private static (string Diff,string Value) DeepEquals(XElement element1, XElement element2)
    {
      if (ReferenceEquals(element1, element2))
      {
        return (null,null);
      }

      if (element1 == null || element2 == null)
      {
        return element1 != null ? ($"element:{element1.Name}",element1.Value) : ($"element:{element2.Name}",element2.Value);
      }

      var attDiff = AttributesEquals(element1, element2);

      if (attDiff.Diff != null)
      {
        return attDiff;
      }

      var descendantslist1 = element1.Descendants().ToList();
      var descendantslist2 = element2.Descendants().ToList();

      if (descendantslist1.Count != descendantslist2.Count)
      {
        return descendantslist1.Count > descendantslist2.Count ? ($"element:{descendantslist1[descendantslist2.Count].Name}",descendantslist1[descendantslist2.Count].Value) : ($"element:{descendantslist2[descendantslist1.Count].Name}",descendantslist2[descendantslist1.Count].Value);
      }

      for (var i = 0; i < descendantslist1.Count; i++)
      {
        var diffRes = DeepEquals(descendantslist1[i], descendantslist2[i]);
        if (diffRes.Diff != null)
        {
          return diffRes;
        }
       
      }

      return (null, null);

    }

    private static (string Diff,string Value) AttributesEquals(XElement element1, XElement element2)
    {

      var attrList1 = element1.Attributes().ToList();
      var attrList2 = element2.Attributes().ToList();

      if (attrList1.Count != attrList2.Count)
      {
        return attrList1.Count > attrList2.Count ? ($"attr:{attrList1[attrList2.Count].Name}",attrList1[attrList2.Count].Value) : ($"attr:{attrList2[attrList1.Count].Name}",attrList2[attrList1.Count].Value);
      }

      for (int i = 0; i < attrList1.Count; i++)
      {
        var attr1 = attrList1[i];
        var attr2 = attrList2[i];
        if (attr1.Name != attr2.Name || attr1.Value != attr2.Value)
        {
          return ($"attr:{attr1.Name}",attr1.Value);
        }
      }
      return (null,null);

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

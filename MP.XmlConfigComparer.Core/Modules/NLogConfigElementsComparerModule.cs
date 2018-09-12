using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class NLogConfigElementsComparerModule  : ConfigElementComparerModule
  {
    public override string DiffType => "nlog";

    public override string ElementName => "nlog";

    protected override bool ShouldBeExcluded(XElement configElements)
    {
      var configSectionsElements = configElements?.Element("configSections")?.Descendants("section");

      if (configSectionsElements == null)
      {
        return false;
      }

      var hasLog4NetConfigSection = configSectionsElements.Any(element => element.Attribute("name")?.Value == "nlog");

      return hasLog4NetConfigSection;
    }
  }
}

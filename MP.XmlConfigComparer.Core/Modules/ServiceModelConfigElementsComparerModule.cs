using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class ServiceModelConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "ServiceModule";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      var serviceModelElement1 = configElements1.Element("system.serviceModel");
      var serviceModelElement2= configElements2.Element("system.serviceModel");

      if (serviceModelElement1 == null && serviceModelElement2 == null)
      {
        return Task.FromResult(new List<ConfigurationDiff>());
      }

      if (serviceModelElement1 == null || serviceModelElement2 == null ||
          !XNode.DeepEquals(serviceModelElement1, serviceModelElement2))
      {
        return Task.FromResult(new List<ConfigurationDiff>
        {
          new ConfigurationDiff()
          {
            Identifier = "system.serviceModel",
            ConfigurationItem1 = new ConfigurationElement {Value = serviceModelElement1?.ToString(),LineNum = serviceModelElement1?.GetLineNumber() },
            ConfigurationItem2 = new ConfigurationElement {Value = serviceModelElement2?.ToString(),LineNum = serviceModelElement2?.GetLineNumber() }
          }
        });
      }
      return Task.FromResult(new List<ConfigurationDiff>());
      
    }
  }
}

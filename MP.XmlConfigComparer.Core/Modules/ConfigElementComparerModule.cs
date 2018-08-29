using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public abstract class ConfigElementComparerModule : IConfigElementsComparerModule
  {
    public abstract string DiffType { get; }

    public abstract string ElementName { get; }

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      var element1 = configElements1.Element(ElementName);
      var element2 = configElements2.Element(ElementName);

      if (element1 == null && element2 == null)
      {
        return Task.FromResult(new List<ConfigurationDiff>());
      }

      if (element1 == null || element2 == null ||
          !XNode.DeepEquals(element1, element2))
      {
        return Task.FromResult(new List<ConfigurationDiff>
        {
          new ConfigurationDiff()
          {
            Identifier = DiffType,
            ConfigurationItem1 = element1 == null ? null : new ConfigurationElement {Value = element1.ToString(),LineNum = element1.GetLineNumber() },
            ConfigurationItem2 = element2 == null ? null : new ConfigurationElement {Value = element2.ToString(),LineNum = element2.GetLineNumber() },
          }
        });
      }
      return Task.FromResult(new List<ConfigurationDiff>());
    }
  }

}

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

    public string NameSpace { get; set; }

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      var elementName = ElementName;
      if (NameSpace != null)
      {
        elementName = "{" + NameSpace + "}" + elementName;
      }
      var element1 = ShouldBeExcluded(configElements1) ? null : configElements1.Name == elementName ? configElements1 : configElements1.Element(elementName);
      var element2 = ShouldBeExcluded(configElements2) ? null : configElements2.Name == elementName ? configElements2 : configElements2.Element(elementName);

      if (element1 == null && element2 == null)
      {
        return Task.FromResult(new List<ConfigurationDiff>());
      }

      if (element1 == null || element2 == null ||
          !XElementExtensions.DeepEqualsWithNormalization(element1, element2))
      {
        var diff = XElementExtensions.DeepEqualsWithNormalizationString(element1, element2);
        return Task.FromResult(new List<ConfigurationDiff>
        {
          new ConfigurationDiff()
          {
            Identifier = $"{ElementName}-{diff}" ,
            ConfigurationItem1 = element1 == null ? null : new ConfigurationElement {Value = element1.ToString(),LineNum = element1.GetLineNumber() },
            ConfigurationItem2 = element2 == null ? null : new ConfigurationElement {Value = element2.ToString(),LineNum = element2.GetLineNumber() },
          }
        });
      }
      return Task.FromResult(new List<ConfigurationDiff>());
    }

    protected virtual bool ShouldBeExcluded(XElement configElements2) => false;

  }

}

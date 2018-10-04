using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class ConfigSectionConfigElementsComparerModule : IConfigElementsComparerModule
  {
    private readonly string[] ExculudeConfigNames = new string[]{"log4net","nlog"};

    public string DiffType => "ConfigSections";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      List<ConfigSectionInfo> configSections1 = ReadConfigSections(configElements1);
      List<ConfigSectionInfo> configSections2 = ReadConfigSections(configElements2);


      var configSectionsDic1 = configSections1.ToDictionary(info => info.Name);
      var configSectionsDic2 = configSections2.ToDictionary(info => info.Name);

      var allkeys = configSectionsDic1.Keys.Union(configSectionsDic2.Keys);

      List<ConfigurationDiff> diffList = new List<ConfigurationDiff>();
      foreach (var key in allkeys)
      {
        if (!configSectionsDic1.TryGetValue(key, out ConfigSectionInfo configSectionInfo1) || configSectionInfo1 == null)
        {
          var localConfigSection = configSectionsDic2[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = localConfigSection.Element == null ? $"{key}:declaration" : key,
            ConfigurationItem1 = null,
            ConfigurationItem2 = new ConfigurationElement {Value = localConfigSection.Element?.ToString(),LineNum = localConfigSection.Element?.GetLineNumber()}
          });
        }
        else if (!configSectionsDic2.TryGetValue(key, out ConfigSectionInfo configSectionInfo2) || configSectionInfo2 == null)
        {
          var localConfigSection = configSectionsDic1[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = localConfigSection.Element == null ? $"{key}:declaration" : key,
            ConfigurationItem2 = null,
            ConfigurationItem1 = new ConfigurationElement {Value = localConfigSection.Element?.ToString(),LineNum = localConfigSection.Element?.GetLineNumber()}
          });
          
        }

       

        
        else if (IsEqual(configSectionInfo1, configSectionInfo2).Diff!= null)
        {
          var diff = IsEqual(configSectionInfo1, configSectionInfo2);
          diffList.Add(new ConfigurationDiff
          {
            Identifier = $"{key}-{diff.Diff}-{diff.Value}",
            ConfigurationItem1 = configSectionInfo1.Element == null ? null : new ConfigurationElement {Value = configSectionInfo1.Element?.ToString(),LineNum = configSectionInfo1.Element?.GetLineNumber()},
            ConfigurationItem2 = configSectionInfo2.Element == null ? null :new ConfigurationElement {Value = configSectionInfo2.Element?.ToString(),LineNum = configSectionInfo2.Element?.GetLineNumber()}
          });
        }

        else if (configSectionInfo1.Type != configSectionInfo2.Type)
        {
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = new ConfigurationElement {Value = configSectionInfo1.Type,LineNum = configSectionInfo1.Element?.GetLineNumber()},
            ConfigurationItem2 = new ConfigurationElement {Value = configSectionInfo2.Type,LineNum = configSectionInfo2.Element?.GetLineNumber()}
          });
        }

      }

    

      return Task.FromResult(diffList);
      
    }

    private (string Diff,string Value)  IsEqual(ConfigSectionInfo configSectionInfo1, ConfigSectionInfo configSectionInfo2)
    {
      if ((configSectionInfo1 == null && configSectionInfo2 == null) ||
          (configSectionInfo1?.Element == null && configSectionInfo2?.Element == null))
      {
        return (null,null);
      }

      return XElementExtensions.DeepEqualsWithNormalizationString(configSectionInfo1.Element, configSectionInfo2.Element);
    }

    private List<ConfigSectionInfo> ReadConfigSections(XElement configElements)
    {
      var configSectionsElements = configElements?.Element("configSections")?.Descendants("section");

      if (configSectionsElements == null)
      {
        return new List<ConfigSectionInfo>();
      }

      return configSectionsElements.Select(element => new ConfigSectionInfo
      {
        Name = element.Attribute("name")?.Value,
        Type = element.Attribute("type")?.Value,
        Element = configElements.Elements().SingleOrDefault(elm => elm.Name.LocalName  == element.Attribute("name")?.Value)

      } ).Where(info => !ExculudeConfigNames.Contains(info.Name) ).ToList();
    }
  }

  internal class ConfigSectionInfo
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public XElement Element { get; set; }
  }
}

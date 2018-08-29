using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class ConnectionStringConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "ConnectionString";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      List<ConnectionString> connectionStrings1 = ReadConnectionStrings(configElements1);
      List<ConnectionString> connectionStrings2 = ReadConnectionStrings(configElements2);

      var connectionStringsDic1 = connectionStrings1.ToDictionary(s => s.Name);
      var connectionStringsDic2 = connectionStrings2.ToDictionary(s => s.Name);

      List<ConfigurationDiff> diffList = new List<ConfigurationDiff>();
      var allkeys = connectionStringsDic1.Keys.Union(connectionStringsDic2.Keys);


      foreach (var key in allkeys)
      {
        if (!connectionStringsDic1.TryGetValue(key, out ConnectionString configSectionInfo1) || configSectionInfo1 == null)
        {
          var localConfigSection = connectionStringsDic2[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = null,
            ConfigurationItem2 = new ConfigurationElement {Value = localConfigSection.Name}
          });
        }
        else if (!connectionStringsDic2.TryGetValue(key, out ConnectionString configSectionInfo2) || configSectionInfo2 == null)
        {
          var localConfigSection = connectionStringsDic1[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem2 = null,
            ConfigurationItem1 = new ConfigurationElement {Value = localConfigSection.Name}
          });
          
        }

      

        else if (!object.Equals(configSectionInfo1.Value,configSectionInfo2.Value))
        {
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = new ConfigurationElement {Value = configSectionInfo1.Value},
            ConfigurationItem2 = new ConfigurationElement {Value = configSectionInfo2.Value}
          });
        }


        else if (!object.Equals(configSectionInfo1.Provider,configSectionInfo2.Provider))
        {
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = new ConfigurationElement {Value = configSectionInfo1.Provider},
            ConfigurationItem2 = new ConfigurationElement {Value = configSectionInfo2.Provider}
          });
        }

      }

      return Task.FromResult(diffList);

    }

    private List<ConnectionString> ReadConnectionStrings(XElement configElements)
    {
      var res = configElements?.Element("connectionStrings")?.Descendants()
        .Select(CreateConnectionString).ToList();

      return res ?? new List<ConnectionString>();
    }

    private ConnectionString CreateConnectionString(XElement element)
    {
      return new ConnectionString
      {
        Name = element.Attribute("name")?.Value,
        Provider = element.Attribute("providerName")?.Value,
        Value = element.Attribute("connectionString")?.Value
      };
    }
  }

  internal class ConnectionString
  {
    public string Name { get; set; }
    public string Value { get; set; }
    public string Provider { get; set; }
  }
}

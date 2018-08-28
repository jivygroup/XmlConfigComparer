using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MP.XmlConfigComparer.Core.Modules
{
  class AppSettingsConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "AppSettings";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      var appsettingsList1 = configElements1.Element("appSettings")?.Descendants().ToList();
      var appsettingsList2 = configElements2.Element("appSettings")?.Descendants().ToList();

      List<ConfigurationDiff> diffMock = new List<ConfigurationDiff>()
      {
        new ConfigurationDiff
        {
          Identifier = "key",
          ConfigurationItem1 = new ConfigurationElement {LineNum = 10,  Value = "10"},
          ConfigurationItem2 = new ConfigurationElement {LineNum = 17,  Value = "11"}
        }
      };

      return Task.FromResult(diffMock);
    }
  }
}

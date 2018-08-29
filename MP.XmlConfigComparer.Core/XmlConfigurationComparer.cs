using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MP.XmlConfigComparer.Core
{
  public class XmlConfigurationComparer : IXmlConfigurationComparer
  {
    private readonly IEnumerable<IConfigElementsComparerModule> _configElementsComparerModules;

    public XmlConfigurationComparer(IEnumerable<IConfigElementsComparerModule> configElementsComparerModules)
    {
      _configElementsComparerModules = configElementsComparerModules;
    }
    public async Task<CompareResult>  Compare(string baseConfigFile, string tragetConfigFile)
    {
      XElement configElement1 = XElement.Load(baseConfigFile,LoadOptions.SetLineInfo);
      XElement configElement2 = XElement.Load(tragetConfigFile,LoadOptions.SetLineInfo);

      List<ConfigurationDiffGroup> configurationDiffGroups = new List<ConfigurationDiffGroup>();

      foreach (var configElementsComparerModule in _configElementsComparerModules)
      {
        var diffRes = await configElementsComparerModule.Compare(configElement1, configElement2);
        if (diffRes?.Count > 0)
        {
          configurationDiffGroups.Add(new ConfigurationDiffGroup {DiffType = configElementsComparerModule.DiffType , ConfigurationDiffs = diffRes});
        }
      }

      return new CompareResult
      {
        IsEqual = !configurationDiffGroups.Any(),
        ConfigurationDiffGroups = configurationDiffGroups
      };
    }
  }
}
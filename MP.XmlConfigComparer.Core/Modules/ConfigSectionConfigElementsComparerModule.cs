using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class ConfigSectionConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "ConfigSections";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {

   

      return Task.FromResult(new List<ConfigurationDiff>());
    }
  }
}

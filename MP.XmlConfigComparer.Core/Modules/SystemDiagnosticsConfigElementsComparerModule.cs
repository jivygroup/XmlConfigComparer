using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class SystemDiagnosticsConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "SystemDiagnostics";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      var diagElement1 = configElements1.Element("system.diagnostics");
      var diagElement2 = configElements2.Element("system.diagnostics");

      if (diagElement1 == null && diagElement2 == null)
      {
        return Task.FromResult(new List<ConfigurationDiff>());
      }

      if (diagElement1 == null || diagElement2 == null ||
          !XNode.DeepEquals(diagElement1, diagElement2))
      {
        return Task.FromResult(new List<ConfigurationDiff>
        {
          new ConfigurationDiff()
          {
            Identifier = "system.diagnostics",
            ConfigurationItem1 = new ConfigurationElement {Value = diagElement1?.ToString(),LineNum = diagElement1?.GetLineNumber() },
            ConfigurationItem2 = new ConfigurationElement {Value = diagElement2?.ToString(),LineNum = diagElement2?.GetLineNumber() }
          }
        });
      }
      return Task.FromResult(new List<ConfigurationDiff>());
    }
  }
}

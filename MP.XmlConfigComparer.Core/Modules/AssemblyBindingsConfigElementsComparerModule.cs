using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class AssemblyBindingsConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "AssemblyBindings";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {
      RuntimeAssemblyBindings asseBindings1 = ReadAssemblyBindings(configElements1);
      RuntimeAssemblyBindings asseBindings2 = ReadAssemblyBindings(configElements2);
   

      return Task.FromResult(new List<ConfigurationDiff>());
    }

    private RuntimeAssemblyBindings ReadAssemblyBindings(XElement configElements2)
    {
      return null;
    }
  }

  internal class RuntimeAssemblyBindings
  {
    public bool GcServerEnable { get; set; }
    public List<AssemblyBinding> AssemblyBinding { get; set; }
  }

  public class AssemblyBinding
  {
    public string AssemblyIdentityName { get; set; }
    public string PublicToken { get; set; }
    public string Calture { get; set; }
    public string OldVersion { get; set; }
    public string NewVersion { get; set; }

  }
}

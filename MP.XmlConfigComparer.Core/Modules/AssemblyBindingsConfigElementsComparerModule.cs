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


      List<ConfigurationDiff> diffList = new List<ConfigurationDiff>();

      if (asseBindings1.GcServerEnable != asseBindings2.GcServerEnable)
      {

        diffList.Add(new ConfigurationDiff
        {
          Identifier = "GcServerEnabled",
          ConfigurationItem1 = new ConfigurationElement
          {
            Value = asseBindings1.GcServerEnable.ToString() 
          },
          ConfigurationItem2 = new ConfigurationElement
          {
            Value = asseBindings2.GcServerEnable.ToString() 
          }
        });

      }

      Dictionary<string, DependentAssemblyBinding> dependentAssemblyBindingDic1 =
        asseBindings1.DependentAssemblyBindings.ToDictionary(binding => binding.Key);

      Dictionary<string, DependentAssemblyBinding> dependentAssemblyBindingDic2 =
        asseBindings2.DependentAssemblyBindings.ToDictionary(binding => binding.Key);

      var allkeys = dependentAssemblyBindingDic1.Keys.Union(dependentAssemblyBindingDic2.Keys);


      foreach (var key in allkeys)
      {
        if (!dependentAssemblyBindingDic1.TryGetValue(key, out DependentAssemblyBinding configSectionInfo1) || configSectionInfo1 == null)
        {
          var localConfigSection = dependentAssemblyBindingDic2[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = null,
            ConfigurationItem2 = new ConfigurationElement {Value = localConfigSection.Element?.ToString(),LineNum = localConfigSection.Element?.GetLineNumber()}
          });
        }
        else if (!dependentAssemblyBindingDic2.TryGetValue(key, out DependentAssemblyBinding configSectionInfo2) || configSectionInfo2 == null)
        {
          var localConfigSection = dependentAssemblyBindingDic1[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem2 = null,
            ConfigurationItem1 = new ConfigurationElement {Value = localConfigSection.Element?.ToString(),LineNum = localConfigSection.Element?.GetLineNumber()}
          });
          
        }

      

        else if (!IsEqual(configSectionInfo1,configSectionInfo2))
        {
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = new ConfigurationElement {Value = configSectionInfo1.Element?.ToString(),LineNum = configSectionInfo1.Element?.GetLineNumber()},
            ConfigurationItem2 = new ConfigurationElement {Value = configSectionInfo2.Element?.ToString(),LineNum = configSectionInfo2.Element?.GetLineNumber()}
          });
        }

      }

      return Task.FromResult(diffList);
    }

    private bool IsEqual(DependentAssemblyBinding configSectionInfo1, DependentAssemblyBinding configSectionInfo2)
    {
      if (configSectionInfo1 == null && configSectionInfo2 == null)
      {
        return true;
      }

      return XElementExtensions.DeepEqualsWithNormalization(configSectionInfo1.Element, configSectionInfo2.Element);
    }

    private RuntimeAssemblyBindings ReadAssemblyBindings(XElement configElements)
    {
      var gcServerElement = configElements?.Element("runtime")?.Descendants("gcServer").FirstOrDefault();


      var dependentAssemblies = configElements?.Element("runtime")
        ?.Descendants("{urn:schemas-microsoft-com:asm.v1}assemblyBinding").FirstOrDefault()
        ?.Descendants("{urn:schemas-microsoft-com:asm.v1}dependentAssembly").ToList();

      return new RuntimeAssemblyBindings
      {
        GcServerEnable = GetGcServer(gcServerElement),
        DependentAssemblyBindings = dependentAssemblies?.Select(CreateDependentAssemblyBinding ).ToList() ?? new List<DependentAssemblyBinding>()
        

      };
    }

    private DependentAssemblyBinding CreateDependentAssemblyBinding(XElement element)
    {
      return new DependentAssemblyBinding
      {
        AssemblyIdentityName = element.Descendants("{urn:schemas-microsoft-com:asm.v1}assemblyIdentity").Single().Attribute("name")?.Value,
        Calture = element.Descendants("{urn:schemas-microsoft-com:asm.v1}assemblyIdentity").Single().Attribute("culture")?.Value,
        PublicToken = element.Descendants("{urn:schemas-microsoft-com:asm.v1}assemblyIdentity").Single().Attribute("publicKeyToken")?.Value,
        Element = element
        
      };
    }

    private bool GetGcServer(XElement gcServerElement)
    {
      if (gcServerElement?.Attribute("enabled") == null)
      {
        return false;
      }

      return bool.Parse(gcServerElement?.Attribute("enabled").Value);
    }
  }

  internal class RuntimeAssemblyBindings
  {
    public bool GcServerEnable { get; set; }
    public List<DependentAssemblyBinding> DependentAssemblyBindings { get; set; }
  }

  public class DependentAssemblyBinding
  {
    public string AssemblyIdentityName { get; set; }
    public string PublicToken { get; set; }
    public string Calture { get; set; }
    public XElement Element { get; set; }


    public string Key => $"{AssemblyIdentityName}_{PublicToken}_{Calture}";


  }
}

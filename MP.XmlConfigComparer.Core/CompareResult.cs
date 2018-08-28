using System.Collections.Generic;

namespace MP.XmlConfigComparer.Core
{
  public class CompareResult
  {
    public bool IsEqual { get; set; }
    public List<ConfigurationDiffGroup> ConfigurationDiffGroups { get; set; }
  }
}
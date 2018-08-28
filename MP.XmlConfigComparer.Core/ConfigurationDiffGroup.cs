using System.Collections.Generic;

namespace MP.XmlConfigComparer.Core
{
  public class ConfigurationDiffGroup
  {
    public string DiffType { get; set; }
    public List<ConfigurationDiff> ConfigurationDiffs { get; set; }
  }
}
namespace MP.XmlConfigComparer.Core
{
  public class ConfigurationDiff
  {
    public string Identifier { get; set; } //key ; attribute:att1 
    public ConfigurationElement ConfigurationItem1 { get; set; }

    public ConfigurationElement ConfigurationItem2 { get; set; }
  }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class ServiceModelConfigElementsComparerModule : ConfigElementComparerModule
  {
    public override string DiffType => "system.serviceModel";

    public override string ElementName => "system.serviceModel";
  }
}

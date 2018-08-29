using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class SystemWebConfigElementsComparerModule : ConfigElementComparerModule
  {
    public override string DiffType => "system.web";

    public override string ElementName => "system.web";
  }

  
}

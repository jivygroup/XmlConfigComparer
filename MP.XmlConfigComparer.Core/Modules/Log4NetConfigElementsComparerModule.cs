﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class Log4NetConfigElementsComparerModule  : ConfigElementComparerModule
  {
    public override string DiffType => "log4net";

    public override string ElementName => "log4net";

  
  }
}

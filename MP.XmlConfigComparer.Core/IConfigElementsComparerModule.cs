using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MP.XmlConfigComparer.Core
{
  public interface IConfigElementsComparerModule
  {
    string DiffType { get;  }
    Task<List<ConfigurationDiff>> Compare(XElement configElements1,XElement configElements2);
  }
}

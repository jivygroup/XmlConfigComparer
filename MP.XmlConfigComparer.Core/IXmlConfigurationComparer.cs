using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.XmlConfigComparer.Core
{
    public interface IXmlConfigurationComparer
    {
      Task<CompareResult> Compare(string baseConfigFile, string tragetConfigFile);
    }
}

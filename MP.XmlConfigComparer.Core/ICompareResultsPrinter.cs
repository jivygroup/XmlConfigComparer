using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.XmlConfigComparer.Core
{
  public interface ICompareResultsPrinter 
  {
    Task<string> PrintResults(CompareResult compareResult, string baseConfigFile, string tragetConfigFile,string outputFile = null);
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.XmlConfigComparer.Core
{
  public interface ICompareResultsPrinter
  {
    Task PrintResults(CompareResult compareResult, string configFile1, string configFile2,string outputFile);
  }
}

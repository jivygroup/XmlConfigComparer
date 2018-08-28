using System;
using System.Linq;
using System.Threading.Tasks;

namespace MP.XmlConfigComparer.Core
{
  public class CompareResultsPrinter : ICompareResultsPrinter
  {
    public async Task PrintResults(CompareResult compareResult, string configFile1, string configFile2, string outputFile)
    {
      Console.WriteLine("compare results of:{0} vs. {1}",configFile1,configFile2);
      if (compareResult.IsEqual)
      {
        Console.WriteLine("No Diff were found!!!");
        return;
      }

      var diffs = compareResult.ConfigurationDiffGroups.SelectMany(group => group.ConfigurationDiffs).ToList();

      foreach (var diff in diffs)
      {
        Console.WriteLine("{4}:{1} (Line {0})\\{3} (Line {2})",diff.ConfigurationItem1?.LineNum,diff.ConfigurationItem1?.Value,
          diff.ConfigurationItem2?.LineNum,diff.ConfigurationItem2?.Value,diff.Identifier);
      }
    }
  }
}
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace MP.XmlConfigComparer.Core
{
  public class CompareResultsPrinter : ICompareResultsPrinter
  {
    public  Task PrintResults(CompareResult compareResult, string configFile1, string configFile2, string outputFile)
    {
      DataTable resultsDataTable = CreateResultsDataTable(compareResult,configFile1,configFile2);
      
      XLWorkbook workbook = new XLWorkbook();
    
      workbook.Worksheets.Add(resultsDataTable );

      workbook.SaveAs(outputFile);


      var diffs = compareResult.ConfigurationDiffGroups.SelectMany(group => group.ConfigurationDiffs).ToList();

      foreach (var diff in diffs)
      {
        Console.WriteLine("{4}:{1} (Line {0})\\{3} (Line {2})",diff.ConfigurationItem1?.LineNum,diff.ConfigurationItem1?.Value,
          diff.ConfigurationItem2?.LineNum,diff.ConfigurationItem2?.Value,diff.Identifier);
      }

      return  Task.CompletedTask;
      
    }

    private DataTable CreateResultsDataTable(CompareResult compareResult, string configFile1, string configFile2)
    {
      DataTable dt = new DataTable("compare results");

      dt.Columns.Add(new DataColumn("config file 1"));
      dt.Columns.Add(new DataColumn("config file 2"));
      dt.Columns.Add(new DataColumn("Identifier"));
      dt.Columns.Add(new DataColumn("Value1"));
      dt.Columns.Add(new DataColumn("Value2"));
      dt.Columns.Add(new DataColumn("LineNum1"));
      dt.Columns.Add(new DataColumn("LineNum2"));


      var diffs = compareResult.ConfigurationDiffGroups.SelectMany(group => group.ConfigurationDiffs).ToList();

      foreach (var diff in diffs)
      {

        dt.Rows.Add(new object[]
        {
          configFile1,
          configFile2,
          diff.Identifier,
          diff.ConfigurationItem1?.Value,
          diff.ConfigurationItem2?.Value,
          diff.ConfigurationItem1?.LineNum,
          diff.ConfigurationItem2?.LineNum,
        });
      }

      return dt;
    }
  }
}
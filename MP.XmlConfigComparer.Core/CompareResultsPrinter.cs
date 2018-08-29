using System;
using System.Data;
using System.Threading.Tasks;
using ClosedXML.Excel;


namespace MP.XmlConfigComparer.Core
{
  public class CompareResultsPrinter : ICompareResultsPrinter
  {
    public Task PrintResults(CompareResult compareResult, string configFile1, string configFile2, string outputFile = null)
    {
      if (outputFile == null)
      {
        outputFile = GenerateFileName(configFile1, configFile2);
      }
      DataTable resultsDataTable = CreateResultsDataTable(compareResult, configFile1, configFile2);
      XLWorkbook workbook = new XLWorkbook();
      workbook.Worksheets.Add(resultsDataTable);
      workbook.SaveAs(outputFile);
      return Task.CompletedTask;
    }

    private string GenerateFileName(string configFile1, string configFile2)
    {
      return $"out_{GetInputFile(configFile1)}_{GetInputFile(configFile2)}_{DateTime.Now:yyyy-dd-M--HH-mm}.xlsx";
    }

    private string GetInputFile(string configFile)
    {
      return $"{System.IO.Path.GetFileNameWithoutExtension(configFile)}";
    }

    private DataTable CreateResultsDataTable(CompareResult compareResult, string configFile1, string configFile2)
    {
      DataTable dt = new DataTable("compare results");
      dt.Columns.Add(new DataColumn("diff source"));
      dt.Columns.Add(new DataColumn("Identifier"));
      dt.Columns.Add(new DataColumn("modification type"));
      dt.Columns.Add(new DataColumn(configFile1));
      dt.Columns.Add(new DataColumn(configFile2));
      dt.Columns.Add(new DataColumn("LineNum1"));
      dt.Columns.Add(new DataColumn("LineNum2"));


      foreach (var compareResultConfigurationDiffGroup in compareResult.ConfigurationDiffGroups)
      {
        foreach (var diff in compareResultConfigurationDiffGroup.ConfigurationDiffs)
        {

          dt.Rows.Add(new object[]
          {
            compareResultConfigurationDiffGroup.DiffType,
            diff.Identifier,
            CalcModificationType(diff),
            diff.ConfigurationItem1?.Value,
            diff.ConfigurationItem2?.Value,
            diff.ConfigurationItem1?.LineNum,
            diff.ConfigurationItem2?.LineNum,
          });
        }

      }

      return dt;
    }

    private string CalcModificationType(ConfigurationDiff diff)
    {
      if (diff.ConfigurationItem1 != null && diff.ConfigurationItem2 != null)
      {
        return ModificationType.Updated.ToString();
      }

      if (diff.ConfigurationItem1 == null)
      {
        return ModificationType.Added.ToString();
      }

      return ModificationType.Removed.ToString();
    }
  }

  public enum ModificationType
  {
    Updated,
    Removed,
    Added
  }
}
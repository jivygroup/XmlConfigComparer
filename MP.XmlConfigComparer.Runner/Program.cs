using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MP.XmlConfigComparer.Core;

namespace MP.XmlConfigComparer.Runner
{
  class Program
  {
    static async Task Main(string[] args)
    {
      try
      {
        IContainer container = BuildContainer();
        var comparer = container.Resolve<IXmlConfigurationComparer>();
        var configFile1 = args[0];
        var configFile2 = args[1];
        var res =  await comparer.Compare(configFile1,configFile2);
        //TODO:move to class
        PrintResults(res,configFile1,configFile2);

      }
      catch (Exception ex)
      {
        PrintUsage();
        Console.Write("error:{0}",ex);
      }
    }

    private static IContainer BuildContainer()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<XmlConfigurationComparer>().As<IXmlConfigurationComparer>();
      builder.RegisterAssemblyTypes(typeof(XmlConfigurationComparer).Assembly)
        .Where(type => typeof(IConfigElementsComparerModule).IsAssignableFrom(type))
        .AsImplementedInterfaces();

      return builder.Build();
    }

    private static void PrintResults(CompareResult compareResult,string configFile1,string configFile2)
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

    private static void PrintUsage()
    {
      Console.WriteLine("usage: MP.XmlConfigComparer.Runner <config file path1> <config file path2>");
    }
  }
}

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
        var outputFile = args[2];
        var res = await comparer.Compare(configFile1, configFile2);
        var printer = container.Resolve<ICompareResultsPrinter>();

        await printer.PrintResults(res, configFile1, configFile2, outputFile);

      }
      catch (Exception ex)
      {
        PrintUsage();
        Console.Write("error:{0}", ex);
      }
    }

    private static IContainer BuildContainer()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<XmlConfigurationComparer>().As<IXmlConfigurationComparer>();
      builder.RegisterType<CompareResultsPrinter>().As<ICompareResultsPrinter>();
      builder.RegisterAssemblyTypes(typeof(XmlConfigurationComparer).Assembly)
        .Where(type => typeof(IConfigElementsComparerModule).IsAssignableFrom(type))
        .AsImplementedInterfaces();

      return builder.Build();
    }


    private static void PrintUsage()
    {
      Console.WriteLine("usage: MP.XmlConfigComparer.Runner <config file path1> <config file path2> <output file>");
    }
  }
}

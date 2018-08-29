using System;
using System.Linq;
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
        var baseConfigFile = args[0];
        var tragetConfigFile = args[1];
        string outputFile = null;
        if (args.Length >= 3 && !string.IsNullOrWhiteSpace(args[2]))
        {
          outputFile = args[2];
        }

        var res = await comparer.Compare(baseConfigFile, tragetConfigFile);
        var printer = container.Resolve<ICompareResultsPrinter>();

        var resfile = await printer.PrintResults(res, baseConfigFile, tragetConfigFile, outputFile);

        Console.WriteLine("Done out file:{0}",resfile);

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
      Console.WriteLine("usage: MP.XmlConfigComparer.Runner <base config file path1> <traget config file path2> <output file>");
    }
  }
}

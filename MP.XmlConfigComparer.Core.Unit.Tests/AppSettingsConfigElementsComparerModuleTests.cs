using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Modules;
using NUnit.Framework;

namespace MP.XmlConfigComparer.Core.Unit.Tests
{
  [TestFixture]
    public class AppSettingsConfigElementsComparerModuleTests
    {

      private readonly AppSettingsConfigElementsComparerModule _appSettingsConfigElementsComparerModule;
      public AppSettingsConfigElementsComparerModuleTests()
      {
        _appSettingsConfigElementsComparerModule = new AppSettingsConfigElementsComparerModule();
      }

      [Test]
      public async Task Compare_NoSetting_NoDiff()
      {
       var res = await  _appSettingsConfigElementsComparerModule.Compare(new XDocument().Root, new XDocument().Root);
        Assert.IsTrue(res.Count == 0);
      }
    }
}

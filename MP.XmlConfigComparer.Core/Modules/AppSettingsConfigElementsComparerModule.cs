﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MP.XmlConfigComparer.Core.Helpers;

namespace MP.XmlConfigComparer.Core.Modules
{
  public class AppSettingsConfigElementsComparerModule : IConfigElementsComparerModule
  {
    public string DiffType => "AppSettings";

    public Task<List<ConfigurationDiff>> Compare(XElement configElements1, XElement configElements2)
    {

      var appsettingsList1 = configElements1?.Element("appSettings")?.Descendants();
      var appsettingsList2 = configElements2?.Element("appSettings")?.Descendants();


      var appsettingsDic1 = ToDictionarySafe(appsettingsList1);
      var appsettingsDic2 = ToDictionarySafe(appsettingsList2);

      var allkeys = appsettingsDic1.Keys.Union(appsettingsDic2.Keys).ToList();

      List<ConfigurationDiff> diffList = new List<ConfigurationDiff>();
      foreach (var key in allkeys)
      {
        if (!appsettingsDic1.TryGetValue(key, out XElement elem1))
        {
          var localElem = appsettingsDic2[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = null,
            ConfigurationItem2 = new ConfigurationElement {Value = localElem?.Attribute("value")?.Value,LineNum = localElem.GetLineNumber()}
          });
        }
        else if (!appsettingsDic2.TryGetValue(key, out XElement elem2))
        {
          var localElem = appsettingsDic1[key];
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem2 = null,
            ConfigurationItem1 = new ConfigurationElement {Value = localElem?.Attribute("value")?.Value,LineNum = localElem.GetLineNumber()}
          });
          
        }

        else if (!(elem1.Attribute("value")?.Value).IsEqualIgnoreCaseAndSpaces(elem2.Attribute("value")?.Value))
        {
          diffList.Add(new ConfigurationDiff
          {
            Identifier = key,
            ConfigurationItem1 = new ConfigurationElement {Value = elem1.Attribute("value")?.Value,LineNum = elem1.GetLineNumber()},
            ConfigurationItem2 = new ConfigurationElement {Value = elem2.Attribute("value")?.Value,LineNum = elem2.GetLineNumber()}
          });
        }

      }

    

      return Task.FromResult(diffList);
    }

    private Dictionary<string,XElement> ToDictionarySafe(IEnumerable<XElement> appsettingsList1)
    {
      Dictionary<string,XElement> res = new Dictionary<string, XElement>();
      if (appsettingsList1 == null)
      {
        return res;
      }
      foreach (var element in appsettingsList1)
      {
        if (element?.Attribute("key")?.Value != null)
          res[element.Attribute("key").Value] = element;
      }

      return res;
    }
  }
}

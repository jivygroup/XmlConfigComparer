using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.XmlConfigComparer.Core.Helpers
{
  public static class StringExtensions
  {
    public static bool IsEqualIgnoreCaseAndSpaces(this string s1, string s2)
    {
      s1 = s1?.Trim();
      s2 = s2?.Trim();
      return string.Compare(s1, s2, StringComparison.InvariantCultureIgnoreCase) == 0;
    }
  }
}

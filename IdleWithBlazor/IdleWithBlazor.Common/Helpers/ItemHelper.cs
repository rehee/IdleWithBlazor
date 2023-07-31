using IdleWithBlazor.Common.Consts;
using IdleWithBlazor.Common.Interfaces.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Helpers
{
  public static class ItemHelper
  {
    public const int MinimumRandom = 70;
    public const int MaximumRandom = 100;
    public static string[] GetRandomProperties(int numberTake)
    {
      return GetRandomArray(numberTake, ConstItem.GeneralItemProperty);
    }
    public static T[] GetRandomArray<T>(int numberTake, T[] list)
    {
      var length = list.Length;
      var random = new Random(ConvertGuidToInt(Guid.NewGuid()));
      return list
        .Select(x => (random.Next(length), x))
        .OrderBy(b => b.Item1)
        .Select(b => b.x)
        .Take(numberTake).ToArray();
    }
    public static int? GetRandomIntValue(double? inputValue, int? min = null, int? max = null)
    {
      if (!inputValue.HasValue)
      {
        return null;
      }
      var result = inputValue * GetRandomPercentage(min, max);
      if (result <= 0)
      {
        return 1;
      }
      return Convert.ToInt32(result);
    }
    public static decimal? GetRandomDecimalValue(double? inputValue, int? min = null, int? max = null)
    {
      if (!inputValue.HasValue)
      {
        return null;
      }
      var result = inputValue * GetRandomPercentage(min, max);
      if (result < 0)
      {
        return 1;
      }
      return Convert.ToDecimal(result);
    }
    public static void SetRandomlValue(double? inputValue, PropertyInfo info, object item, int? min = null, int? max = null)
    {
      if (!inputValue.HasValue)
      {
        return;
      }

      TypeCode typeCode = TypeCode.Object;
      if (info.PropertyType.GenericTypeArguments?.Any() == true)
      {
        typeCode = Type.GetTypeCode(info.PropertyType.GenericTypeArguments.FirstOrDefault());
      }
      else
      {
        typeCode = Type.GetTypeCode(info.PropertyType);
      }
      switch (typeCode)
      {
        case TypeCode.Int32:
          info.SetValue(item, GetRandomIntValue(inputValue, min, max));
          return;
        case TypeCode.Decimal:
          info.SetValue(item, GetRandomDecimalValue(inputValue, min, max));
          return;
      }



    }

    public static double GetRandomPercentage(int? min = null, int? max = null)
    {
      var random = GetNewRandom();
      var range = random.Next(min ?? MinimumRandom, max ?? MaximumRandom);
      var result = range / 100d;
      if (result > 1)
      {
        var a = 1;
      }
      return result;
    }

    public static Random GetNewRandom()
    {
      return new Random(ConvertGuidToInt(Guid.NewGuid()));
    }

    public static int ConvertGuidToInt(this Guid guid)
    {
      byte[] guidBytes = guid.ToByteArray();
      using (MD5 md5 = MD5.Create())
      {
        byte[] hashBytes = md5.ComputeHash(guidBytes);
        int result = BitConverter.ToInt32(hashBytes, 0);
        return result;
      }
    }


  }
}

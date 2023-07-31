using IdleWithBlazor.Common.DTOs;
using IdleWithBlazor.Common.Enums.Numbers;
using IdleWithBlazor.Common.Interfaces;
using IdleWithBlazor.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Consts
{
  public static class ConstItem
  {
    private static ConcurrentDictionary<Type, string[]> typeNameMapping = new ConcurrentDictionary<Type, string[]>();
    private static string[] getTypeNameMapping<T>()
    {
      if (typeNameMapping == null)
      {
        typeNameMapping = new ConcurrentDictionary<Type, string[]>();
      }
      return typeNameMapping.GetOrAdd(typeof(T), typeof(T).GetProperties().Select(b => b.Name).ToArray());
    }

    public static string[] StatusProperty => getTypeNameMapping<IStatusProperty>();
    public static string[] OffensiveProperty => getTypeNameMapping<IOffensiveProperty>();
    public static string[] DefensiveProperty => getTypeNameMapping<IDefensiveProperty>();
    public static string[] SecondaryProperty => getTypeNameMapping<ISecondaryProperty>();

    public static string[] OffensiveItemProperty => OffensiveProperty;
    public static string[] DefensiveItemProperty => StatusProperty.Concat(DefensiveProperty).ToArray();
    public static string[] GeneralItemProperty => StatusProperty.Concat(DefensiveProperty).Concat(DefensiveProperty).ToArray();
    public static Dictionary<string, NumberGrowth> AllPropertyNumberGrowth
    {
      get
      {
        if (allPropertyNumberGrowth != null)
        {
          return allPropertyNumberGrowth;
        }
        var mapper = itemPropertyNumberGrowthDTOs.ToDictionary(x => x.PropertyName ?? "", x => x.GetNumberGrowth());
        allPropertyNumberGrowth = mapper;
        return allPropertyNumberGrowth;
      }
    }
    private static Dictionary<string, NumberGrowth>? allPropertyNumberGrowth { get; set; }
    private static ItemPropertyNumberGrowthDTO[] itemPropertyNumberGrowthDTOs => new ItemPropertyNumberGrowthDTO[]
    {
      new ItemPropertyNumberGrowthDTO(
        nameof(IStatusProperty.Primary),1,EnumNumberGrowth.Linear,2
        ),
      new ItemPropertyNumberGrowthDTO(
        nameof(IStatusProperty.Endurance),1,EnumNumberGrowth.Linear,2
        ),
      new ItemPropertyNumberGrowthDTO(
        nameof(IStatusProperty.Reflection),1,EnumNumberGrowth.Linear,2
        ),
      new ItemPropertyNumberGrowthDTO(
        nameof(IStatusProperty.Will),1,EnumNumberGrowth.Linear,2
        ),
      new ItemPropertyNumberGrowthDTO(
        nameof(IStatusProperty.AllStatus),1,EnumNumberGrowth.Linear,1.5
        ),
    };


    
  }
}

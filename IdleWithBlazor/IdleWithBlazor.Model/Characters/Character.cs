using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Common.Interfaces.Items;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Characters
{
  public class Character : Actor
  {
    public override Type TypeDiscriminator => typeof(Character);


    private Player _player;
    [JsonIgnore]
    public Player ThisPlayer
    {
      get
      {
        if (_player != null)
        {
          return _player;
        }
        var newPlayer = new Player();
        return newPlayer;
      }
    }
    public GameRoom CreateRoom()
    {
      return new GameRoom();
    }

    public void JoinRoom(GameRoom room)
    {

    }
    public Equiptor? ThisEquiptor { get; set; }
    public List<IGameItem>? Inventory { get; set; }

    public int Primary { get; set; }
    public int Endurance { get; set; }
    public int Reflection { get; set; }
    public int Will { get; set; }

    public int MaxInventory { get; set; }

    public bool Equip(Guid id)
    {
      prepareInventory();
      lock (this)
      {
        var item = Inventory.Where(b => b.Id == id).FirstOrDefault();


        Inventory.Remove(item);
      }
      return false;
    }
    public bool UnEquip(params EnumEquipment[] types)
    {
      prepareInventory();
      lock (this)
      {
        if ((MaxInventory - Inventory.Count) < types.Count())
        {
          return false;
        }
        foreach (var type in types)
        {
          switch (type)
          {

          }
        }
      }
      return false;
    }

    public bool Pick(IGameItem item)
    {
      prepareInventory();
      lock (this)
      {
        if (Inventory.Count >= MaxInventory)
        {
          return false;
        }
        Inventory.Add(item);
        return true;
      }
    }

    void prepareInventory()
    {
      lock (this)
      {
        if (Inventory == null)
        {
          Inventory = new List<IGameItem>();
        }
      }

    }
  }
}

using IdleWithBlazor.Common.DTOs.Inventories;
using IdleWithBlazor.Common.Enums;
using IdleWithBlazor.Web.Components;

namespace IdleWithBlazor.Web.Pages
{
  public class BackpackPage : PageBase

  {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {
        Inventory.ValueChange += ValueChange;
        await connection.SetPage(EnumUserPage.Backpack);
      }
    }

    private void ValueChange(object? sender, Common.Events.ContextScopeEventArgs<InventoryDTO> e)
    {
      Console.WriteLine(e.Value);
      StateHasChanged();
    }
    public async Task EquipAsync(Guid? itemId,int? offset=0)
    {
      Console.WriteLine($"{itemId}");
      await this.connection.EquipItem(itemId, offset);

    }
    public async Task UnEquipAsync(EnumEquipmentSlot slot)
    {
      Console.WriteLine($"{slot}");
      await this.connection.UnEquipItem(slot);
    }
    public override void Dispose()
    {
      base.Dispose();
      Inventory.ValueChange -= ValueChange;
    }
  }
}

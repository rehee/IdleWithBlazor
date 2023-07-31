using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IActor : IAsyncDisposable, IDisposable
  {
    Type TypeDiscriminator { get; }
    Guid Id { get; set; }
    string? Name { get; set; }
    Task OnInitialization();
    Task<bool> OnTick();
    IEnumerable<IActor> Children { get; set; }
    [JsonIgnore]
    IActor? Parent { get; }
    void SetParent(IActor? actor);
  }
}

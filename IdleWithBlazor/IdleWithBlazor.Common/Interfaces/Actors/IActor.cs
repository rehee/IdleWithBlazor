using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Common.Interfaces.Actors
{
  public interface IActor : IAsyncDisposable, IDisposable, ITyped, IName
  {
    Guid Id { get; set; }

    Task OnInitialization();
    Task<bool> OnTick(IServiceProvider sp);
    IEnumerable<IActor> Children();
    Task<bool> AddChildrenAsync(params IActor[] actors);
    Task<bool> RemoveChildrenAsync(params Guid[] actorIds);
    [JsonIgnore]
    IActor? Parent { get; }
    void SetParent(IActor? actor);
    void Init(IActor? parent, params object[] setInfo);
  }
}

using IdleWithBlazor.Common.Interfaces.Actors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdleWithBlazor.Model.Actors
{
  public abstract class Actor : IActor
  {
    public abstract Type TypeDiscriminator { get; }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    private ConcurrentDictionary<Guid, IActor>? children { get; set; }
    public virtual IEnumerable<IActor> Children()
    {
      if (children != null)
      {
        foreach (var child in children.Values)
        {
          yield return child;
        }
      }
    }
    public Task<bool> AddChildrenAsync(params IActor[] actors)
    {
      if (actors?.Any() != true)
      {
        return Task.FromResult(false);
      }
      var allAdd = actors.Where(b => children?.TryAdd(b.Id, b) != true).Count() <= 0;
      return Task.FromResult(allAdd);
    }
    public Task<bool> RemoveChildrenAsync(params Guid[] actorIds)
    {
      if (actorIds?.Any() != true)
      {
        return Task.FromResult(false);
      }
      var allAdd = actorIds.Where(b => children?.TryRemove(b, out var item) != true).Count() <= 0;
      return Task.FromResult(allAdd);
    }
    [JsonIgnore]
    public IActor? Parent { get; protected set; }
    public virtual void SetParent(IActor? actor)
    {
      Parent = actor;
    }
    public virtual void Init(IActor? parent, params object[] setInfo)
    {
      SetParent(parent);
      children = new ConcurrentDictionary<Guid, IActor>();
    }
    protected bool IsDisposed { get; set; }
    public virtual void Dispose()
    {
      if (IsDisposed)
      {
        return;
      }
      IsDisposed = true;
      Parent = null;
      children = null;
    }
    public ValueTask DisposeAsync()
    {

      Dispose();
      return ValueTask.CompletedTask;
    }

    public virtual Task OnInitialization()
    {
      return Task.CompletedTask;
    }

    public virtual async Task<bool> OnTick(IServiceProvider sp)
    {
      var results = await Task.WhenAll(Children().Select(b => b?.OnTick(sp)));
      return results?.All(b => b == true) ?? true;
    }
  }
}

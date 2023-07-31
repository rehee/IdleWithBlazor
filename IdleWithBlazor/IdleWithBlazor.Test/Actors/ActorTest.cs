using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Jsons.Converters;
using System.Text.Json;

namespace IdleWithBlazor.Test.Actors
{
  public class ActorTest
  {
    [Test]
    public void Actor_Json_Test()
    {
      var actorList = new IActor[]
      {
        new TestActor(),
        new TestActor2(),
      };
      var option = new JsonSerializerOptions
      {

      };
      option.Converters.Add(new TypeJsonConverter());
      option.Converters.Add(new ActionJsonConverter());
      var json = JsonSerializer.Serialize(actorList, option);
      var obj = JsonSerializer.Deserialize<IActor[]>(json, option);

      Assert.That(obj.Count, Is.EqualTo(2));
      Assert.That(
        obj.Select(b => b.GetType())
        .Where(b => b == typeof(TestActor))
        .Count(), Is.EqualTo(1));
      Assert.That(
        obj.Select(b => b.GetType())
        .Where(b => b == typeof(TestActor2))
        .Count(), Is.EqualTo(1));
    }
  }

  public class TestActor : IActor
  {
    public Type TypeDiscriminator => typeof(TestActor);
    public string Name { get; set; }
    public Guid Id { get; set; }
    public IEnumerable<IActor> Actors { get; set; }
    public IEnumerable<IActor> Children { get; set; }

    public IActor? Parent { get; private set; }

    public void Dispose()
    {

    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    public async Task OnInitialization()
    {
      await Task.CompletedTask;
    }

    public Task<bool> OnTick()
    {
      return Task.FromResult(true);
    }

    public void SetParent(IActor? actor)
    {

    }


  }
  public class TestActor2 : IActor
  {
    public Type TypeDiscriminator => typeof(TestActor2);
    public string Name { get; set; }
    public Guid Id { get; set; }
    public IEnumerable<IActor> Actors { get; set; }
    public IEnumerable<IActor> Children { get; set; }

    public IActor? Parent { get; private set; }

    public void Dispose()
    {

    }

    public ValueTask DisposeAsync()
    {
      Dispose();
      return ValueTask.CompletedTask;
    }

    public async Task OnInitialization()
    {
      await Task.CompletedTask;
    }

    public Task<bool> OnTick()
    {
      return Task.FromResult(true);
    }

    public void SetParent(IActor? actor)
    {

    }
  }
}

using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Common.Jsons.Converters;
using IdleWithBlazor.Model.Actors;
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



  public class TestActor : Actor
  {
    public override Type TypeDiscriminator => typeof(TestActor);
  }
  public class TestActor2 : Actor
  {
    public override Type TypeDiscriminator => typeof(TestActor2);
  }
}

using IdleWithBlazor.Common.Helpers;
using IdleWithBlazor.Common.Interfaces.Actors;
using IdleWithBlazor.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleWithBlazor.Test.Commons.Helpers
{
  public class ActorHelperTest : TestBase
  {
    [Test]
    public void ActorHelper_New_Entity()
    {
      var user = ActorHelper.New<ICharacter>();
      Assert.IsTrue(!user.Id.Equals(Guid.Empty));
      Assert.IsTrue(user.Name == user.Id.ToString());
      var id = Guid.NewGuid();
      var user1 = ActorHelper.New<ICharacter>(id);
      Assert.IsTrue(user1.Id.Equals(id));
      Assert.IsTrue(user1.Name == id.ToString());
      var user2 = ActorHelper.New<ICharacter>(id, "123");
      Assert.IsTrue(user2.Id.Equals(id));
      Assert.IsTrue(user2.Name == "123");
    }

  }
}

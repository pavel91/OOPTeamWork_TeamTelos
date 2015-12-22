using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IEntity : IGameObject, IDamage, IDestroyable
    {
    }
}

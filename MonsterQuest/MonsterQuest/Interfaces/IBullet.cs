using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IBullet : IGameObject,IAttack
    {
        bool IsActive { get; set; }
    }
}

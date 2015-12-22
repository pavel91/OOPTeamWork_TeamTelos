using MonsterQuest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface ICharacter : ICollect
    {
        ICollection<IBullet> Bullets { get; }

        BulletType CurrentWeapon { get; }

        int Gold { get; }
    }
}

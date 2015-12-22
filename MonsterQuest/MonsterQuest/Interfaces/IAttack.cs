using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IAttack : IDamage
    {
        void ApplyDamage(ICollection<Enemy> enemies);
    }
}

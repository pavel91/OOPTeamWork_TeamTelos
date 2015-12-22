using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IDestroyable
    {
        int Health { get; }

        void ReceiveDamage(int damage);

        bool IsAlive { get; }
    }
}

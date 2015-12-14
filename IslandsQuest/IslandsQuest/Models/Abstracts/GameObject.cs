using IslandsQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IslandsQuest.Models.Abstracts
{
    public abstract class GameObject : IIntersect
    {
        public abstract void IntersectWithEnemies(IList<EntityModels.Enemy> enemies, int score);
    }
}

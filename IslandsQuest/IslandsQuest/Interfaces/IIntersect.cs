using IslandsQuest.Models.EntityModels;
using System.Collections.Generic;

namespace IslandsQuest.Interfaces
{
    public interface IIntersect
    {
        void IntersectWithEnemies(IList<Enemy> enemies, int score);
    }
}

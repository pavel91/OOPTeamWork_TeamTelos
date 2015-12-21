using IslandsQuest.Models.EntityModels.Enemies;
using System.Collections.Generic;

namespace IslandsQuest.Interfaces
{
    public interface IIntersect
    {
        void IntersectWithEnemies(IList<Enemy> enemies, int score);
    }
}

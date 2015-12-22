using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Core.Factories
{
   public class EnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(string enemyName, Texture2D enemyImage)
        {
            switch (enemyName)
            {
                case "Skeleton":
                    return new Skeleton(enemyImage);
                default:
                    throw new ArgumentException("Invalid enemy type");
            }
        }
    }
}

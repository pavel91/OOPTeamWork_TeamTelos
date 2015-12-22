using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IEnemyFactory
    {
        Enemy CreateEnemy(string enemyName, Texture2D enemyImage);
    }
}

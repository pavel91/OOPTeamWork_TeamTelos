using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Interfaces
{
    public interface IBulletFactory
    {
        IBullet CreateBullet(string bulletType, Vector2 position, Texture2D bulletImage,BulletDirection bulletDirection);
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Enums;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Core.Factories
{
    public class BulletFactory : IBulletFactory
    {
        public IBullet CreateBullet(string bulletType,Vector2 position,Texture2D bulletImage,BulletDirection bulletDirection)
        {
            switch (bulletType)
            {
                case "RegularBullet":
                    return new RegularBullet(bulletImage,position,bulletDirection);
                default:
                    throw new ArgumentException("Invalid bullet type.");
            }
        }
    }
}

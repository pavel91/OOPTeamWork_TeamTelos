using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Bullets
{
    public class RegularBullet : Bullet
    {
        private Vector2 defaultRegularBulletVelocity = new Vector2(13, 0);
        private const int DefaultRegularBulletDamage = 8;


        public RegularBullet(Texture2D image, Vector2 position,BulletDirection direction)
            : base(image, DefaultRegularBulletDamage, position,direction)
        {
            this.Velocity = defaultRegularBulletVelocity;
        }
    }
}

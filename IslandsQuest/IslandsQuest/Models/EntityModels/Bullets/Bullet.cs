using IslandsQuest.Interfaces;
using IslandsQuest.Models.Abstracts;
using IslandsQuest.Models.EntityModels.Enemies;
using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace IslandsQuest.Models.EntityModels.Bullets
{
    public class Bullet : GameObject, IDamage, IIntersect
    {
        public bool isActive;
        private BulletDirection direction;
        private int damage;
        public int newScore;

        public int Damage { get { return this.damage; } set { this.damage = value; } }
        
        public BulletDirection Direction { get { return this.direction; } set { this.direction = value; } }

        public Bullet(Texture2D bulletImage, float xPos, float yPos, float bulletSpeed, int bulletDamage, BulletDirection direction)
            : base(bulletImage, xPos, yPos, bulletSpeed)
        {
            this.Image = bulletImage;
            this.XPosition = xPos;
            this.YPosition = yPos;
            this.Width = this.Image.Width;
            this.Height = this.Image.Height;
            this.Velocity = bulletSpeed;
            this.Damage = bulletDamage;
            this.Direction = direction;
            this.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (direction == BulletDirection.Right)
            {
                this.XPosition += this.Velocity;
            }
            else
            {
                this.XPosition -= this.Velocity;
            }

            if (this.XPosition > 700 || this.XPosition < -5)
            {
                isActive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //var width = this.Image.Width;
            //var height = this.Image.Height;

            var sourceRectangle = new Rectangle(0, 0, this.Width, this.Height);
            var destinationRectangle = new Rectangle((int)this.XPosition, (int)this.YPosition, this.Width, this.Height);

            this.Bounds = new Rectangle((int)this.XPosition, (int)this.YPosition, this.Width, this.Height);

            spriteBatch.Draw(this.Image, destinationRectangle, sourceRectangle, Color.White);
        }

        public void IntersectWithEnemies(IList<Enemy> enemies, int score)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                newScore = score;
                if (this.Bounds.Intersects(enemies[i].Bounds))
                {
                    this.isActive = false;
                    enemies[i].Health -= this.damage;
                    if (enemies[i].Health <= 0)
                    {
                        enemies.RemoveAt(i);
                        newScore += 20;
                    }
                }
            }
        }
    }
}

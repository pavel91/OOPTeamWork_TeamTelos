using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IslandsQuest.Models.Enums;
using IslandsQuest.Interfaces;
using IslandsQuest.Models.Abstracts;

namespace IslandsQuest.Models.EntityModels
{
    public class Bullet : GameObject, IAttack
    {
        private float velocity;

        public bool isActive;
        private Vector2 location;
        private Texture2D texture;
        private BulletDirection direction;
        private int damage;
        public int newScore;

        public Texture2D Texture { get { return this.texture; } set { this.texture = value; } }
        public Vector2 Location { get { return this.location; } set { this.location = value; } }
        public float Velocity { get { return this.velocity; } set { this.velocity = value; } }
        public int Damage { get { return this.damage; } set { this.damage = value; } }
        public BulletDirection Direction { get { return this.direction; } set { this.direction = value; } }

        public Rectangle Bounds { get; set; }

        public Bullet(Texture2D bulletTexture, Vector2 location, float bulletSpeed, int bulletDamage, BulletDirection direction)
        {
            this.Texture = bulletTexture;
            this.Location = location;
            this.Velocity = bulletSpeed;
            this.Damage = bulletDamage;
            this.Direction = direction;
            this.isActive = true;
        }

        public void Update(GameTime gameTime)
        {

            if (direction == BulletDirection.Right)
            {
                location.X += velocity;
            }
            else
            {
                location.X -= velocity;
            }

            if (location.X > 700 || location.X < -5)
            {
                isActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var width = texture.Width;
            var height = texture.Height;

            var sourceRectangle = new Rectangle(0, 0, width, height);
            var destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            this.Bounds = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public override void IntersectWithEnemies(IList<Enemy> enemies, int score)
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

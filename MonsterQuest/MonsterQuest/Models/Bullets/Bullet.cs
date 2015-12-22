using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Enums;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Bullets
{
    public abstract class Bullet : IBullet
    {
        private Texture2D image;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 velocity;
        private int damage;
        private BulletDirection bulletDirection;
        private bool isActive = true;

        protected Bullet(Texture2D image, int damage, Vector2 position, BulletDirection bulletDirection)
        {
            this.Image = image;
            this.Damage = damage;
            this.Position = position;
            this.bulletDirection = bulletDirection;
        }

        public Texture2D Image { get { return this.image; } protected set { this.image = value; } }

        public Rectangle Bounds { get { return this.bounds; } protected set { this.bounds = value; } }

        public Vector2 Position { get { return this.position; } protected set { this.position = value; } }

        public Vector2 Velocity { get { return this.velocity; } protected set { this.velocity = value; } }

        public int Damage { get { return this.damage; } protected set { this.damage = value; } }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.bulletDirection == BulletDirection.Right)
            {
                this.Position += this.Velocity;
            }
            else
            {
                this.Position -= this.Velocity;
            }

            if (this.Position.X > 800 || this.Position.X < -5)
            {
                isActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var width = this.Image.Width;
            var height = this.Image.Height;

            var sourceRectangle = new Rectangle(0, 0, width, height);
            var destinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, width, height);

            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, width, height);

            spriteBatch.Draw(this.Image, destinationRectangle, sourceRectangle, Color.White);
        }

        public bool CollisionDetected(IGameObject target)
        {
            if (this.bounds.Intersects(target.Bounds))
            {
                return true;
            }
            return false;
        }

        public void ApplyDamage(ICollection<Enemy> enemies)
        {
            
        }


    }
}

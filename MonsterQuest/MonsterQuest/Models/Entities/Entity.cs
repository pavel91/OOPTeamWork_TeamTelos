using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonsterQuest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterQuest.Models.Entities
{
    public abstract class Entity : IEntity, IScore
    {
        private Texture2D image;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 velocity;
        private int width;
        private int height;
        private int health;
        private int damage;
        private int score;

        private bool isAlive = true;

        protected Entity(Texture2D image, int health, int damage)
        {
            this.Image = image;
            this.Bounds = bounds;
            this.Position = new Vector2(0, 330);
            //this.Velocity = velocity;
            this.Health = health;
            this.Damage = damage;

            this.SetBounds();
        }

        public Texture2D Image { get { return this.image; } protected set { this.image = value; } }

        public Rectangle Bounds { get { return this.bounds; } protected set { this.bounds = value; } }

        public Vector2 Position { get { return this.position; } protected set { this.position = value; } }

        public Vector2 Velocity { get { return this.velocity; } protected set { this.velocity = value; } }

        public int Width { get { return this.width; } protected set { this.width = value; } }

        public int Height { get { return this.height; } protected set { this.height = value; } }

        public int Health { get { return this.health; } protected set { this.health = value; } }

        public int Damage { get { return this.damage; } protected set { this.damage = value; } }

        public bool IsAlive
        {
            get { return this.isAlive; }
        }

        public int Score { get { return this.score; } set { this.score = value; } }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Attack();

        //public abstract void ApplyDamage(IGameObject target);

        //To do
        private void SetBounds()
        {
        }

        public bool CollisionDetected(IGameObject target)
        {
            if (this.bounds.Intersects(target.Bounds))
            {
                return true;
            }
            return false;
        }

        public void ReceiveDamage(int damage)
        {
            this.health -= damage;

            if (this.health <= 0)
            {
                this.isAlive = false;
                this.health = 0;
            }
        }
    }
}

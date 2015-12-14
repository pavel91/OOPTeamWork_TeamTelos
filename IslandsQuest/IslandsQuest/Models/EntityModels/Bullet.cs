using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IslandsQuest.Models.Enums;

namespace IslandsQuest.Models.EntityModels
{
    public class Bullet
    {
        private float velocity;
        private int damage;
        public bool isActive;
        private Vector2 location;
        private Texture2D texture;
        private BulletDirection direction;

        public Texture2D Texture { get { return this.texture; } set { this.texture = value; } }
        public Vector2 Location { get { return this.location; } set { this.location = value; } }
        public float Velocity { get { return this.velocity; } set { this.velocity = value; } }
        public int Damage { get { return this.damage; } set { this.damage = value; } }
        public BulletDirection Direction { get { return this.direction; } set { this.direction = value; } }

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
            
            if (location.X > 1000 || location.X < -100  )
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

                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}

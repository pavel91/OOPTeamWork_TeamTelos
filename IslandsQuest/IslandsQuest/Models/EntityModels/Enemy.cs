using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IslandsQuest.Models.EntityModels
{
    public class Enemy
    {
        private const int DefaultEnemyHealth = 75;
        private const int DefaultEnemyDamage = 15;

        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public float XPosition { get; set; }
        public float YPosition { get; set; }

        //not dissappearing logic
        public bool walkingLeft = true;
        public bool hasMadeDamage = false;

        public int Health { get; set; }
        public int Damage { get; set; }

        public bool IsAlive { get; set; }

        public Rectangle Bounds { get; set; }

        public Enemy(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 8;
            totalFrames = Rows * Columns;
            XPosition = 900;
            YPosition = 350;
            this.Health = DefaultEnemyHealth;
            this.Damage = DefaultEnemyDamage;
            this.IsAlive = true;
        }

        public void Update()
        {
            //not dissappearing logic
            if (XPosition<1)
            {
                walkingLeft = false;
            }
            if (XPosition>800)
            {
                walkingLeft = true;
            }
            if (walkingLeft)
            {
                XPosition -= 1.5f;
            }
            else
            {
                XPosition += 1.5f;
            }

            currentFrame++;

            if (currentFrame == 16)
                currentFrame = 8;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;
            Vector2 location = new Vector2(XPosition, YPosition);

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            //Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.Bounds = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, this.Bounds, sourceRectangle, Color.White);

        }
    }
}

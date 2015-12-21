using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace IslandsQuest.Models.EntityModels.Items
{
    public class Gold 
    {
        private Vector2 velocity = new Vector2(0, 10);

        //може да се изнесе в Gift класа
        private int activeTimeLimit = 5000;
        private int activeFrom;
        
        //може да се изнесе в Object -> ползва се и при Character
        private const int minY = 350; 
        private Texture2D sprite;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 boundOffset = new Vector2(15, 10);
        private bool isActive=true;
        private bool isOnTheGround = false;

        public Rectangle Bounds { get { return this.bounds; } }
        public bool IsActive { get { return this.isActive; } set { this.isActive = value; } }

        public Gold(Texture2D sprite)
        {
            this.sprite = sprite;
            Random random = new Random();
            this.activeFrom = 0;
            //вместо 700-> да има поле за широчината на екрана
            int x = random.Next(700);
            position = new Vector2(x, 0);
        }

        public void Update(GameTime gameTime)
        {
            activeFrom += gameTime.ElapsedGameTime.Milliseconds;
            if (activeFrom >activeTimeLimit)
            {
                isActive = false;
            }

            if (isActive)
            {
                if (position.Y >= minY)
                {
                    isOnTheGround = true;
                }
                if (!isOnTheGround)
                {
                    this.position += velocity;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                var width = sprite.Width / 12;
                var height = sprite.Height / 8;
                int row = 6;
                int column = 8;

                var sourceRectangle = new Rectangle(width * column, height * row, width, height);
                var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

                this.bounds = new Rectangle((int)position.X + (int)boundOffset.X,
                                (int)position.Y + (int)boundOffset.Y,
                                width - 2 * (int)boundOffset.X,
                                height - 2 * (int)boundOffset.Y);

                spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);               
            }
        }
    }
}

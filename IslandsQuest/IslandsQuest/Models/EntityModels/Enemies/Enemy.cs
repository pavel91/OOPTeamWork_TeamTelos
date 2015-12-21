using IslandsQuest.Interfaces;
using IslandsQuest.Models.Abstracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IslandsQuest.Models.EntityModels.Enemies
{
    public class Enemy : GameObject, ICharacter, IDamage
    {
        private const int DefaultEnemyHealth = 75;
        private const int DefaultEnemyDamage = 15;
        private const float DefaultEnemyVelocity = 2.5f;

        //These two are used to cut the skeleton sprite
        private const int DefaultImageSpriteRows = 8;
        private const int DefaultImageSpriteColumns = 9;

        //public Texture2D Texture { get; set; }

        //public Rectangle Bounds { get; set; }

        //public int Rows { get; set; }
        //public int Columns { get; set; }

        private int currentFrame;
        //private int totalFrames;

        //public float XPosition { get; set; }
        //public float YPosition { get; set; }

        private int timeSinceLastFrame = 0;
        private int millisecondPerFrame = 50;

        //not dissappearing logic
        public bool walkingLeft = true;
        public bool hasMadeDamage = false;

        public int Health { get; set; }

        public int Damage { get; set; }

        public bool IsAlive { get; set; }

        public Enemy(Texture2D enemyImage, int xPos, int yPos, float velocity)
            : base(enemyImage, xPos, yPos, DefaultEnemyVelocity)
        {
            this.Image = enemyImage;
            //Rows = rows;
            //Columns = columns;
            //totalFrames = Rows * Columns;
            //XPosition = 900;
            //YPosition = 330;
            //this.XPosition = xPos;
            //this.YPosition = yPos;
            //this.Velocity = velocity;
            this.Health = DefaultEnemyHealth;
            this.Damage = DefaultEnemyDamage;
            this.IsAlive = true;
            currentFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondPerFrame)
            {

                //not dissappearing logic
                if (XPosition < 1)
                {
                    walkingLeft = false;
                }
                if (XPosition > 800)
                {
                    walkingLeft = true;
                }

                if (walkingLeft)
                {
                    if (currentFrame < 18 || currentFrame > 23)
                    {
                        currentFrame = 19;
                    }

                    //velocity - 2.5f
                    //XPosition -= 2.5f;
                    this.XPosition -= this.Velocity;
                }
                else
                {
                    if (currentFrame < 55 || currentFrame > 60)
                    {
                        currentFrame = 56;
                    }

                    //velocity - 2.5f
                    //XPosition += 2.5f;
                    this.XPosition += this.Velocity;
                }

                currentFrame++;
                timeSinceLastFrame -= millisecondPerFrame;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int width = this.Image.Width / DefaultImageSpriteColumns;
            int height = this.Image.Height / DefaultImageSpriteRows;
            int row = (int)((float)currentFrame / (float)DefaultImageSpriteColumns);
            int column = currentFrame % DefaultImageSpriteColumns;
            Vector2 location = new Vector2(XPosition, YPosition);

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            //Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.Bounds = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(this.Image, this.Bounds, sourceRectangle, Color.White);

        }
    }
}

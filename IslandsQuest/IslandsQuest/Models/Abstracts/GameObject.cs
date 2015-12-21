using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IslandsQuest.Models.Abstracts
{
    public abstract class GameObject
    {
        public Texture2D Image { get; set; }

        public Rectangle Bounds { get; set; }

        //public Vector2 Position { get; set; }

        public float XPosition { get; set; }

        public float YPosition { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public float Velocity { get; set; }

        protected GameObject(Texture2D image, float xPos, float yPos, float velocity)
        {
            this.Image = image;
            this.XPosition = xPos;
            this.YPosition = yPos;
            this.Velocity = velocity;
        }

        //public virtual void Initialize(Texture2D texture, float XPos, float YPos)
        //{
        //    this.Image = texture;
        //    this.XPosition = XPos;
        //    this.YPosition = YPos;
        //}

        public virtual void Update(GameTime gametime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}

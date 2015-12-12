using IslandsQuest.Models.EntityModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IslandsQuest
{
    public class IslandsQuest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Character character;
        private Texture2D sprite;
        private Texture2D backgroundLevel1;
        private Vector2 location;

        public IslandsQuest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            location = new Vector2(0, 260);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("gb_walk");
            backgroundLevel1 = this.Content.Load<Texture2D>("overworld_bg");

            character = new Character(sprite, location);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            character.Update(gameTime, location);
            location = character.CharacterPosition;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);
            character.Draw(spriteBatch, location);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
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
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        List<Enemy> enemies;
        private Character character;
        private Texture2D sprite;
        private Texture2D backgroundLevel1;
        private Vector2 location;
        private Enemy enemy;

        public IslandsQuest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            location = new Vector2(0, 260);
            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("gb_walk");
            backgroundLevel1 = this.Content.Load<Texture2D>("space_background");
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
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
            }
            UpdateEnemies(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);
            character.Draw(spriteBatch, location);
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {

                previousSpawnTime = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("monster");
                enemy = new Enemy(texture, 2, 8);
                enemies.Add(enemy);
            }
        }
    }
}

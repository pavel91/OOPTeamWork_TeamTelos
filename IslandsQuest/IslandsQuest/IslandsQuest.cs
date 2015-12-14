using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IslandsQuest.Models.EntityModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IslandsQuest
{
    public class IslandsQuest : Game
    {
        private int PlayerSpriteColumns = 6;
        private int PlayerSpriteRows = 3;

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
        private SpriteFont titleFont;

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
            titleFont = Content.Load<SpriteFont>("title");
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

            //Check for collision and do damage
            UpdateCollision(gameTime);

            UpdateEnemies(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.DrawString(titleFont, string.Format("Health: {0}", this.character.health), new Vector2(5, 5), Color.White);
            character.Draw(spriteBatch, location);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsAlive)
                {
                    enemies[i].Draw(spriteBatch);
                }
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

            foreach (var enemy in enemies)
            {
                enemy.Update();
            }
        }

        //The collision method
        private void UpdateCollision(GameTime gameTime)
        {
            BoundingBox playerBounds;
            var playerWidth = this.character.sprite.Width / PlayerSpriteColumns;
            var playerHeight = this.character.sprite.Height / PlayerSpriteRows;
            Rectangle enemyBounds;

            //playerBounds = new BoundingBox(new Vector3(this.character.CharacterPosition.X, this.character.CharacterPosition.Y, 0),
            //                              new Vector3(this.character.CharacterPosition.X + playerWidth, this.character.CharacterPosition.Y + playerHeight, 0));

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemyWidth = enemies[i].Texture.Width / enemy.Columns;
                var enemyHeight = enemies[i].Texture.Height / enemy.Rows;

                //enemyBounds = new BoundingBox(new Vector3(enemies[i].XPosition, enemies[i].YPosition, 0),
                //                            new Vector3(enemies[i].XPosition + enemyWidth, enemies[i].YPosition + enemyHeight, 0));
                enemyBounds = new Rectangle();

                if (this.character.Bounds.Intersects(enemies[i].Bounds))
                {
                    this.character.health -= enemies[i].Damage;
                    enemies.RemoveAt(i);
                }
            }
        }
    }
}

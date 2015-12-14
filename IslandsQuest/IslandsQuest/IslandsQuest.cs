using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IslandsQuest.Models.EntityModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IslandsQuest.Models.Enums;

namespace IslandsQuest
{
    public class IslandsQuest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        IList<Enemy> enemies;
        private Character character;
        private Texture2D sprite;
        private Texture2D bulletTexture;
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
            location = new Vector2(0, 330);
            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(6.0f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("transparentElf");
            backgroundLevel1 = this.Content.Load<Texture2D>("space_background");

            titleFont = Content.Load<SpriteFont>("title");
            bulletTexture = this.Content.Load<Texture2D>("Fireball");

            character = new Character(sprite, location, bulletTexture);
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

            character.Update(gameTime);
            character.IntersectWithEnemies(enemies, this.character.Score);

            //Check for collision and do damage
            //UpdateCollision(gameTime);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
            }
            UpdateEnemies(gameTime);

            ICollection<Bullet> activeBullets = new List<Bullet>();
            foreach (var bullet in character.Bullets)
            {
                bullet.Update(gameTime);
                bullet.IntersectWithEnemies(enemies, this.character.Score);
                if (bullet.newScore>this.character.Score)
                {
                    this.character.Score = bullet.newScore;
                }
                
                if (bullet.isActive)
                {
                    activeBullets.Add(bullet);
                }
            }
            character.Bullets = activeBullets;

            this.Window.Title = enemies.Count.ToString();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draw background
            spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);

            spriteBatch.DrawString(titleFont, string.Format("Health: {0}", this.character.health), new Vector2(5, 5), Color.White);
            spriteBatch.DrawString(titleFont, string.Format("Score: {0}", this.character.Score), new Vector2(150, 5), Color.White);

            //Draw Hero
            character.Draw(spriteBatch);

            //Draw Enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsAlive)
                {
                    enemies[i].Draw(spriteBatch);
                }
            }

            //Draw bullets
            foreach (var bullet in character.Bullets)
            {
                if (bullet.isActive)
                {
                    bullet.Draw(spriteBatch);
                }
                else
                {
                    character.Bullets.Remove(bullet);
                }
            }
            //bullet.Draw(spriteBatch);
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
    }
}

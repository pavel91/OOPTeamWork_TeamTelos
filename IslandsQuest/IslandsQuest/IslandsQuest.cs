using System;
using System.Collections;
using System.Collections.Generic;
using IslandsQuest.Models.Core;
using System.Linq;
using IslandsQuest.Models.EntityModels;
using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IslandsQuest.Models.EntityModels.Items;

namespace IslandsQuest
{
    public class IslandsQuest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TimeSpan enemySpawnTime;
        TimeSpan giftSpawnTime;
        TimeSpan goldSpawnTime;

        TimeSpan previousSpawnTime;
        TimeSpan previousSpawnTimePotion;
        TimeSpan previousSpawnTimeGold;
        
        IList<Enemy> enemies;
        List<Potion> potions;
        List<Gold> gold;

        private Character character;
        private Texture2D sprite;
        private Texture2D bulletTexture;
        private Texture2D backgroundLevel0;
        private Texture2D backgroundLevel1;
        private Vector2 location;
        
        private Enemy enemy;
        private Potion potion;
        private Gold oneGold;
        
        private SpriteFont titleFont;
        private Level level;

        private Rectangle startArea = new Rectangle(50, 20, 160, 90);
        private Rectangle gameArea = new Rectangle(230, 20, 160, 90);
        private Rectangle rulesArea = new Rectangle(410, 20, 160, 90);
        private Rectangle creditsArea = new Rectangle(590, 20, 160, 90);
        private Texture2D button;

        private EventListener listener;

        public IslandsQuest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.level = Level.Initial;
            location = new Vector2(0, 330);
            enemies = new List<Enemy>();
            potions = new List<Potion>();
            gold = new List<Gold>();

            previousSpawnTime = TimeSpan.Zero;
            previousSpawnTimePotion = TimeSpan.Zero;
            previousSpawnTimeGold = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(6.0f);
            
            //da se iznesat
            giftSpawnTime = TimeSpan.FromSeconds(5.0f);
            goldSpawnTime = TimeSpan.FromSeconds(8.0f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("transparentElf");
            backgroundLevel0 = this.Content.Load<Texture2D>("level0_background");
            button = this.Content.Load<Texture2D>("buttons_background");
            backgroundLevel1 = this.Content.Load<Texture2D>("space_background");

            titleFont = Content.Load<SpriteFont>("title");
            bulletTexture = this.Content.Load<Texture2D>("Fireball");

            character = new Character(sprite, location, bulletTexture);

            listener = new EventListener(character, this.level);

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
            DetectClick();
            character.Update(gameTime);
            character.IntersectWithEnemies(enemies, this.character.Score);

            character.IntersectWithPotions(potions, this.character.health);
            character.IntersectWithGold(gold);

            //Check for collision and do damage
            //UpdateCollision(gameTime);
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
            }
            UpdateEnemies(gameTime);
            
            //same logic for every Item
            List<Potion> аctivePotions = new List<Potion>();
            for (int i = 0; i < potions.Count; i++)
            {
                potions[i].Update(gameTime);
                if (potions[i].IsActive)
                {
                    аctivePotions.Add(potions[i]);
                }
            }
            potions = аctivePotions;

            //same logic for every Item
            List<Gold> аctiveGold = new List<Gold>();
            for (int i = 0; i < gold.Count; i++)
            {
                gold[i].Update(gameTime);
                if (gold[i].IsActive)
                {
                    аctiveGold.Add(gold[i]);
                }
            }
            gold = аctiveGold;

            ICollection<Bullet> activeBullets = new List<Bullet>();
            foreach (var bullet in character.Bullets)
            {
                bullet.Update(gameTime);
                bullet.IntersectWithEnemies(enemies, this.character.Score);
                if (bullet.newScore > this.character.Score)
                {
                    this.character.Score = bullet.newScore;
                }

                if (bullet.isActive)
                {
                    activeBullets.Add(bullet);
                }
            }
            character.Bullets = activeBullets;

            //this.Window.Title = character.keys.ToString();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (listener.GameLevel == Level.GameOver)
            {
                this.DrawGameOver();
            }
            else
            {
                if (level == Level.Initial)
                {
                    this.DrawInitialBackground();
                }
                if (level == Level.Game)
                {
                    this.DrawInitialBackground();
                    this.DrawGame();
                }
                if (level == Level.Credits)
                {
                    this.DrawInitialBackground();
                    this.DrawCredits();
                }
                if (level == Level.Rules)
                {
                    this.DrawInitialBackground();
                    this.DrawRules();
                }

                if (level == Level.First)
                {
                    //Draw background
                    spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);

                    spriteBatch.DrawString(titleFont, string.Format("Health: {0}", this.character.health),
                        new Vector2(5, 5),
                        Color.White);
                    spriteBatch.DrawString(titleFont, string.Format("Score: {0}", this.character.Score),
                        new Vector2(150, 5),
                        Color.White);
                    spriteBatch.DrawString(titleFont, string.Format("Gold: {0}", this.character.gold),
                        new Vector2(270, 5),
                        Color.White);

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

                    //draw potions
            foreach (var item in potions)
            {
                item.Draw(spriteBatch);
            }

            //draw gold
            foreach (var item in gold)
            {
                item.Draw(spriteBatch);
            }
                    //bullet.Draw(spriteBatch);

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
                Texture2D texture = Content.Load<Texture2D>("skeleton");

                enemy = new Enemy(texture, 8, 9);

                enemies.Add(enemy);
            }

            if (gameTime.TotalGameTime - previousSpawnTimePotion > giftSpawnTime)
            {
                previousSpawnTimePotion = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("transparentItems");

                potion = new Potion(texture);

                potions.Add(potion);
            }

            if (gameTime.TotalGameTime - previousSpawnTimeGold > goldSpawnTime)
            {
                previousSpawnTimeGold = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("transparentItems");

                oneGold = new Gold(texture);

                gold.Add(oneGold);
            }

        }

        private void DetectClick()
        {
            MouseInput.LastMouseState = MouseInput.MouseState;
            MouseInput.MouseState = Mouse.GetState();
            Point mousePos = new Point(MouseInput.getMouseX(), MouseInput.getMouseY());
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released && 
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                startArea.Contains(mousePos))
            {
                level= Level.First;
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                creditsArea.Contains(mousePos))
            {
                level=Level.Credits;
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                rulesArea.Contains(mousePos))
            {
                level = Level.Rules;
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                gameArea.Contains(mousePos))
            {
                level = Level.Game;
            }
        }

        private void DrawInitialBackground()
        {
            this.IsMouseVisible = true;
            spriteBatch.Draw(backgroundLevel0, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(button, startArea, Color.White);
            spriteBatch.Draw(button, gameArea, Color.White);
            spriteBatch.Draw(button, rulesArea, Color.White);
            spriteBatch.Draw(button, creditsArea, Color.White);
            spriteBatch.DrawString(titleFont, "P L A Y", new Vector2(90, 50), Color.Blue);
            spriteBatch.DrawString(titleFont, "G A M E", new Vector2(265, 50), Color.Blue);
            spriteBatch.DrawString(titleFont, "R U L E S", new Vector2(440, 50), Color.Blue);
            spriteBatch.DrawString(titleFont, "C R E D I T S", new Vector2(599, 50), Color.Blue);
            spriteBatch.DrawString(titleFont, "M  O  N  S  T  E  R    Q  U  E  S  T", new Vector2(220, 440), Color.Blue);
        }

        private void DrawCredits()
        {
            spriteBatch.DrawString(titleFont, "A N T O N   V E L I K O V", new Vector2(50, 150), Color.Blue);
            spriteBatch.DrawString(titleFont, "P A V E L   S H A L E V", new Vector2(50, 210), Color.Blue);
            spriteBatch.DrawString(titleFont, "P L A M E N A   M I T E V A", new Vector2(50, 270), Color.Blue);
        }

        private void DrawRules()
        {
            spriteBatch.DrawString(titleFont, "Press RIGHT ARROW to move right", new Vector2(50, 150), Color.Blue);
            spriteBatch.DrawString(titleFont, "Press LEFT ARROW to move left", new Vector2(50, 210), Color.Blue);
            spriteBatch.DrawString(titleFont, "Press UP ARROW to jump", new Vector2(50, 270), Color.Blue);
            spriteBatch.DrawString(titleFont, "Press SPACE to shoot", new Vector2(50, 330), Color.Blue);
        }

        private void DrawGame()
        {
            spriteBatch.DrawString(titleFont, "From the beginning of time, mankind has been riveted by accounts of mysterious", new Vector2(30, 120), Color.Blue);
            spriteBatch.DrawString(titleFont, "creatures, from mega hogs to vampires and giant spiders.", new Vector2(30, 150), Color.Blue);
        }

        private void DrawGameOver()
        {
            spriteBatch.Draw(backgroundLevel0, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.DrawString(titleFont, "G  A  M  E   O  V  E  R", new Vector2(270, 80), Color.Blue);
            spriteBatch.DrawString(titleFont, "M  O  N  S  T  E  R    Q  U  E  S  T", new Vector2(220, 440), Color.Blue);
        }

        //public void HandlePointChanged(object sender, EventArgs eventArgs)
        //{
        //    level=0;
        //}
    }
}

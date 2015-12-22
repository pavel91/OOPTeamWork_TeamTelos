using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonsterQuest.Core.Data;
using MonsterQuest.Core.Factories;
using MonsterQuest.Interfaces;
using MonsterQuest.Models.Entities.Characters;
using MonsterQuest.Models.Entities.Enemies;
using MonsterQuest.Models.Items;
using System;
using System.Collections.Generic;
using MonsterQuest.Core;
using MonsterQuest.Struct;

namespace MonsterQuest
{
    public class MonsterQuest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private IBulletFactory bulletFactory;
        private IEnemyFactory enemyFactory;
        private IItemFactory itemFactory;
        private IData data;
        private Character player;

        private Texture2D playerImage;
        private Texture2D bulletImage;
        private Texture2D backgroundLevel0;
        private Texture2D backgroundLevel1;
        private Texture2D buttonImage;
        private Texture2D enemyImage;
        private SpriteFont titleFont;

        TimeSpan enemySpawnTime;
        TimeSpan giftSpawnTime;
        TimeSpan goldSpawnTime;

        TimeSpan previousSpawnTime;
        TimeSpan previousSpawnTimePotion;
        TimeSpan previousSpawnTimeGold;

        private Rectangle startArea = new Rectangle(50, 20, 160, 90);
        private Rectangle gameArea = new Rectangle(230, 20, 160, 90);
        private Rectangle rulesArea = new Rectangle(410, 20, 160, 90);
        private Rectangle creditsArea = new Rectangle(590, 20, 160, 90);

        private EventListener listener;

        private Enemy enemy;
        private IItem gold;
        private IItem potion;
        private GameState gameState;

        public MonsterQuest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
                this.bulletFactory = new BulletFactory();
                this.enemyFactory = new EnemyFactory();
                this.itemFactory = new ItemFactory();
                this.data = new Data();
                gameState = new GameState(0);
                previousSpawnTime = TimeSpan.Zero;
                previousSpawnTimePotion = TimeSpan.Zero;
                previousSpawnTimeGold = TimeSpan.Zero;
                enemySpawnTime = TimeSpan.FromSeconds(6.0f);
                giftSpawnTime = TimeSpan.FromSeconds(5.0f);
                goldSpawnTime = TimeSpan.FromSeconds(8.0f);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImage = this.Content.Load<Texture2D>("Images/transparentElf");
            backgroundLevel0 = this.Content.Load<Texture2D>("Images/level0_background");
            buttonImage = this.Content.Load<Texture2D>("Images/buttons_background");
            backgroundLevel1 = this.Content.Load<Texture2D>("Images/space_background");
            enemyImage = this.Content.Load<Texture2D>("Images/skeleton");

            titleFont = Content.Load<SpriteFont>("Fonts/title");
            bulletImage = this.Content.Load<Texture2D>("Images/Fireball");
            this.data.AddBulletImage(bulletImage);

            player = new Elf(playerImage, bulletFactory, data, new Vector2(25, 15), new Vector2(8, 0), new Vector2(0, 330));
            listener = new EventListener(player);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            this.DetectClick();
            if (gameState.Level == 1  && !listener.GameOver )
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                //this.DetectClick();
                this.GenerateObjects(gameTime);

                player.Update(gameTime);
                //character.IntersectWithEnemies(enemies, this.character.Score);
                foreach (var enemy in this.data.Enemies)
                {
                    enemy.Update(gameTime);
                    var isColided = player.CollisionDetected(enemy);
                    if (isColided && !enemy.HasAppliedDamage)
                    {
                        player.ReceiveDamage(enemy.Damage);
                        enemy.HasAppliedDamage = true;
                    }
                    else if (!isColided && enemy.HasAppliedDamage)
                    {
                        enemy.HasAppliedDamage = false;
                    }
                }

                foreach (var item in this.data.Items)
                {
                    item.Update(gameTime);
                    var isColided = player.CollisionDetected(item);
                    if (isColided && item.IsActive)
                    {
                        player.CollectItem(item);
                        item.IsActive = false;
                    }
                }

                foreach (var bullet in player.Bullets)
                {
                    bullet.Update(gameTime);

                    foreach (var enemy in this.data.Enemies)
                    {
                        if (bullet.CollisionDetected(enemy))
                        {
                            enemy.ReceiveDamage(bullet.Damage);
                            bullet.IsActive = false;
                            if (!enemy.IsAlive)
                            {
                                this.player.IncrementScore(enemy.Score);
                            }
                        }
                    }

                    bullet.ApplyDamage(this.data.Enemies);
                }

                this.Window.Title = this.data.Enemies.Count.ToString();

                this.data.RemoveInactiveElements();
                this.player.RemoveInactiveBullets();
                base.Update(gameTime);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (listener.GameOver)
            {
                this.DrawGameOver();
            }
            else
            {
                if (gameState.Level == 0)
                {
                    this.DrawInitialBackground();
                }
                else if (gameState.Level == 2)
                {
                    this.DrawInitialBackground();
                    this.DrawGame();
                }
                else if (gameState.Level == 3)
                {
                    this.DrawInitialBackground();
                    this.DrawCredits();
                }
                else if (gameState.Level == 4)
                {
                    this.DrawInitialBackground();
                    this.DrawRules();
                }
                else if (gameState.Level == 1)
                {
                    spriteBatch.Draw(backgroundLevel1, new Rectangle(0, 0, 800, 480), Color.White);

                    spriteBatch.DrawString(titleFont, string.Format("Health: {0}", this.player.Health),
                        new Vector2(5, 5),
                        Color.White);
                    spriteBatch.DrawString(titleFont, string.Format("Score: {0}", this.player.Score),
                        new Vector2(150, 5),
                        Color.White);
                    spriteBatch.DrawString(titleFont, string.Format("Gold: {0}", this.player.Gold),
                        new Vector2(270, 5),
                        Color.White);

                    //Draw Hero
                    this.player.Draw(spriteBatch);

                    //Draw Enemies
                    foreach (var enemy in this.data.Enemies)
                    {
                        enemy.Draw(spriteBatch);
                    }

                    //Draw bullets
                    foreach (var bullet in this.player.Bullets)
                    {
                        bullet.Draw(spriteBatch);
                    }

                    foreach (var item in this.data.Items)
                    {
                        item.Draw(spriteBatch);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void GenerateObjects(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                enemy = this.enemyFactory.CreateEnemy("Skeleton", enemyImage);

                //enemy = new Enemy(texture, 900, 330, 2.5f);

                this.data.AddEnemies(enemy);
            }

            if (gameTime.TotalGameTime - previousSpawnTimePotion > giftSpawnTime)
            {
                previousSpawnTimePotion = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("Images/transparentItems");

                potion = this.itemFactory.CreateItem("Potion", texture);

                this.data.AddItems(potion);
            }

            if (gameTime.TotalGameTime - previousSpawnTimeGold > goldSpawnTime)
            {
                previousSpawnTimeGold = gameTime.TotalGameTime;
                Texture2D texture = Content.Load<Texture2D>("Images/transparentItems");

                gold = this.itemFactory.CreateItem("Gold", texture);

                this.data.AddItems(gold);
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
                gameState = new GameState(1);
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                creditsArea.Contains(mousePos))
            {
                gameState = new GameState(3);
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                rulesArea.Contains(mousePos))
            {
                gameState = new GameState(4);
            }
            if (MouseInput.LastMouseState.LeftButton == ButtonState.Released &&
                MouseInput.MouseState.LeftButton == ButtonState.Pressed &&
                gameArea.Contains(mousePos))
            {
                gameState = new GameState(2);
            }
        }

        private void DrawInitialBackground()
        {
            this.IsMouseVisible = true;
            spriteBatch.Draw(backgroundLevel0, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(buttonImage, startArea, Color.White);
            spriteBatch.Draw(buttonImage, gameArea, Color.White);
            spriteBatch.Draw(buttonImage, rulesArea, Color.White);
            spriteBatch.Draw(buttonImage, creditsArea, Color.White);
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
            spriteBatch.DrawString(titleFont, "Your objective is to avoid intersecting monsters and kill as many of them as", new Vector2(30, 180), Color.Blue);
            spriteBatch.DrawString(titleFont, "possible in this haunted forest.", new Vector2(30, 210), Color.Blue);
            spriteBatch.DrawString(titleFont, "On your way through the forest collect some magical items to help you", new Vector2(30, 240), Color.Blue);
            spriteBatch.DrawString(titleFont, "survive on your journey.", new Vector2(30, 270), Color.Blue);
        }

        private void DrawGameOver()
        {
            spriteBatch.Draw(backgroundLevel0, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.DrawString(titleFont, "G  A  M  E   O  V  E  R", new Vector2(270, 80), Color.Blue);
            spriteBatch.DrawString(titleFont, "M  O  N  S  T  E  R    Q  U  E  S  T", new Vector2(220, 440), Color.Blue);
        }
    }
}

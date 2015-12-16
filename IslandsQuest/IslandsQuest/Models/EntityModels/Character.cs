using System;
using System.Collections.Generic;
using IslandsQuest.Models.Abstracts;
using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IslandsQuest.Models.EntityModels.Items;

namespace IslandsQuest.Models.EntityModels
{
    public delegate void GameOverEventHandler(object sender, EventArgs e);


    public class Character : GameObject
    {
        private const int DefaultPlayerHealth = 100;
        private const int DefaultPlayerScore = 0;

        public int health;
        public int gold;
        private Texture2D sprite;
        private Texture2D bulletTexture;
        private Vector2 characterPosition;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private HeroState characterState;
        private ICollection<Bullet> bullets;
        private Vector2 boundOffset = new Vector2(25, 15);
        public int Score { get; set; }

        private int currentFrame;
        private int firstFrame;
        private int lastFrame;

        public Rectangle Bounds { get; set; }

        private int timeSinceLastFrame = 0;
        private int millisecondPerFrame = 80;

        //set-vat se ruchno
        private int Columns = 5;
        private int Rows = 4;

        bool jumping;
        float startY, jumpspeed = 0;

        //enemy not die when intersect logic
        


        public ICollection<Bullet> Bullets { get { return this.bullets; } set { this.bullets = value; } }
        public Vector2 CharacterPosition { get { return this.characterPosition; } }

        public Character(Texture2D sprite, Vector2 location, Texture2D bulletTexture)
        {
            this.sprite = sprite;
            this.characterPosition = location;
            this.bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
            this.health = DefaultPlayerHealth;
            this.Score = DefaultPlayerScore;
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondPerFrame)
            {
                //Moveing Logic
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    characterState = HeroState.WalkingLeft;
                    SetFrames(characterState);
                    currentFrame++;
                    characterPosition.X -= 8;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    characterState = HeroState.WalkingRight;
                    SetFrames(characterState);
                    currentFrame++;
                    characterPosition.X += 8;
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == HeroState.WalkingRight)
                {
                    characterState = HeroState.StandingRight;
                }
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState == HeroState.WalkingLeft)
                {
                    characterState = HeroState.StandingLeft;
                }

                //Jumping logic
                if (jumping)
                {
                    startY = 330;
                    characterPosition.Y += jumpspeed;//Making it go up
                    jumpspeed += 5;
                    if (characterPosition.Y >= startY)
                    //If it's farther than ground
                    {
                        characterPosition.Y = startY;//Then set it on
                        jumping = false;
                    }
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        jumping = true;
                        jumpspeed = -34;//Give it upward thrust
                    }
                }

                //Shooting logic
                if (keyboardState.IsKeyDown(Keys.X) && previousKeyboardState.IsKeyUp(Keys.X))
                {
                    BulletDirection bulletDirection;
                    if (characterState == HeroState.StandingLeft || characterState == HeroState.WalkingLeft)
                    {
                        bulletDirection = BulletDirection.Left;
                    }
                    else
                    {
                        bulletDirection = BulletDirection.Right;
                    }

                    Bullet bullet = new Bullet(bulletTexture, characterPosition, 13f, 8, bulletDirection);

                    bullets.Add(bullet);
                }

                previousKeyboardState = keyboardState;
                timeSinceLastFrame -= millisecondPerFrame;

                if (currentFrame == lastFrame)
                {
                    currentFrame = firstFrame;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var width = sprite.Width / Columns;
            var height = sprite.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            this.Bounds = new Rectangle((int)characterPosition.X+(int)boundOffset.X,
                                            (int)characterPosition.Y+(int)boundOffset.Y, 
                                            width-2*(int)boundOffset.X,
                                            height-2*(int)boundOffset.Y);


            if (characterState == HeroState.StandingLeft)
            {
                var sourceRectangle = new Rectangle(width * 0, height * 0, width, height);
                var destinationRectangle = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, width, height);

                spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
            }
            else if (characterState == HeroState.StandingRight)
            {
                var sourceRectangle = new Rectangle(width * 4, height * 1, width, height);
                var destinationRectangle = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, width, height);

                spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
            }
            else
            {
                var sourceRectangle = new Rectangle(width * column, height * row, width, height);
                var destinationRectangle = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, width, height);
                spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        private void SetFrames(HeroState state)
        {
            if (state == HeroState.WalkingRight)
            {
                firstFrame = 5;
                lastFrame = 10;
                if (currentFrame <= 5 && currentFrame > 10)
                {
                    currentFrame = firstFrame;
                }
            }
            else if (state == HeroState.WalkingLeft)
            {
                firstFrame = 0;
                lastFrame = 5;
                if (currentFrame > 5)
                {
                    currentFrame = firstFrame;
                }
            }
        }

        //??? Да се интерсектва с Object-и -> метода да върши работа за интерсектване с Enemy and Items
        public override void IntersectWithEnemies(IList<Enemy> enemies, int score)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (this.Bounds.Intersects(enemies[i].Bounds) && !enemies[i].hasMadeDamage)
                {
                    this.health -= enemies[i].Damage;
                    enemies[i].hasMadeDamage = true;
                }
                else if (!this.Bounds.Intersects(enemies[i].Bounds) && enemies[i].hasMadeDamage)
                {
                    enemies[i].hasMadeDamage = false;
                }

                if (this.health < 0)
                {
                    OnPointChanged(EventArgs.Empty);
                }

            }
        }

        public void IntersectWithPotions(List<Potion> potions, int health)
        {
            foreach (var potion in potions)
            {
                if (this.Bounds.Intersects(potion.Bounds))
                {
                    this.health += potion.HealthPoints;
                    potion.IsActive = false;
                }

            }
        }

        public void IntersectWithGold(List<Gold> gold)
        {
            foreach (var oneGold in gold)
            {
                if (this.Bounds.Intersects(oneGold.Bounds))
                {
                    this.gold += 1;
                    oneGold.IsActive = false;
                }
            }
        }
         
        public event GameOverEventHandler PointChanged;

        protected virtual void OnPointChanged(EventArgs e)
        {
            if (PointChanged != null)
                PointChanged(this, e);
        }

        //public void OnPointChanged()
        //{
        //    if (PointChanged != null)
        //    {
        //        PointChanged(this, EventArgs.Empty);
        //    }
        //}
    }
}

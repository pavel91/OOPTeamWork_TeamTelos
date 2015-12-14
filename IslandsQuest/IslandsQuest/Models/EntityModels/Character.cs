﻿using IslandsQuest.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace IslandsQuest.Models.EntityModels
{
    class Character
    {
        private Texture2D sprite;
        private Texture2D bulletTexture;
        private Vector2 characterPosition;
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private HeroState characterState;
        private ICollection<Bullet> bullets;

        private int currentFrame;
        private int firstFrame;
        private int lastFrame;

        private int timeSinceLastFrame = 0;
        private int millisecondPerFrame = 80;

        //set-vat se ruchno
        private int Columns = 5;
        private int Rows = 4;

        bool jumping; 
        float startY, jumpspeed = 0;

        public ICollection<Bullet> Bullets { get { return this.bullets; } set { this.bullets = value; } }
        public Vector2 CharacterPosition { get { return this.characterPosition; } }

        public Character(Texture2D sprite,Vector2 location,Texture2D bulletTexture)
        {
            this.sprite = sprite;
            this.characterPosition=location;
            this.bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame>millisecondPerFrame)
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
                else if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left) && characterState==HeroState.WalkingRight)
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
                    startY = 260;
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
                        jumpspeed = -24;//Give it upward thrust
                    }
                }

                //Shooting logic
                if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C))
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

                    Bullet bullet = new Bullet(bulletTexture, characterPosition, 13f, 5, bulletDirection);

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

            if (characterState==HeroState.StandingLeft)
            {
                var sourceRectangle = new Rectangle(width * 0, height * 0, width, height);
                var destinationRectangle = new Rectangle((int)characterPosition.X, (int)characterPosition.Y, width, height);

                spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
            }
            else if (characterState==HeroState.StandingRight)
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
                if (currentFrame <=5 && currentFrame > 10)
                {
                    currentFrame = firstFrame;
                }
            }
            else if (state == HeroState.WalkingLeft)
            {
                firstFrame = 0;
                lastFrame = 5;
                if (currentFrame> 5)
                {
                    currentFrame = firstFrame;
                }
            }
        }
    }
}
